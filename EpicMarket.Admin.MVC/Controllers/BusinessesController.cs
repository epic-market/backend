using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis.Elfie.Model;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Entities.CustomModels;
using System.Net.Mail;
using EpicMarket.Admin.MVC.Contracts;
using Microsoft.IdentityModel.Tokens;
using EpicMarket.Entities;
using System.IO;
using System.Text.Json;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class BusinessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public BusinessesController(
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Business/GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] BusinessFilterViewModel filter)
        {
            try
            {
                var query = _context.Businesses
                    .Include(b => b.Address)
                    .Include(b => b.BusinessCategory)
                    .Include(b => b.Person)
                    .Include(b => b.Status)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.BusinessId))
                {
                    query = query.Where(b => b.ID.ToString().Contains(filter.BusinessId));
                }

                if (!string.IsNullOrWhiteSpace(filter.OwnerUsername))
                {
                    query = query.Where(b => b.Person.UserName.Contains(filter.OwnerUsername));
                }

                if (!string.IsNullOrWhiteSpace(filter.ContactNumber))
                {
                    query = query.Where(b => b.ContactNumber.ToString().Contains(filter.ContactNumber));
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(b => b.ID) : query.OrderByDescending(b => b.ID),
                    "name" => filter.SortDirection == "asc" ? query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name),
                    "contactnumber" => filter.SortDirection == "asc" ? query.OrderBy(b => b.ContactNumber) : query.OrderByDescending(b => b.ContactNumber),
                    _ => query.OrderBy(b => b.ID)
                };

                // Apply pagination and project to DTO
                var businesses = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(b => new BusinessDto
                    {
                        ID = b.ID,
                        Name = b.Name,
                        ContactNumber = b.ContactNumber,
                        ContactEmail = b.ContactEmail,
                        StatusName = b.Status.Status,
                        PersonUserName = b.Person.UserName,
                        PersonId = b.Person.Id,
                        BusinessCategoryName = b.BusinessCategory.Name,
                        City = b.Address.City
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = businesses });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }


        // GET: Businesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var businessDetailModel = new BusinessDetailModel();

			var business = await _context.Businesses
                .Include(b => b.Address)
                .Include(b => b.BusinessCategory)
                .Include(b => b.Person)
                .Include(b=> b.Status)
                .FirstOrDefaultAsync(m => m.ID == id);



			var braches = await _context.Outlets
                         .Include(b => b.Address)        
				        .Where(m => m.BussinessID == id)
                        .ToListAsync();

			var employees = await _context.BusinessEmployeeMaps
		                    .Include(b => b.Employee)
		                    .Where(m => m.BussinessID == id)
		                    .ToListAsync();



			var products = await _context.Catalogs
							.Where(m => m.BusinessID == id)
							.ToListAsync();


			var attachmentTypeID_LOGO = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.LOGO);
            var attachmentTypeID_PROOF = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PROOF);



             var attachments_logo = from attachment in _context.Attachments
                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                              join entity in _context.Entity on link.EntityID equals entity.ID
                              where entity.Name == EntityConstants.Business && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_LOGO.ID
                              select new
                              {
                                  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                              };

            var attachments_proof = from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            where entity.Name == EntityConstants.Business && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_PROOF.ID
                            orderby attachment.CreateDate descending
                            select new AttachmentProof
                            {
                                ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}",
                                Name = $"{attachment.DocumentFile}"
							};


            ViewBag.AttachmentLogo = attachments_logo.Select(a => a.ImagePath).FirstOrDefault();
            ViewBag.AttachmentProof = attachments_proof.ToList();
            businessDetailModel.Business = business;
			businessDetailModel.Outlets = braches;
			businessDetailModel.employees = employees;
			businessDetailModel.Catalogs = products;

			if (business == null)
            {
                return NotFound();
            }

            return View(businessDetailModel);
        }

        // GET: Businesses/Create
        public IActionResult Create()
        {
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "Name");
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["StatusID"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View();
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PersonID,BusinessCategoryID,Name,Description,Banner,Logo,ContactNumber,ContactEmail,AddressID,Rating,ReviewCount,IsOpen,Weight,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Business business, IFormFile[] Images, IFormFile[] Proofs)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            business.Address.CreateBy = userName;
            business.Address.CreateDate = DateTime.UtcNow;
            business.CreateBy = userName;
            business.CreateDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(business);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddBusiness,
                    EntityName = EntityConstants.Business,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added business '{business.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(business),
                    RecordId = business.ID,
                    BusinessID = business.ID,
                    LoggedInUserName = User.Identity.Name
                });

                if (Images?.Length > 0) {
                    var attachmentLogo = new BusinessAttachmentModel()
                    {
                        Name = EntityConstants.Business,
                        Comment = EntityConstants.Business,
                        RecordID = business.ID,
                        Entity = EntityConstants.Business,
                        AttachmentType = AttachmentTypeConstants.LOGO,
                        Files = Images,
                        BusinessID = business.ID

                    };
                    await attachmentService.UploadBusinessAttachment(attachmentLogo);
                }

                if (Proofs?.Length > 0)
                {
                    var attachmentProofs = new BusinessAttachmentModel()
                    {
                        Name = EntityConstants.Business,
                        Comment = EntityConstants.Business,
                        RecordID = business.ID,
                        Entity = EntityConstants.Business,
                        AttachmentType = AttachmentTypeConstants.PROOF,
                        Files = Proofs,
                        BusinessID = business.ID
                    };
                    await attachmentService.UploadBusinessAttachment(attachmentProofs);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "Name");
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["StatusID"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View(business);
        }

        // GET: Businesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses.Where(b=>b.ID == id).Include(c=>c.Address).FirstOrDefaultAsync();
            ViewBag.attachments = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.LOGO,
                Entity = EntityConstants.Business,
                RecordID = business.ID
            });

            ViewBag.Proofs = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.PROOF,
                Entity = EntityConstants.Business,
                RecordID = business.ID
            });


            if (business == null)
            {
                return NotFound();
            }
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "Name", business.BusinessCategoryID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName", business.PersonID);
			ViewData["StatusID"] = new SelectList(_context.StatusOptionSets, "Id", "Status", business.StatusId);
			return View(business);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,BusinessCategoryID,Name,Description,Banner,Logo,ContactNumber,ContactEmail,AddressID,Rating,ReviewCount,IsOpen,Weight,CreateDate,CreateBy,ModifiedDate,ModifiedBy,StatusId,Address,IsActive")] Business business, IFormFile[] newLogo, string? addedLogos, IFormFile[] newProofs, string? removedProofs, string? addedProofs)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            business.Address.ModifiedBy = userName;
            business.Address.ModifiedDate = DateTime.UtcNow;
            business.Address.Id = (int)business.AddressID;
            business.ModifiedBy = userName;
            business.ModifiedDate = DateTime.UtcNow;

            if (id != business.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalBusiness = await _context.Businesses
                        .AsNoTracking()
                        .Include(b => b.Address)
                        .FirstOrDefaultAsync(b => b.ID == id);

                    _context.Update(business);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditBusiness,
                        EntityName = EntityConstants.Business,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated business '{business.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalBusiness,
                            Updated = business
                        }),
                        RecordId = business.ID,
                        BusinessID = business.ID,
                        LoggedInUserName = User.Identity.Name
                    });

                    // Handle added images
                    if (addedLogos != null && newLogo?.Length > 0)
                    {
                        List<string> ExisitingLogo = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.LOGO,
                            Entity = EntityConstants.Business,
                            RecordID = business.ID
                        });
                        if (ExisitingLogo?.Count > 0)
                        {
                            await this.fileService.DeleteImage(ExisitingLogo, userName);
                        }

                        var attachment = new BusinessAttachmentModel()
                        {
                            Name = EntityConstants.Business,
                            Comment = EntityConstants.Business,
                            RecordID = business.ID,
                            Entity = EntityConstants.Business,
                            AttachmentType = AttachmentTypeConstants.LOGO,
                            Files = newLogo,
                            BusinessID  = business.ID
                        };
                        await attachmentService.UploadBusinessAttachment(attachment);
                    }

                    // Handle removed images
                    if (!string.IsNullOrEmpty(removedProofs))
                    {
                        var removedImageUrls = removedProofs.Split(',').ToList();
                        await this.fileService.DeleteImage(removedImageUrls, userName);
                    }

                    // Handle added images
                    if (addedProofs != null && newProofs?.Length > 0)
                    {
                        var attachmentProofs = new BusinessAttachmentModel()
                        {
                            Name = EntityConstants.Business,
                            Comment = EntityConstants.Business,
                            RecordID = business.ID,
                            Entity = EntityConstants.Business,
                            AttachmentType = AttachmentTypeConstants.PROOF,
                            Files = newProofs,
                            BusinessID = business.ID

                        };
                        await attachmentService.UploadBusinessAttachment(attachmentProofs);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessExists(business.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", business.AddressID);
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "ID", business.BusinessCategoryID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "Id", business.PersonID);
            return View(business);
        }

        // GET: Businesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses
                .Include(b => b.Address)
                .Include(b => b.BusinessCategory)
                .Include(b => b.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var business = await _context.Businesses.FindAsync(id);
            if (business != null)
            {
                _context.Businesses.Remove(business);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteBusiness,
                    EntityName = EntityConstants.Business,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted business '{business.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(business),
                    RecordId = business.ID,
                    BusinessID = business.ID,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(e => e.ID == id);
        }

        // GET: Businesses/UploadProducts/5
        public IActionResult UploadProducts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = _context.Businesses.FirstOrDefault(b => b.ID == id);
            if (business == null)
            {
                return NotFound();
            }

            ViewBag.BusinessId = id;
            ViewBag.BusinessName = business.Name;
            return View();
        }

        // POST: Businesses/UploadProducts/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProducts(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload.");
                ViewBag.BusinessId = id;
                var business = _context.Businesses.FirstOrDefault(b => b.ID == id);
                ViewBag.BusinessName = business?.Name;
                return View();
            }
            
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".csv")
            {
                ModelState.AddModelError("File", "Please upload a CSV file.");
                ViewBag.BusinessId = id;
                var business = _context.Businesses.FirstOrDefault(b => b.ID == id);
                ViewBag.BusinessName = business?.Name;
                return View();
            }
            
            try
            {
                var productImports = new List<ProductImportDto>();
                var catalogs = new List<Catalog>();
                var catalogVariants = new List<CatalogVariants>();
                
                // Get default status ID for active products
                var activeStatusId = await _context.StatusOptionSets
                    .Where(s => s.Status.ToLower() == "active")
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();
                
                if (activeStatusId == 0)
                {
                    // Fallback to first status if "Active" not found
                    activeStatusId = await _context.StatusOptionSets
                        .Select(s => s.Id)
                        .FirstOrDefaultAsync();
                }
                
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    // Skip header row
                    await reader.ReadLineAsync();

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(',');

                        if (values.Length >= 7) // Ensure we have the minimum required fields
                        {
                            var productImport = new ProductImportDto
                            {
                                CatalogName = values[0].Trim().Replace("\"", ""),
                                CatalogDescription = values[1].Trim().Replace("\"", ""),
                                CategoryName = values[2].Trim().Replace("\"", ""),
                                IsRecommended = bool.TryParse(values[3].Trim(), out bool isRecommended) ? isRecommended : false,
                                RequiresRefrigeration = bool.TryParse(values[4].Trim(), out bool requiresRefrigeration) ? requiresRefrigeration : false,
                                SKU = values[5].Trim().Replace("\"", ""),
                                Barcode = values[6].Trim().Replace("\"", "")
                            };

                            // Parse additional fields if they exist
                            if (values.Length > 7)
                            {
                                if (values.Length > 7) productImport.AttributeName1 = values[7].Trim().Replace("\"", "");
                                if (values.Length > 8) productImport.AttributeValue1 = values[8].Trim().Replace("\"", "");
                                if (values.Length > 9) productImport.AttributeName2 = values[9].Trim().Replace("\"", "");
                                if (values.Length > 10) productImport.AttributeValue2 = values[10].Trim().Replace("\"", "");
                                if (values.Length > 11) productImport.AttributeName3 = values[11].Trim().Replace("\"", "");
                                if (values.Length > 12) productImport.AttributeValue3 = values[12].Trim().Replace("\"", "");
                                
                                if (values.Length > 13 && double.TryParse(values[13].Trim(), out double costPrice))
                                    productImport.CostPrice = costPrice;
                                    
                                if (values.Length > 14 && double.TryParse(values[14].Trim(), out double salePrice))
                                    productImport.SalePrice = salePrice;
                                    
                                if (values.Length > 15 && double.TryParse(values[15].Trim(), out double compareAtPrice))
                                    productImport.CompareAtPrice = compareAtPrice;
                                    
                                if (values.Length > 16 && int.TryParse(values[16].Trim(), out int maxOrderQty))
                                    productImport.MaximumOrderQuantity = maxOrderQty;
                                    
                                if (values.Length > 17 && int.TryParse(values[17].Trim(), out int minOrderQty))
                                    productImport.MinimumOrderQuantity = minOrderQty;
                                    
                                if (values.Length > 18 && double.TryParse(values[18].Trim(), out double packedHeight))
                                    productImport.PackedHeight = packedHeight;
                                    
                                if (values.Length > 19 && double.TryParse(values[19].Trim(), out double packedWidth))
                                    productImport.PackedWidth = packedWidth;
                                    
                                if (values.Length > 20 && double.TryParse(values[20].Trim(), out double packedDepth))
                                    productImport.PackedDepth = packedDepth;
                                    
                                if (values.Length > 21 && double.TryParse(values[21].Trim(), out double weight))
                                    productImport.Weight = weight;
                            }

                            productImports.Add(productImport);
                        }
                    }
                }
                
                // Group products by catalog name to create unique catalogs
                var groupedProducts = productImports.GroupBy(p => p.CatalogName);
                
                foreach (var group in groupedProducts)
                {
                    var firstProduct = group.First();
                    
                    // Look up category ID if it exists, otherwise create it
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == firstProduct.CategoryName.ToLower());
                        
                    int categoryId;
                    if (category == null)
                    {
                        // Create new category
                        var newCategory = new Category 
                        { 
                            Name = firstProduct.CategoryName,
                            Description = $"Auto-generated category for {firstProduct.CategoryName}",
                            IsActive = true,
                            BusinessID = id,
                            CreateDate = DateTime.UtcNow,
                            CreateBy = User.Identity.Name
                        };
                        
                        _context.Categories.Add(newCategory);
                        await _context.SaveChangesAsync();
                        categoryId = newCategory.ID;
                    }
                    else
                    {
                        categoryId = category.ID;
                    }
                    
                    // Create catalog
                    var catalog = firstProduct.ToCatalog(id, activeStatusId);
                    catalog.CategoryID = categoryId;
                    catalog.BusinessID = id;
                    catalog.CreateBy = User.Identity.Name;
                    catalog.VariantOptions = GetVarientOptionsJson(group);
                    
                    _context.Catalogs.Add(catalog);
                    await _context.SaveChangesAsync();
                    
                    // Create variants for this catalog
                    foreach (var product in group)
                    {
                        var variant = product.ToCatalogVariant(catalog.ID, User.Identity.Name);
                        _context.CatalogVariants.Add(variant);
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    catalogs.Add(catalog);
                }
                
                if (catalogs.Count > 0)
                {
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.AddCatalog,
                        EntityName = EntityConstants.Catalog,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Uploaded {catalogs.Count} catalogs with {productImports.Count} variants for business ID {id}",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            FileName = file.FileName, 
                            CatalogCount = catalogs.Count,
                            VariantCount = productImports.Count,
                            BusinessId = id
                        }),
                        RecordId = id,
                        BusinessID = id,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    TempData["SuccessMessage"] = $"Successfully imported {catalogs.Count} catalogs with {productImports.Count} variants.";
                    return RedirectToAction(nameof(Details), new { id = id });
                }
                else
                {
                    ModelState.AddModelError("File", "No valid data found in the uploaded file.");
                    ViewBag.BusinessId = id;
                    var business = _context.Businesses.FirstOrDefault(b => b.ID == id);
                    ViewBag.BusinessName = business?.Name;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", $"Error processing file: {ex.Message}");
                ViewBag.BusinessId = id;
                var business = _context.Businesses.FirstOrDefault(b => b.ID == id);
                ViewBag.BusinessName = business?.Name;
                return View();
            }
        }

        private string GetVarientOptionsJson(IGrouping<string, ProductImportDto> group)
        {
            var options = new Dictionary<string, List<string>>();
            foreach (var product in group)
            {
                if (!string.IsNullOrEmpty(product.AttributeName1) && !string.IsNullOrEmpty(product.AttributeValue1))
                {
                    if (!options.ContainsKey(product.AttributeName1))
                    {
                        options[product.AttributeName1] = new List<string>();
                    }
                    if (!options[product.AttributeName1].Contains(product.AttributeValue1))
                    {
                        options[product.AttributeName1].Add(product.AttributeValue1);
                    }
                }

                if (!string.IsNullOrEmpty(product.AttributeName2) && !string.IsNullOrEmpty(product.AttributeValue2))
                {
                    if (!options.ContainsKey(product.AttributeName2))
                    {
                        options[product.AttributeName2] = new List<string>();
                    }
                    if (!options[product.AttributeName2].Contains(product.AttributeValue2))
                    {
                        options[product.AttributeName2].Add(product.AttributeValue2);
                    }
                }

                if (!string.IsNullOrEmpty(product.AttributeName3) && !string.IsNullOrEmpty(product.AttributeValue3))
                {
                    if (!options.ContainsKey(product.AttributeName3))
                    {
                        options[product.AttributeName3] = new List<string>();
                    }
                    if (!options[product.AttributeName3].Contains(product.AttributeValue3))
                    {
                        options[product.AttributeName3].Add(product.AttributeValue3);
                    }
                }
            }

            return JsonSerializer.Serialize(options);
        }

        // GET: Businesses/DownloadProductSample
        public IActionResult DownloadProductSample()
        {
            // Create a sample CSV content
            var csvContent = "CatalogName,CatalogDescription,CategoryName,IsRecommended,RequiresRefrigeration,SKU,Barcode,AttributeName1,AttributeValue1,AttributeName2,AttributeValue2,AttributeName3,AttributeValue3,CostPrice,SalePrice,CompareAtPrice,MaximumOrderQuantity,MinimumOrderQuantity,PackedHeight,PackedWidhth,PackedDepth,Weight\n" +
                             "Men's T-Shirt,\"Premium cotton t-shirt for everyday wear\",Men's Clothing,TRUE,FALSE,TS-BLK-S,8901234567890,Size,S,Color,Black,,,8.50,19.99,24.99,10,1,2,20,15,150\n" +
                             "Men's T-Shirt,\"Premium cotton t-shirt for everyday wear\",Men's Clothing,TRUE,FALSE,TS-BLK-M,8901234567891,Size,M,Color,Black,,,8.50,19.99,24.99,10,1,2,22,16,170\n" +
                             "Men's T-Shirt,\"Premium cotton t-shirt for everyday wear\",Men's Clothing,TRUE,FALSE,TS-BLK-L,8901234567892,Size,L,Color,Black,,,9.00,19.99,24.99,10,1,2,24,17,190\n" +
                             "Men's T-Shirt,\"Premium cotton t-shirt for everyday wear\",Men's Clothing,TRUE,FALSE,TS-WHT-M,8901234567893,Size,M,Color,White,,,8.50,19.99,24.99,10,1,2,22,16,170\n" +
                             "Women's Jeans,\"High-quality denim jeans with stretch\",Women's Clothing,TRUE,FALSE,WJ-BLU-28,8901234567894,Size,28,Color,Blue,,,22.50,49.99,59.99,5,1,3,25,20,450";
            
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            
            return File(bytes, "text/csv", "product_catalog_sample.csv");
        }

        [HttpGet]
        [Route("Business/GetStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                // Get current date and first day of current month
                var currentDate = DateTime.UtcNow;
                var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                
                // Get total business count
                var totalBusinesses = await _context.Businesses.CountAsync();
                
                // Get verified businesses count (assuming there's a status named "Verified")
                var verifiedStatusId = await _context.StatusOptionSets
                    .Where(s => s.Status.ToLower() == "verified")
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();
                    
                var verifiedBusinesses = await _context.Businesses
                    .Where(b => b.StatusId == verifiedStatusId)
                    .CountAsync();
                
                // Get new businesses count (created this month)
                var newBusinessesThisMonth = await _context.Businesses
                    .Where(b => b.CreateDate >= firstDayOfMonth && b.CreateDate <= currentDate)
                    .CountAsync();
                
                return Json(new { 
                    totalBusinesses, 
                    verifiedBusinesses, 
                    newBusinessesThisMonth 
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
