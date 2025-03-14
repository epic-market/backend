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

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
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

                    // Process variant data - improved check for actual variants
                    bool hasValidVariants = Request.Form.ContainsKey("VariantData") && 
                                           !string.IsNullOrEmpty(Request.Form["VariantData"]) && 
                                           Request.Form["VariantData"] != "[]" &&
                                           Request.Form.ContainsKey("hasVariants") &&
                                           Request.Form["hasVariants"] == "true";
                    
                    if (hasValidVariants)
                    {
                        // Multiple variants case
                        var variantDataJson = Request.Form["VariantData"].ToString();
                        var variantsData = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(variantDataJson);
                        
                        // Double check if we actually have variants
                        if (variantsData != null && variantsData.Count > 0)
                        {
                            // Validate and save variant options
                            if (Request.Form.ContainsKey("VariantOptions"))
                            {
                                string variantOptions = ValidateVariantOptions(
                                    Request.Form["VariantOptions"].ToString(), 
                                    true, 
                                    variantsData.Count
                                );
                                
                                catalog.VarientOptions = variantOptions;
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
                                    Attributes = variantData.GetProperty("Attributes").GetString(),
                                    IsDefaultVariant = variantData.TryGetProperty("IsDefault", out System.Text.Json.JsonElement isDefault) ? isDefault.GetBoolean() : false,
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
                                await HandleVariantAttachments(variant.ID, i, catalog.BusinessID);
                            }
                            
                            // We successfully processed variants, so return early
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    
                    // If we get here, either there were no variants or variants processing failed
                    // Default variant case - create one variant from form data
                    var defaultVariant = new CatalogVariants
                    {
                        CatalogID = catalog.ID,
                        SKU = Request.Form["defaultVariant.SKU"],
                        Barcode = Request.Form["defaultVariant.Barcode"],
                        CostPrice = double.Parse(Request.Form["defaultVariant.CostPrice"]),
                        SalePrice = double.Parse(Request.Form["defaultVariant.SalePrice"]),
                        IsDefaultVariant = true,
                        CreateBy = userName,
                        CreateDate = DateTime.UtcNow
                    };

                    // Process optional fields
                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.CompareAtPrice"]))
                        defaultVariant.CompareAtPrice = double.Parse(Request.Form["defaultVariant.CompareAtPrice"]);

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.MinimumOrderQuantity"]))
                        defaultVariant.MinimumOrderQuantity = int.Parse(Request.Form["defaultVariant.MinimumOrderQuantity"]);

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.MaximumOrderQuantity"]))
                        defaultVariant.MaximumOrderQuantity = int.Parse(Request.Form["defaultVariant.MaximumOrderQuantity"]);

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedHeight"]))
                        defaultVariant.PackedHeight = double.Parse(Request.Form["defaultVariant.PackedHeight"]);

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedWidth"]))
                        defaultVariant.PackedWidth = double.Parse(Request.Form["defaultVariant.PackedWidth"]);

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedDepth"]))
                        defaultVariant.PackedDepth = double.Parse(Request.Form["defaultVariant.PackedDepth"]);

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.WeightUnit"]))
                        defaultVariant.WeightUnit = Request.Form["defaultVariant.WeightUnit"];

                    if (!string.IsNullOrEmpty(Request.Form["defaultVariant.Weight"]))
                        defaultVariant.Weight = double.Parse(Request.Form["defaultVariant.Weight"]);

                    _context.CatalogVariants.Add(defaultVariant);
                    await _context.SaveChangesAsync();

                    // Handle default variant attachments
                    var thumbnailFiles = Request.Form.Files
                        .Where(f => f.Name == "defaultVariant.thumbnail")
                        .ToArray();
                    
                    var productImageFiles = Request.Form.Files
                        .Where(f => f.Name == "defaultVariant.productImages")
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

                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.AddCatalog,
                        EntityName = EntityConstants.Catalog,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Added catalog '{catalog.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(catalog),
                        RecordId = catalog.ID,
                        BusinessID = catalog.BusinessID,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving catalog: {ex.Message}");
                }
            }

            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        private async Task HandleVariantAttachments(int variantId, int variantIndex, int businessId)
        {
            var thumbnailFiles = Request.Form.Files
                .Where(f => f.Name == $"variants[{variantIndex}].thumbnail")
                .ToArray();
                
            var productImageFiles = Request.Form.Files
                .Where(f => f.Name == $"variants[{variantIndex}].productImages")
                .ToArray();

            if (thumbnailFiles.Length > 0)
            {
                var attachmentModel = new BusinessAttachmentModel()
                {
                    Name = EntityConstants.CatelogVariant,
                    Comment = $"Thumbnail for variant {variantId}",
                    RecordID = variantId,
                    Entity = EntityConstants.CatelogVariant,
                    AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                    Files = thumbnailFiles,
                    BusinessID = businessId
                };
                await attachmentService.UploadBusinessAttachment(attachmentModel);
            }

            if (productImageFiles.Length > 0)
            {
                var attachmentModel = new BusinessAttachmentModel()
                {
                    Name = EntityConstants.CatelogVariant,
                    Comment = $"Product images for variant {variantId}",
                    RecordID = variantId,
                    Entity = EntityConstants.CatelogVariant,
                    AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                    Files = productImageFiles,
                    BusinessID = businessId
                };
                await attachmentService.UploadBusinessAttachment(attachmentModel);
            }
        }

        // GET: Catalogs/Edit/5
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
                            
                            catalog.VarientOptions = variantOptions;
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

                            // For new variants, we need to process attachments
                            // For existing variants, check if there are new files
                            if (isNewVariant)
                            {
                                await HandleVariantAttachments(variant.ID, i, catalog.BusinessID);
                            }
                            else
                            {
                                // Handle attachment updates - similar to how BusinessesController does it
                                string variantThumbnailUpdateFlag = $"variants[{i}].thumbnailUpdated";
                                string variantImagesUpdateFlag = $"variants[{i}].imagesUpdated";
                                
                                if (Request.Form.ContainsKey(variantThumbnailUpdateFlag) && Request.Form[variantThumbnailUpdateFlag] == "true")
                                {
                                    var thumbnailFiles = Request.Form.Files
                                        .Where(f => f.Name == $"variants[{i}].thumbnail")
                                        .ToArray();
                                        
                                    if (thumbnailFiles.Length > 0)
                                    {
                                        // Delete existing thumbnails
                                        var existingThumbnails = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                                        {
                                            AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                                            Entity = EntityConstants.CatelogVariant,
                                            RecordID = variant.ID
                                        });
                                        
                                        if (existingThumbnails?.Count > 0)
                                        {
                                            await fileService.DeleteImage(existingThumbnails, userName);
                                        }
                                        
                                        // Upload new thumbnail
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
                                }
                                
                                if (Request.Form.ContainsKey(variantImagesUpdateFlag) && Request.Form[variantImagesUpdateFlag] == "true")
                                {
                                    var productImageFiles = Request.Form.Files
                                        .Where(f => f.Name == $"variants[{i}].productImages")
                                        .ToArray();
                                        
                                    if (productImageFiles.Length > 0)
                                    {
                                        // Instead of deleting existing images, we might want to keep them
                                        // and just add new ones. If deletion is required, uncomment:
                                        /*
                                        var existingImages = await attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                                        {
                                            AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                                            Entity = EntityConstants.CatelogVariant,
                                            RecordID = variant.ID
                                        });
                                        
                                        if (existingImages?.Count > 0)
                                        {
                                            await fileService.DeleteImage(existingImages, userName);
                                        }
                                        */
                                        
                                        // Upload new product images
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
                        }
                    }
                    else if (Request.Form.ContainsKey("defaultVariant.SKU"))
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
                                SKU = Request.Form["defaultVariant.SKU"],
                                Barcode = Request.Form["defaultVariant.Barcode"],
                                CostPrice = double.Parse(Request.Form["defaultVariant.CostPrice"]),
                                SalePrice = double.Parse(Request.Form["defaultVariant.SalePrice"]),
                                IsDefaultVariant = true,
                                CreateBy = userName,
                                CreateDate = DateTime.UtcNow
                            };

                            // Process optional fields
                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.CompareAtPrice"]))
                                defaultVariant.CompareAtPrice = double.Parse(Request.Form["defaultVariant.CompareAtPrice"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.MinimumOrderQuantity"]))
                                defaultVariant.MinimumOrderQuantity = int.Parse(Request.Form["defaultVariant.MinimumOrderQuantity"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.MaximumOrderQuantity"]))
                                defaultVariant.MaximumOrderQuantity = int.Parse(Request.Form["defaultVariant.MaximumOrderQuantity"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedHeight"]))
                                defaultVariant.PackedHeight = double.Parse(Request.Form["defaultVariant.PackedHeight"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedWidth"]))
                                defaultVariant.PackedWidth = double.Parse(Request.Form["defaultVariant.PackedWidth"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedDepth"]))
                                defaultVariant.PackedDepth = double.Parse(Request.Form["defaultVariant.PackedDepth"]);

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.WeightUnit"]))
                                defaultVariant.WeightUnit = Request.Form["defaultVariant.WeightUnit"];

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.Weight"]))
                                defaultVariant.Weight = double.Parse(Request.Form["defaultVariant.Weight"]);

                            _context.CatalogVariants.Add(defaultVariant);
                            await _context.SaveChangesAsync();

                            // Handle default variant attachments
                            var thumbnailFiles = Request.Form.Files
                                .Where(f => f.Name == "defaultVariant.thumbnail")
                                .ToArray();
                                
                            var productImageFiles = Request.Form.Files
                                .Where(f => f.Name == "defaultVariant.productImages")
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
                            
                            defaultVariant.SKU = Request.Form["defaultVariant.SKU"];
                            defaultVariant.Barcode = Request.Form["defaultVariant.Barcode"];
                            defaultVariant.CostPrice = double.Parse(Request.Form["defaultVariant.CostPrice"]);
                            defaultVariant.SalePrice = double.Parse(Request.Form["defaultVariant.SalePrice"]);
                            defaultVariant.IsDefaultVariant = true;
                            defaultVariant.ModifiedBy = userName;
                            defaultVariant.ModifiedDate = DateTime.UtcNow;

                            // Process optional fields
                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.CompareAtPrice"]))
                                defaultVariant.CompareAtPrice = double.Parse(Request.Form["defaultVariant.CompareAtPrice"]);
                            else
                                defaultVariant.CompareAtPrice = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.MinimumOrderQuantity"]))
                                defaultVariant.MinimumOrderQuantity = int.Parse(Request.Form["defaultVariant.MinimumOrderQuantity"]);
                            else
                                defaultVariant.MinimumOrderQuantity = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.MaximumOrderQuantity"]))
                                defaultVariant.MaximumOrderQuantity = int.Parse(Request.Form["defaultVariant.MaximumOrderQuantity"]);
                            else
                                defaultVariant.MaximumOrderQuantity = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedHeight"]))
                                defaultVariant.PackedHeight = double.Parse(Request.Form["defaultVariant.PackedHeight"]);
                            else
                                defaultVariant.PackedHeight = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedWidth"]))
                                defaultVariant.PackedWidth = double.Parse(Request.Form["defaultVariant.PackedWidth"]);
                            else
                                defaultVariant.PackedWidth = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.PackedDepth"]))
                                defaultVariant.PackedDepth = double.Parse(Request.Form["defaultVariant.PackedDepth"]);
                            else
                                defaultVariant.PackedDepth = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.WeightUnit"]))
                                defaultVariant.WeightUnit = Request.Form["defaultVariant.WeightUnit"];
                            else
                                defaultVariant.WeightUnit = null;

                            if (!string.IsNullOrEmpty(Request.Form["defaultVariant.Weight"]))
                                defaultVariant.Weight = double.Parse(Request.Form["defaultVariant.Weight"]);
                            else
                                defaultVariant.Weight = null;

                            _context.CatalogVariants.Update(defaultVariant);
                            await _context.SaveChangesAsync();

                            // Handle default variant thumbnail update
                            if (Request.Form.ContainsKey("defaultVariant.thumbnailUpdated") && 
                                Request.Form["defaultVariant.thumbnailUpdated"] == "true")
                            {
                                var thumbnailFiles = Request.Form.Files
                                    .Where(f => f.Name == "defaultVariant.thumbnail")
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
                            if (Request.Form.ContainsKey("defaultVariant.imagesUpdated") && 
                                Request.Form["defaultVariant.imagesUpdated"] == "true")
                            {
                                var productImageFiles = Request.Form.Files
                                    .Where(f => f.Name == "defaultVariant.productImages")
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
    }
}
