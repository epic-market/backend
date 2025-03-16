using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EpicMarket.Entities.CustomModels;
using Microsoft.CodeAnalysis;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using EpicMarket.Entities.Constants;
using EpicMarket.Admin.MVC.Attributes;

namespace EpicMarket.Admin.MVC.Controllers
{
    [SecurableAuthorize(SecurableConstants.CatalogsView)]
    public class CatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public CatalogsController(
            ApplicationDbContext context, 
            IAttachmentService attachmentService, 
            IFileService fileService,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            this.attachmentService = attachmentService;
            this.fileService = fileService;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Catalogs
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Catalog/GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] CatalogFilterViewModel filter)
        {
            try
            {
                var query = _context.Catalogs
                    .Include(c => c.Business)
                    .Include(c => c.Category)
                    .Include(c => c.StatusOptionSets)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.CatalogId))
                {
                    query = query.Where(c => c.ID.ToString().Contains(filter.CatalogId));
                }

                if (!string.IsNullOrWhiteSpace(filter.CatalogName))
                {
                    query = query.Where(c => c.Name.Contains(filter.CatalogName));
                }

                if (!string.IsNullOrWhiteSpace(filter.BusinessName))
                {
                    query = query.Where(c => c.Business.Name.Contains(filter.BusinessName));
                }

                // Add filter for Business ID
                if (!string.IsNullOrWhiteSpace(filter.BusinessId))
                {
                    if (int.TryParse(filter.BusinessId, out int businessId))
                    {
                        query = query.Where(c => c.BusinessID == businessId);
                    }
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(c => c.ID) : query.OrderByDescending(c => c.ID),
                    "name" => filter.SortDirection == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                    "businessname" => filter.SortDirection == "asc" ? query.OrderBy(c => c.Business.Name) : query.OrderByDescending(c => c.Business.Name),
                    "rating" => filter.SortDirection == "asc" ? query.OrderBy(c => c.Rating) : query.OrderByDescending(c => c.Rating),
                    "status" => filter.SortDirection == "asc" ? query.OrderBy(c => c.StatusOptionSets.Status) : query.OrderByDescending(c => c.StatusOptionSets.Status),
                    _ => query.OrderBy(c => c.ID)
                };

                // Apply pagination and project to DTO
                var catalogs = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(c => new CatalogDto
                    {
                        ID = c.ID,
                        Name = c.Name,
                        BusinessName = c.Business.Name,
                        BusinessID = c.BusinessID,
                        CategoryName = c.Category.Name,
                        Rating = c.Rating ?? 0,
                        StatusName = c.StatusOptionSets.Status,
                        IsRecommended = c.IsRecommended
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = catalogs });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Catalogs/Details/5
        [SecurableAuthorize(SecurableConstants.CatalogsView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Business)
                .Include(c => c.StatusOptionSets)
                .Include(c => c.CatalogVariants)
                    .ThenInclude(v => v.Inventory)
                        .ThenInclude(i => i.Outlet)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (catalog == null)
            {
                return NotFound();
            }

            var OutletProductsList = await _context.Inventory
                .Include(c => c.Outlet)
                .Where(c => c.CatalogVariants.Catalog.ID == id)
                .ToListAsync();

            var CatalogModel = new CatelogModel()
            {
                Catalog = catalog,
                Inventorys = OutletProductsList
            };

            // Get attachment type IDs
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            
            var attachmentTypeID_Product = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);

            // Default thumbnail and product images collections
            string defaultThumbnail = null;
            var productImages = new List<string>();
            
            // Get variant images for each variant
            var variantImages = new Dictionary<int, List<string>>();
            
            if (catalog?.CatalogVariants != null)
            {
                // Find default variant
                var defaultVariant = catalog.CatalogVariants.FirstOrDefault(v => v.IsDefaultVariant) ?? 
                                     catalog.CatalogVariants.FirstOrDefault();
                
                // Fetch images for each variant
                foreach (var variant in catalog.CatalogVariants)
                {
                    // Thumbnail images query
                    var variantThumbnailQuery = from attachment in _context.Attachments
                                             join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                             join entity in _context.Entity on link.EntityID equals entity.ID
                                             where entity.Name == EntityConstants.CatelogVariant && 
                                                   link.RecordID == variant.ID && 
                                                   link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                             select new
                                             {
                                                 ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                             };

                    // Product images query
                    var variantProductImagesQuery = from attachment in _context.Attachments
                                                 join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                                 join entity in _context.Entity on link.EntityID equals entity.ID
                                                 where entity.Name == EntityConstants.CatelogVariant && 
                                                       link.RecordID == variant.ID && 
                                                       link.AttachmentTypeID == attachmentTypeID_Product.ID
                                                 select new
                                                 {
                                                     ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                                 };

                    // Get results
                    var thumbnailResult = await variantThumbnailQuery.Select(a => a.ImagePath).FirstOrDefaultAsync();
                    var productImagesResult = await variantProductImagesQuery.Select(a => a.ImagePath).ToListAsync();
                    
                    // Store all variant images
                    var allVariantImages = new List<string>();
                    if (!string.IsNullOrEmpty(thumbnailResult))
                    {
                        allVariantImages.Add(thumbnailResult);
                    }
                    
                    if (productImagesResult.Any())
                    {
                        allVariantImages.AddRange(productImagesResult);
                    }
                    
                    if (allVariantImages.Any())
                    {
                        variantImages.Add(variant.ID, allVariantImages);
                    }
                    
                    // If this is the default variant, set the default thumbnail and add to product images
                    if (variant.ID == defaultVariant?.ID)
                    {
                        defaultThumbnail = thumbnailResult;
                        productImages.AddRange(productImagesResult);
                    }
                }
            }
            
            // Fallback to catalog-level thumbnails if no variant thumbnails found
            if (string.IsNullOrEmpty(defaultThumbnail))
            {
                var catalogThumbnailQuery = from attachment in _context.Attachments
                                          join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                          join entity in _context.Entity on link.EntityID equals entity.ID
                                          where entity.Name == EntityConstants.Catelog && 
                                                link.RecordID == catalog.ID && 
                                                link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                          select new
                                          {
                                              ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                          };
                                          
                defaultThumbnail = await catalogThumbnailQuery.Select(a => a.ImagePath).FirstOrDefaultAsync();
            }
            
            // Fallback to catalog-level product images if no variant images found
            if (!productImages.Any())
            {
                var catalogProductImagesQuery = from attachment in _context.Attachments
                                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                              join entity in _context.Entity on link.EntityID equals entity.ID
                                              where entity.Name == EntityConstants.Catelog && 
                                                    link.RecordID == catalog.ID && 
                                                    link.AttachmentTypeID == attachmentTypeID_Product.ID
                                              select new
                                              {
                                                  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                              };
                                              
                var catalogProductImages = await catalogProductImagesQuery.Select(a => a.ImagePath).ToListAsync();
                productImages.AddRange(catalogProductImages);
            }

            // Set ViewBag properties
            ViewBag.thumbnail = defaultThumbnail;
            ViewBag.productImages = productImages;
            ViewBag.variantImages = variantImages;

            return View(CatalogModel);
        }

        // GET: Catalogs/Create
        [SecurableAuthorize(SecurableConstants.CatalogsAdd)]
        public IActionResult Create()
        {
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID");
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View();
        }

        // POST: Catalogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Catalog catalog)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            catalog.CreateBy = userName;
            catalog.CreateDate = DateTime.UtcNow;
            if (ModelState.ContainsKey("CreateDate"))
            {
                ModelState.Remove("CreateDate");
            }

            // Default status if not provided
            if (catalog.StatusId == 0)
            {
                var activeStatus = await _context.StatusOptionSets
                    .FirstOrDefaultAsync(s => s.Status.ToLower() == "active");
                catalog.StatusId = activeStatus?.Id ?? 1;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Process highlights
                    if (Request.Form.ContainsKey("BaseHightlights"))
                    {
                        catalog.BaseHightlights = Request.Form["BaseHightlights"];
                    }

                    // Save catalog to get ID
                    _context.Add(catalog);
                    await _context.SaveChangesAsync();

                    // Process variant data - unified approach for all variants
                    bool hasVariantData = Request.Form.ContainsKey("VariantData") && 
                                         !string.IsNullOrEmpty(Request.Form["VariantData"]) && 
                                         Request.Form["VariantData"] != "[]";
                    
                    if (hasVariantData)
                    {
                        var variantDataJson = Request.Form["VariantData"].ToString();
                        var variantsData = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(variantDataJson);
                        
                        // Double check if we actually have variants
                        if (variantsData != null && variantsData.Count > 0)
                        {
                            // Validate and save variant options if in variant mode
                            bool hasMultipleVariants = variantsData.Count > 1 || 
                                                      (Request.Form.ContainsKey("hasVariants") && 
                                                       Request.Form["hasVariants"] == "true");
                            
                            if (hasMultipleVariants && Request.Form.ContainsKey("VariantOptions"))
                            {
                                string variantOptions = ValidateVariantOptions(
                                    Request.Form["VariantOptions"].ToString(), 
                                    true, 
                                    variantsData.Count
                                );
                                
                                catalog.VariantOptions = variantOptions;
                                _context.Update(catalog);
                                await _context.SaveChangesAsync();
                            }

                            // Process each variant
                            for (int i = 0; i < variantsData.Count; i++)
                            {
                                var variantData = variantsData[i];
                                var variant = new CatalogVariants
                                {
                                    CatalogID = catalog.ID,
                                    SKU = variantData.GetProperty("SKU").GetString(),
                                    Barcode = variantData.TryGetProperty("Barcode", out System.Text.Json.JsonElement barcode) ? barcode.GetString() : null,
                                    CostPrice = variantData.GetProperty("CostPrice").GetDouble(),
                                    SalePrice = variantData.GetProperty("SalePrice").GetDouble(),
                                    Attributes = variantData.TryGetProperty("Attributes", out System.Text.Json.JsonElement attributes) ? attributes.GetString() : "{}",
                                    IsDefaultVariant = variantData.TryGetProperty("IsDefault", out System.Text.Json.JsonElement isDefault) ? isDefault.GetBoolean() : (i == 0),
                                    CreateBy = userName,
                                    CreateDate = DateTime.UtcNow
                                };

                                // Process optional numeric fields
                                if (variantData.TryGetProperty("CompareAtPrice", out System.Text.Json.JsonElement comparePrice))
                                    variant.CompareAtPrice = comparePrice.ValueKind != System.Text.Json.JsonValueKind.Null ? comparePrice.GetDouble() : null;

                                if (variantData.TryGetProperty("MinimumOrderQuantity", out System.Text.Json.JsonElement minOrder))
                                    variant.MinimumOrderQuantity = minOrder.ValueKind != System.Text.Json.JsonValueKind.Null ? minOrder.GetInt32() : null;

                                if (variantData.TryGetProperty("MaximumOrderQuantity", out System.Text.Json.JsonElement maxOrder))
                                    variant.MaximumOrderQuantity = maxOrder.ValueKind != System.Text.Json.JsonValueKind.Null ? maxOrder.GetInt32() : null;

                                if (variantData.TryGetProperty("PackedHeight", out System.Text.Json.JsonElement height))
                                    variant.PackedHeight = height.ValueKind != System.Text.Json.JsonValueKind.Null ? height.GetDouble() : null;

                                if (variantData.TryGetProperty("PackedWidth", out System.Text.Json.JsonElement width))
                                    variant.PackedWidth = width.ValueKind != System.Text.Json.JsonValueKind.Null ? width.GetDouble() : null;

                                if (variantData.TryGetProperty("PackedDepth", out System.Text.Json.JsonElement depth))
                                    variant.PackedDepth = depth.ValueKind != System.Text.Json.JsonValueKind.Null ? depth.GetDouble() : null;

                                if (variantData.TryGetProperty("WeightUnit", out System.Text.Json.JsonElement weightUnit))
                                    variant.WeightUnit = weightUnit.GetString();

                                if (variantData.TryGetProperty("Weight", out System.Text.Json.JsonElement weight))
                                    variant.Weight = weight.ValueKind != System.Text.Json.JsonValueKind.Null ? weight.GetDouble() : null;

                                if (variantData.TryGetProperty("AdditionalHighlights", out System.Text.Json.JsonElement highlights))
                                    variant.AdditionalHightlights = highlights.GetString();

                                _context.CatalogVariants.Add(variant);
                                await _context.SaveChangesAsync();

                                // Handle attachments for this variant
                                // For default mode, use variants_0 naming
                                // For variant mode, use variants_index naming
                                bool isDefaultMode = !hasMultipleVariants && i == 0;
                                
                                var thumbnailFiles = isDefaultMode 
                                    ? Request.Form.Files.Where(f => f.Name == "variants_0_thumbnail").ToArray()
                                    : Request.Form.Files.Where(f => f.Name == $"variants_{i}_thumbnail").ToArray();
                                    
                                var productImageFiles = isDefaultMode
                                    ? Request.Form.Files.Where(f => f.Name == "variants_0_productImages").ToArray()
                                    : Request.Form.Files.Where(f => f.Name == $"variants_{i}_productImages").ToArray();

                                if (thumbnailFiles.Length > 0)
                                {
                                    var attachmentModel = new BusinessAttachmentModel()
                                    {
                                        Name = EntityConstants.CatelogVariant,
                                        Comment = $"Thumbnail for variant {variant.ID}",
                                        RecordID = variant.ID,
                                        Entity = EntityConstants.CatelogVariant,
                                        AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                        Files = thumbnailFiles,
                                        BusinessID = catalog.BusinessID
                                    };
                                    await attachmentService.UploadBusinessAttachment(attachmentModel);
                                }

                                if (productImageFiles.Length > 0)
                                {
                                    var attachmentModel = new BusinessAttachmentModel()
                                    {
                                        Name = EntityConstants.CatelogVariant,
                                        Comment = $"Product images for variant {variant.ID}",
                                        RecordID = variant.ID,
                                        Entity = EntityConstants.CatelogVariant,
                                        AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                                        Files = productImageFiles,
                                        BusinessID = catalog.BusinessID
                                    };
                                    await attachmentService.UploadBusinessAttachment(attachmentModel);
                                }
                            }
                            
                            // Log the event
                            await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                            {
                                EventName = EventConstants.AddCatalog,
                                EntityName = EntityConstants.Catalog,
                                Source = _urlContextService.CurrentPageUrl,
                                Description = $"Added catalog '{catalog.Name}' with {variantsData.Count} variants",
                                Data = System.Text.Json.JsonSerializer.Serialize(catalog),
                                RecordId = catalog.ID,
                                BusinessID = catalog.BusinessID,
                                LoggedInUserName = User.Identity.Name
                            });
                            
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    
                    // If we get here, something went wrong with variant processing
                    ModelState.AddModelError("", "No valid variant data was provided. Please try again.");
                    
                    // Log the error for debugging
                    System.Diagnostics.Debug.WriteLine("Failed to process variants. Form data:");
                    foreach (var key in Request.Form.Keys)
                    {
                        System.Diagnostics.Debug.WriteLine($"{key}: {Request.Form[key]}");
                    }
                    
                    ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", catalog.BusinessID);
                    ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
                    return View(catalog);
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    System.Diagnostics.Debug.WriteLine($"Error saving catalog: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    
                    ModelState.AddModelError("", $"Error saving catalog: {ex.Message}");
                }
            }

            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        // GET: Catalogs/Edit/5
        [SecurableAuthorize(SecurableConstants.CatalogsEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Update to include CatalogVariants
            var catalog = await _context.Catalogs
                .Include(c => c.CatalogVariants)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (catalog == null)
            {
                return NotFound();
            }

            // Get catalog-level attachments
            ViewBag.thumbnails = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                Entity = EntityConstants.Catelog,
                RecordID = catalog.ID
            });

            ViewBag.ProductImages = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                Entity = EntityConstants.Catelog,
                RecordID = catalog.ID
            });

            // Get attachment type IDs
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            
            var attachmentTypeID_Product = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);

            // Prepare variant attachment data
            var variantAttachments = new Dictionary<int, Dictionary<string, object>>();

            // Process each variant to get its attachments
            if (catalog.CatalogVariants != null && catalog.CatalogVariants.Any())
            {
                foreach (var variant in catalog.CatalogVariants)
                {
                    // Get variant thumbnails
                    var variantThumbnails = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                    {
                        AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                        Entity = EntityConstants.CatelogVariant,
                        RecordID = variant.ID
                    });

                    // Get variant product images
                    var variantProductImages = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                    {
                        AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                        Entity = EntityConstants.CatelogVariant,
                        RecordID = variant.ID
                    });

                    variantAttachments[variant.ID] = new Dictionary<string, object>
                    {
                        ["thumbnails"] = variantThumbnails,
                        ["productImages"] = variantProductImages
                    };
                }
            }

            ViewBag.VariantAttachments = variantAttachments;
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        // POST: Catalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Catalog catalog, string? removedVariantIds)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            catalog.ModifiedBy = userName;
            catalog.ModifiedDate = DateTime.UtcNow;

            if (id != catalog.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.Catalogs.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    // Process highlights
                    if (Request.Form.ContainsKey("BaseHightlights"))
                    {
                        catalog.BaseHightlights = Request.Form["BaseHightlights"];
                    }

                    _context.Update(catalog);
                    await _context.SaveChangesAsync();

                    // Delete removed variants if any
                    if (!string.IsNullOrEmpty(removedVariantIds))
                    {
                        var idsToRemove = removedVariantIds.Split(',').Select(int.Parse).ToList();
                        foreach (var variantId in idsToRemove)
                        {
                            var variantToRemove = await _context.CatalogVariants.FindAsync(variantId);
                            if (variantToRemove != null)
                            {
                                // Get and delete all attachments for this variant
                                var thumbnailAttachments = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                                {
                                    AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                    Entity = EntityConstants.CatelogVariant,
                                    RecordID = variantId
                                });
                                
                                var productImageAttachments = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                                {
                                    AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                                    Entity = EntityConstants.CatelogVariant,
                                    RecordID = variantId
                                });
                                
                                if (thumbnailAttachments?.Count > 0)
                                {
                                    await fileService.DeleteImage(thumbnailAttachments, userName);
                                }
                                
                                if (productImageAttachments?.Count > 0)
                                {
                                    await fileService.DeleteImage(productImageAttachments, userName);
                                }
                                
                                _context.CatalogVariants.Remove(variantToRemove);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    // Process variant data for update or new variants
                    if (Request.Form.ContainsKey("VariantData") && !string.IsNullOrEmpty(Request.Form["VariantData"]))
                    {
                        var variantDataJson = Request.Form["VariantData"].ToString();
                        var variantsData = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(variantDataJson);
                        
                        // Update variant options if provided
                        if (Request.Form.ContainsKey("VariantOptions"))
                        {
                            string variantOptions = ValidateVariantOptions(
                                Request.Form["VariantOptions"].ToString(), 
                                true, 
                                variantsData.Count
                            );
                            
                            catalog.VariantOptions = variantOptions;
                            _context.Update(catalog);
                            await _context.SaveChangesAsync();
                        }

                        // Process each variant
                        for (int i = 0; i < variantsData.Count; i++)
                        {
                            var variantData = variantsData[i];
                            int? variantId = null;
                            
                            // Check if variant has ID (existing) or needs to be created
                            if (variantData.TryGetProperty("ID", out System.Text.Json.JsonElement idProp) && idProp.ValueKind != System.Text.Json.JsonValueKind.Null)
                            {
                                variantId = idProp.GetInt32();
                            }

                            CatalogVariants variant;
                            bool isNewVariant = !variantId.HasValue;
                            
                            if (isNewVariant)
                            {
                                // Create new variant
                                variant = new CatalogVariants
                                {
                                    CatalogID = catalog.ID,
                                    CreateBy = userName,
                                    CreateDate = DateTime.UtcNow
                                };
                            }
                            else
                            {
                                // Update existing variant
                                variant = await _context.CatalogVariants.FindAsync(variantId.Value);
                                if (variant == null) continue;
                                
                                variant.ModifiedBy = userName;
                                variant.ModifiedDate = DateTime.UtcNow;
                            }

                            // Update variant properties
                            variant.SKU = variantData.GetProperty("SKU").GetString();
                            variant.Barcode = variantData.TryGetProperty("Barcode", out System.Text.Json.JsonElement barcode) ? barcode.GetString() : null;
                            variant.CostPrice = variantData.GetProperty("CostPrice").GetDouble();
                            variant.SalePrice = variantData.GetProperty("SalePrice").GetDouble();
                            variant.Attributes = variantData.GetProperty("Attributes").GetString();
                            variant.IsDefaultVariant = variantData.TryGetProperty("IsDefault", out System.Text.Json.JsonElement isDefault) ? isDefault.GetBoolean() : false;

                            // Process optional numeric fields
                            if (variantData.TryGetProperty("CompareAtPrice", out System.Text.Json.JsonElement comparePrice))
                                variant.CompareAtPrice = comparePrice.ValueKind != System.Text.Json.JsonValueKind.Null ? comparePrice.GetDouble() : null;

                            if (variantData.TryGetProperty("MinimumOrderQuantity", out System.Text.Json.JsonElement minOrder))
                                variant.MinimumOrderQuantity = minOrder.ValueKind != System.Text.Json.JsonValueKind.Null ? minOrder.GetInt32() : null;

                            if (variantData.TryGetProperty("MaximumOrderQuantity", out System.Text.Json.JsonElement maxOrder))
                                variant.MaximumOrderQuantity = maxOrder.ValueKind != System.Text.Json.JsonValueKind.Null ? maxOrder.GetInt32() : null;

                            if (variantData.TryGetProperty("PackedHeight", out System.Text.Json.JsonElement height))
                                variant.PackedHeight = height.ValueKind != System.Text.Json.JsonValueKind.Null ? height.GetDouble() : null;

                            if (variantData.TryGetProperty("PackedWidth", out System.Text.Json.JsonElement width))
                                variant.PackedWidth = width.ValueKind != System.Text.Json.JsonValueKind.Null ? width.GetDouble() : null;

                            if (variantData.TryGetProperty("PackedDepth", out System.Text.Json.JsonElement depth))
                                variant.PackedDepth = depth.ValueKind != System.Text.Json.JsonValueKind.Null ? depth.GetDouble() : null;

                            if (variantData.TryGetProperty("WeightUnit", out System.Text.Json.JsonElement weightUnit))
                                variant.WeightUnit = weightUnit.GetString();

                            if (variantData.TryGetProperty("Weight", out System.Text.Json.JsonElement weight))
                                variant.Weight = weight.ValueKind != System.Text.Json.JsonValueKind.Null ? weight.GetDouble() : null;

                            if (variantData.TryGetProperty("AdditionalHighlights", out System.Text.Json.JsonElement highlights))
                                variant.AdditionalHightlights = highlights.GetString();

                            if (isNewVariant)
                            {
                                _context.CatalogVariants.Add(variant);
                            }
                            else
                            {
                                _context.CatalogVariants.Update(variant);
                            }
                            
                            await _context.SaveChangesAsync();

                            // Handle new variant attachments directly
                            var thumbnailFiles = Request.Form.Files
                                .Where(f => f.Name == $"variants_{i}_thumbnail")
                                .ToArray();
                                
                            var productImageFiles = Request.Form.Files
                                .Where(f => f.Name == $"variants_{i}_productImages")
                                .ToArray();

                            if (thumbnailFiles.Length > 0)
                            {
                                var attachmentModel = new BusinessAttachmentModel()
                                {
                                    Name = EntityConstants.CatelogVariant,
                                    Comment = $"Thumbnail for variant {variant.ID}",
                                    RecordID = variant.ID,
                                    Entity = EntityConstants.CatelogVariant,
                                    AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                    Files = thumbnailFiles,
                                    BusinessID = catalog.BusinessID
                                };
                                await attachmentService.UploadBusinessAttachment(attachmentModel);
                            }

                            if (productImageFiles.Length > 0)
                            {
                                var attachmentModel = new BusinessAttachmentModel()
                                {
                                    Name = EntityConstants.CatelogVariant,
                                    Comment = $"Product images for variant {variant.ID}",
                                    RecordID = variant.ID,
                                    Entity = EntityConstants.CatelogVariant,
                                    AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                                    Files = productImageFiles,
                                    BusinessID = catalog.BusinessID
                                };
                                await attachmentService.UploadBusinessAttachment(attachmentModel);
                            }
                        }
                    }
                    else if (Request.Form.ContainsKey("defaultVariant_SKU"))
                    {
                        // Handle default variant update
                        var variants = await _context.CatalogVariants
                            .Where(v => v.CatalogID == catalog.ID)
                            .ToListAsync();
                        
                        if (variants.Count == 0)
                        {
                            // Create new default variant
                            var defaultVariant = new CatalogVariants
                            {
                                CatalogID = catalog.ID,
                                SKU = Request.Form["defaultVariant_SKU"],
                                Barcode = Request.Form["defaultVariant_Barcode"],
                                CostPrice = double.Parse(Request.Form["defaultVariant_CostPrice"]),
                                SalePrice = double.Parse(Request.Form["defaultVariant_SalePrice"]),
                                IsDefaultVariant = true,
                                CreateBy = userName,
                                CreateDate = DateTime.UtcNow
                            };

                            // Process optional fields
                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_CompareAtPrice"]))
                                defaultVariant.CompareAtPrice = double.Parse(Request.Form["defaultVariant_CompareAtPrice"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_MinimumOrderQuantity"]))
                                defaultVariant.MinimumOrderQuantity = int.Parse(Request.Form["defaultVariant_MinimumOrderQuantity"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_MaximumOrderQuantity"]))
                                defaultVariant.MaximumOrderQuantity = int.Parse(Request.Form["defaultVariant_MaximumOrderQuantity"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_PackedHeight"]))
                                defaultVariant.PackedHeight = double.Parse(Request.Form["defaultVariant_PackedHeight"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_PackedWidth"]))
                                defaultVariant.PackedWidth = double.Parse(Request.Form["defaultVariant_PackedWidth"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_PackedDepth"]))
                                defaultVariant.PackedDepth = double.Parse(Request.Form["defaultVariant_PackedDepth"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_WeightUnit"]))
                                defaultVariant.WeightUnit = Request.Form["defaultVariant_WeightUnit"];

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_Weight"]))
                                defaultVariant.Weight = double.Parse(Request.Form["defaultVariant_Weight"]);

                            _context.CatalogVariants.Add(defaultVariant);
                            await _context.SaveChangesAsync();

                            // Handle default variant attachments
                            var thumbnailFiles = Request.Form.Files
                                .Where(f => f.Name == "defaultVariant_thumbnail")
                                .ToArray();
                                
                            var productImageFiles = Request.Form.Files
                                .Where(f => f.Name == "defaultVariant_productImages")
                                .ToArray();

                            if (thumbnailFiles.Length > 0)
                            {
                                var attachmentModel = new BusinessAttachmentModel()
                                {
                                    Name = EntityConstants.CatelogVariant,
                                    Comment = $"Thumbnail for default variant",
                                    RecordID = defaultVariant.ID,
                                    Entity = EntityConstants.CatelogVariant,
                                    AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                    Files = thumbnailFiles,
                                    BusinessID = catalog.BusinessID
                                };
                                await attachmentService.UploadBusinessAttachment(attachmentModel);
                            }

                            if (productImageFiles.Length > 0)
                            {
                                var attachmentModel = new BusinessAttachmentModel()
                                {
                                    Name = EntityConstants.CatelogVariant,
                                    Comment = $"Product images for default variant",
                                    RecordID = defaultVariant.ID,
                                    Entity = EntityConstants.CatelogVariant,
                                    AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                                    Files = productImageFiles,
                                    BusinessID = catalog.BusinessID
                                };
                                await attachmentService.UploadBusinessAttachment(attachmentModel);
                            }
                        }
                        else
                        {
                            // Update existing default variant
                            var defaultVariant = variants.FirstOrDefault(v => v.IsDefaultVariant) ?? variants.First();
                            
                            defaultVariant.SKU = Request.Form["defaultVariant_SKU"];
                            defaultVariant.Barcode = Request.Form["defaultVariant_Barcode"];
                            defaultVariant.CostPrice = double.Parse(Request.Form["defaultVariant_CostPrice"]);
                            defaultVariant.SalePrice = double.Parse(Request.Form["defaultVariant_SalePrice"]);
                            defaultVariant.IsDefaultVariant = true;
                            defaultVariant.ModifiedBy = userName;
                            defaultVariant.ModifiedDate = DateTime.UtcNow;

                            // Process optional fields
                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_CompareAtPrice"]))
                                defaultVariant.CompareAtPrice = double.Parse(Request.Form["defaultVariant_CompareAtPrice"]);
                            else
                                defaultVariant.CompareAtPrice = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_MinimumOrderQuantity"]))
                                defaultVariant.MinimumOrderQuantity = int.Parse(Request.Form["defaultVariant_MinimumOrderQuantity"]);
                            else
                                defaultVariant.MinimumOrderQuantity = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_MaximumOrderQuantity"]))
                                defaultVariant.MaximumOrderQuantity = int.Parse(Request.Form["defaultVariant_MaximumOrderQuantity"]);
                            else
                                defaultVariant.MaximumOrderQuantity = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_PackedHeight"]))
                                defaultVariant.PackedHeight = double.Parse(Request.Form["defaultVariant_PackedHeight"]);
                            else
                                defaultVariant.PackedHeight = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_PackedWidth"]))
                                defaultVariant.PackedWidth = double.Parse(Request.Form["defaultVariant_PackedWidth"]);
                            else
                                defaultVariant.PackedWidth = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_PackedDepth"]))
                                defaultVariant.PackedDepth = double.Parse(Request.Form["defaultVariant_PackedDepth"]);
                            else
                                defaultVariant.PackedDepth = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_WeightUnit"]))
                                defaultVariant.WeightUnit = Request.Form["defaultVariant_WeightUnit"];
                            else
                                defaultVariant.WeightUnit = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant_Weight"]))
                                defaultVariant.Weight = double.Parse(Request.Form["defaultVariant_Weight"]);
                            else
                                defaultVariant.Weight = null;

                            _context.CatalogVariants.Update(defaultVariant);
                            await _context.SaveChangesAsync();

                            // Handle default variant thumbnail update
                            if (Request.Form.ContainsKey("defaultVariant_thumbnailUpdated") && 
                                Request.Form["defaultVariant_thumbnailUpdated"] == "true")
                            {
                                var thumbnailFiles = Request.Form.Files
                                    .Where(f => f.Name == "defaultVariant_thumbnail")
                                    .ToArray();
                                    
                                if (thumbnailFiles.Length > 0)
                                {
                                    // Delete existing thumbnails
                                    var existingThumbnails = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                                    {
                                        AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                        Entity = EntityConstants.CatelogVariant,
                                        RecordID = defaultVariant.ID
                                    });
                                    
                                    if (existingThumbnails?.Count > 0)
                                    {
                                        await fileService.DeleteImage(existingThumbnails, userName);
                                    }
                                    
                                    // Upload new thumbnail
                                    var attachmentModel = new BusinessAttachmentModel()
                                    {
                                        Name = EntityConstants.CatelogVariant,
                                        Comment = $"Thumbnail for default variant",
                                        RecordID = defaultVariant.ID,
                                        Entity = EntityConstants.CatelogVariant,
                                        AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                        Files = thumbnailFiles,
                                        BusinessID = catalog.BusinessID
                                    };
                                    await attachmentService.UploadBusinessAttachment(attachmentModel);
                                }
                            }

                            // Handle default variant product images update
                            if (Request.Form.ContainsKey("defaultVariant_imagesUpdated") && 
                                Request.Form["defaultVariant_imagesUpdated"] == "true")
                            {
                                var productImageFiles = Request.Form.Files
                                    .Where(f => f.Name == "defaultVariant_productImages")
                                    .ToArray();
                                    
                                if (productImageFiles.Length > 0)
                                {
                                    // Add product images without deleting existing ones
                                    var attachmentModel = new BusinessAttachmentModel()
                                    {
                                        Name = EntityConstants.CatelogVariant,
                                        Comment = $"Product images for default variant",
                                        RecordID = defaultVariant.ID,
                                        Entity = EntityConstants.CatelogVariant,
                                        AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                                        Files = productImageFiles,
                                        BusinessID = catalog.BusinessID
                                    };
                                    await attachmentService.UploadBusinessAttachment(attachmentModel);
                                }
                            }
                        }
                    }

                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditCatalog,
                        EntityName = EntityConstants.Catalog,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated catalog '{catalog.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = catalog
                        }),
                        RecordId = catalog.ID,
                        BusinessID = catalog.BusinessID,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogExists(catalog.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating catalog: {ex.Message}");
                }
            }

            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        // GET: Catalogs/Delete/5
        [SecurableAuthorize(SecurableConstants.CatalogsDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Business)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.CatalogsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalog = await _context.Catalogs
                .Include(c => c.CatalogVariants)
                .FirstOrDefaultAsync(c => c.ID == id);
        
            if (catalog != null)
            {
                var userName = this.User.FindFirst(ClaimTypes.Name).Value;
                
                // Delete all variants and their attachments
                if (catalog.CatalogVariants != null && catalog.CatalogVariants.Any())
                {
                    foreach (var variant in catalog.CatalogVariants)
                    {
                        // Delete variant thumbnails
                        var thumbnailAttachments = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                            Entity = EntityConstants.CatelogVariant,
                            RecordID = variant.ID
                        });
                        
                        if (thumbnailAttachments?.Count > 0)
                        {
                            await fileService.DeleteImage(thumbnailAttachments, userName);
                        }
                        
                        // Delete variant product images
                        var productImageAttachments = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                            Entity = EntityConstants.CatelogVariant,
                            RecordID = variant.ID
                        });
                        
                        if (productImageAttachments?.Count > 0)
                        {
                            await fileService.DeleteImage(productImageAttachments, userName);
                        }
                        
                        // Remove the variant
                        _context.CatalogVariants.Remove(variant);
                    }
                }
                
                // Delete catalog-level attachments
                var catalogThumbnails = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                {
                    AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                    Entity = EntityConstants.Catelog,
                    RecordID = catalog.ID
                });
                
                if (catalogThumbnails?.Count > 0)
                {
                    await fileService.DeleteImage(catalogThumbnails, userName);
                }
                
                var catalogProductImages = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                {
                    AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                    Entity = EntityConstants.Catelog,
                    RecordID = catalog.ID
                });
                
                if (catalogProductImages?.Count > 0)
                {
                    await fileService.DeleteImage(catalogProductImages, userName);
                }

                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteCatalog,
                    EntityName = EntityConstants.Catalog,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted catalog '{catalog.Name}' with {catalog.CatalogVariants?.Count ?? 0} variants",
                    Data = System.Text.Json.JsonSerializer.Serialize(catalog),
                    RecordId = catalog.ID,
                    BusinessID = catalog.BusinessID,
                    LoggedInUserName = User.Identity.Name
                });
                
                // Remove the catalog (this would automatically remove variants if cascade delete is set up)
                _context.Catalogs.Remove(catalog);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalogs.Any(e => e.ID == id);
        }
        
        /// <summary>
        /// Validates and sanitizes VariantOptions format
        /// </summary>
        /// <param name="variantOptions">The variant options JSON string</param>
        /// <param name="hasVariants">Whether the product has multiple variants</param>
        /// <param name="variantCount">The number of variants</param>
        /// <returns>A properly formatted VariantOptions string or null</returns>
        private string ValidateVariantOptions(string variantOptions, bool hasVariants, int variantCount)
        {
            // Clear variant options if:
            // 1. No variants flag is set
            // 2. There's only one variant and it's set as default
            // 3. The variantOptions string is empty or null
            
            if (!hasVariants || variantCount <= 1 || string.IsNullOrWhiteSpace(variantOptions))
            {
                return null;
            }
            
            try
            {
                // Validate that the JSON format is correct (should be a dictionary of string arrays)
                var options = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string[]>>(variantOptions);
                
                // Ensure there's at least one option with values
                if (options == null || !options.Any() || !options.Any(o => o.Value != null && o.Value.Length > 0))
                {
                    return null;
                }
                
                // Re-serialize to ensure format consistency
                return System.Text.Json.JsonSerializer.Serialize(options);
            }
            catch
            {
                // If parsing fails, return null
                return null;
            }
        }

        [HttpPost]
        [Route("Catalogs/MapToOutlet")]
        public async Task<IActionResult> MapToOutlet([FromBody] MapToOutletViewModel model)
        {
            try
            {
                if (model == null || model.OutletId <= 0 || model.CatalogIds == null || !model.CatalogIds.Any())
                {
                    return Json(new { success = false, message = "Invalid outlet or no catalogs selected" });
                }

                // Get the outlet to verify it exists
                var outlet = await _context.Outlets.FindAsync(model.OutletId);
                if (outlet == null)
                {
                    return Json(new { success = false, message = "Selected outlet not found" });
                }

                // Get all catalog variants for the selected catalogs
                var variants = await _context.CatalogVariants
                    .Where(v => model.CatalogIds.Contains(v.CatalogID))
                    .ToListAsync();

                if (!variants.Any())
                {
                    return Json(new { success = false, message = "No variants found for the selected catalogs" });
                }

                // Check if inventory entries already exist for these variants and outlet
                var existingInventories = await _context.Inventory
                    .Where(i => i.OutletID == model.OutletId && variants.Select(v => v.ID).Contains(i.ProductVariantID))
                    .ToListAsync();

                var existingVariantIds = existingInventories.Select(i => i.ProductVariantID).ToList();

                // Create new inventory entries for variants that don't already have one for this outlet
                var newInventories = new List<Inventory>();
                foreach (var variant in variants)
                {
                    if (!existingVariantIds.Contains(variant.ID))
                    {
                        newInventories.Add(new Inventory
                        {
                            OutletID = model.OutletId,
                            ProductVariantID = variant.ID,
                            TrackInventory = false,
                            IsInStock = true,
                            QuantityAvailable = null,
                            MinimumStockLevel = null,
                            MaximumStockLevel = null,
                            ReorderPoint = null,
                            BackOrders = false
                        });
                    }
                }

                if (newInventories.Any())
                {
                    await _context.Inventory.AddRangeAsync(newInventories);
                    await _context.SaveChangesAsync();
                }

                // Log the event
                // await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                // {
                //     EventName = EventConstants.MapCatalogsToOutlet,
                //     EntityName = EntityConstants.Inventory,
                //     Source = _urlContextService.CurrentPageUrl,
                //     Description = $"Mapped {model.CatalogIds.Count} catalogs to outlet ID {model.OutletId}",
                //     Data = System.Text.Json.JsonSerializer.Serialize(new { 
                //         OutletId = model.OutletId, 
                //         CatalogIds = model.CatalogIds,
                //         NewInventoryCount = newInventories.Count
                //     }),
                //     RecordId = model.OutletId,
                //     BusinessID = outlet.BussinessID,
                //     LoggedInUserName = User.Identity.Name
                // });

                return Json(new { 
                    success = true, 
                    message = $"Successfully mapped {newInventories.Count} new variants to the outlet",
                    totalVariants = variants.Count,
                    newVariants = newInventories.Count,
                    existingVariants = existingVariantIds.Count
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error mapping catalogs to outlet: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("Catalogs/GetOutletsByBusiness/{businessId}")]
        public async Task<IActionResult> GetOutletsByBusiness(int businessId)
        {
            try
            {
                // Note: In Outlet.cs, the property is named "BussinessID" (with double 's')
                var outlets = await _context.Outlets
                    .Where(o => o.BussinessID == businessId)
                    .Select(o => new { 
                        id = o.ID, 
                        name = o.Name, 
                        address = o.Address != null ? (o.Address.Address1 ?? o.Address.City ?? "No address") : "No address"
                    })
                    .ToListAsync();

                return Json(new { success = true, data = outlets });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error fetching outlets: {ex.Message}" });
            }
        }
    }

    public class MapToOutletViewModel
    {
        public int OutletId { get; set; }
        public List<int> CatalogIds { get; set; } = new List<int>();
    }
}
