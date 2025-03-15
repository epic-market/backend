using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class ProductService : IProductService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        private readonly ICommunicationQueueService communicationQueueService;
		private readonly IApplicationConfigurationService applicationConfigurationService;
		private readonly IFileService fileService;  
        private readonly IAttachmentService attachmentService;
		private readonly IUnitOfWork unitOfWork;

		public ProductService(
                                ApplicationDbContext context,
                                IMapper mapper, 
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                IAttachmentService attachmentService,
                                ICommunicationQueueService communicationQueueService,
                                IApplicationConfigurationService applicationConfigurationService,
                                IFileService fileService,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
            this.attachmentService = attachmentService;
            this.communicationQueueService = communicationQueueService;
			this.applicationConfigurationService = applicationConfigurationService;
			this.fileService = fileService;
			this.unitOfWork = unitOfWork;
		}

        public async Task<int> AddProduct(AddProductsDto productsDto, string UserName, int businessID, string PageSource)
        {
            // Validate at least one variant exists
            if (productsDto.Variants == null || !productsDto.Variants.Any())
            {
                throw new Exception("At least one product variant is required.");
            }

            // Create the base product
            var product = mapper.Map<Catalog>(productsDto);
            product.BusinessID = businessID;
            product.CreateBy = UserName;
            product.CreateDate = DateTime.Now;
            var status = await _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == StatusConstants.UNVERIFIED);
            product.StatusId = status.Id;
            
            await _context.Catalogs.AddAsync(product);
            await unitOfWork.Complete();

            // Add variants
            if (productsDto.Variants != null && productsDto.Variants.Any())
            {
                foreach (var variantDto in productsDto.Variants)
                {
                    var variant = new CatalogVariants
                    {
                        CatalogID = product.ID,
                        SKU = variantDto.SKU,
                        Barcode = variantDto.Barcode,
                        Attributes = variantDto.Attributes,
                        SalePrice = variantDto.SalePrice,
                        CostPrice = variantDto.CostPrice,
                        CompareAtPrice = variantDto.CompareAtPrice,
                        AdditionalHightlights = variantDto.AdditionalHightlights,
                        MaximumOrderQuantity = variantDto.MaximumOrderQuantity,
                        MinimumOrderQuantity = variantDto.MinimumOrderQuantity,
                        PackedHeight = variantDto.PackedHeight,
                        PackedWidth = variantDto.PackedWidhth,
                        PackedDepth = variantDto.PackedDepth,
                        WeightUnit = variantDto.WeightUnit,
                        Weight = variantDto.Weight,
                        CreateBy = UserName,
                        CreateDate = DateTime.Now,
                        IsActive = true
                    };

                    await _context.CatalogVariants.AddAsync(variant);
                    await unitOfWork.Complete();

                    // Handle variant images
                    if (variantDto.ProductImages?.Length > 0)
                    {
                        foreach (var imageKey in variantDto.ProductImages)
                        {
                            var attachmentId = await attachmentService.GetAttachmentId(imageKey);
                            await attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                            {
                                AttachmentTypeName = AttachmentTypeConstants.PRODUCTIMAGES,
                                AttachmentID = attachmentId,
                                Entity = EntityConstants.CatelogVariant,
                                RecordID = variant.ID
                            }, businessID);
                        }
                    }

                    // Handle variant thumbnail
                    if (!string.IsNullOrEmpty(variantDto.Thumbnail))
                    {
                        var attachmentId = await attachmentService.GetAttachmentId(variantDto.Thumbnail);
                        await attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                        {
                            AttachmentTypeName = AttachmentTypeConstants.THUMBNAIL,
                            AttachmentID = attachmentId,
                            Entity = EntityConstants.CatelogVariant,
                            RecordID = variant.ID
                        }, businessID);
                    }
                }
            }

            // Log the event
            var events = EventConstants.AddCatelog;
            var saved = await _context.Catalogs.FirstOrDefaultAsync(o => o.ID == product.ID);
            string savedJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS 
            { 
                RecordId = product.ID, 
                Data = savedJson, 
                Description = null, 
                EventName = events, 
                EntityName = EntityConstants.Catelog,
                Source = PageSource 
            });

            // Add communication queue entry
            await communicationQueueService.InsertCommunicationQueue(
                new CommunicationQueueDTO()
                {
                    MessageData = null,
                    Subject = MessageDataConstants.AddCatelog,
                    NotificationRecipient = UserName,
                    ContactMethod = ContactMethodConstants.EMAIL,
                    CreateBy = UserName
                });

            return product.ID;
        }

		public async Task<int> UpdateProducts(AddProductsDto productsDto, int id, string UserName, int businessID, string PageSource)
		{
            // Validate at least one variant exists
            if (productsDto.Variants == null || !productsDto.Variants.Any())
            {
                throw new Exception("At least one product variant is required.");
            }

            var product = await _context.Catalogs
                .Include(c => c.CatalogVariants)
                .FirstOrDefaultAsync(c => c.ID == id && c.IsActive);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Update base product
            mapper.Map(productsDto, product);
            product.ModifiedBy = UserName;
            product.ModifiedDate = DateTime.Now;
            _context.Entry(product).State = EntityState.Modified;

            // Handle variants
            if (productsDto.Variants != null && productsDto.Variants.Any())
            {
                // Deactivate existing variants that are not in the update
                var updatedVariantIds = productsDto.Variants.Where(v => v.VariantId.HasValue).Select(v => v.VariantId.Value).ToList();
                var variantsToDeactivate = product.CatalogVariants
                    .Where(v => !updatedVariantIds.Contains(v.ID));
                
                foreach (var variant in variantsToDeactivate)
                {
                    variant.IsActive = false;
                    variant.ModifiedBy = UserName;
                    variant.ModifiedDate = DateTime.Now;
                }

                // Update or add variants
                foreach (var variantDto in productsDto.Variants)
                {
                    var existingVariant = product.CatalogVariants
                        .FirstOrDefault(v => v.ID == variantDto.VariantId && v.IsActive);

                    if (existingVariant != null)
                    {
                        // Update existing variant
                        existingVariant.SKU = variantDto.SKU;
                        existingVariant.Barcode = variantDto.Barcode;
                        existingVariant.Attributes = variantDto.Attributes;
                        existingVariant.SalePrice = variantDto.SalePrice;
                        existingVariant.CostPrice = variantDto.CostPrice;
                        existingVariant.CompareAtPrice = variantDto.CompareAtPrice;
                        existingVariant.AdditionalHightlights = variantDto.AdditionalHightlights;
                        existingVariant.MaximumOrderQuantity = variantDto.MaximumOrderQuantity;
                        existingVariant.MinimumOrderQuantity = variantDto.MinimumOrderQuantity;
                        existingVariant.PackedHeight = variantDto.PackedHeight;
                        existingVariant.PackedWidth = variantDto.PackedWidhth;
                        existingVariant.PackedDepth = variantDto.PackedDepth;
                        existingVariant.WeightUnit = variantDto.WeightUnit;
                        existingVariant.Weight = variantDto.Weight;
                        existingVariant.ModifiedBy = UserName;
                        existingVariant.ModifiedDate = DateTime.Now;
                        _context.CatalogVariants.Update(existingVariant);
                    }
                    else
                    {
                        // Add new variant with all fields
                        var newVariant = new CatalogVariants
                        {
                            CatalogID = product.ID,
                            SKU = variantDto.SKU,
                            Barcode = variantDto.Barcode,
                            Attributes = variantDto.Attributes,
                            SalePrice = variantDto.SalePrice,
                            CostPrice = variantDto.CostPrice,
                            CompareAtPrice = variantDto.CompareAtPrice,
                            AdditionalHightlights = variantDto.AdditionalHightlights,
                            MaximumOrderQuantity = variantDto.MaximumOrderQuantity,
                            MinimumOrderQuantity = variantDto.MinimumOrderQuantity,
                            PackedHeight = variantDto.PackedHeight,
                            PackedWidth = variantDto.PackedWidhth,
                            PackedDepth = variantDto.PackedDepth,
                            WeightUnit = variantDto.WeightUnit,
                            Weight = variantDto.Weight,
                            CreateBy = UserName,
                            CreateDate = DateTime.Now,
                            IsActive = true
                        };
                        await _context.CatalogVariants.AddAsync(newVariant);
                        await unitOfWork.Complete();

                        // Handle variant images for new variant
                        if (variantDto.ProductImages?.Length > 0)
                        {
                            foreach (var imageKey in variantDto.ProductImages)
                            {
                                var attachmentId = await attachmentService.GetAttachmentId(imageKey);
                                await attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                                {
                                    AttachmentTypeName = AttachmentTypeConstants.PRODUCTIMAGES,
                                    AttachmentID = attachmentId,
                                    Entity = EntityConstants.CatelogVariant,
                                    RecordID = newVariant.ID
                                }, businessID);
                            }
                        }

                        // Handle variant thumbnail for new variant
                        if (!string.IsNullOrEmpty(variantDto.Thumbnail))
                        {
                            var attachmentId = await attachmentService.GetAttachmentId(variantDto.Thumbnail);
                            await attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                            {
                                AttachmentTypeName = AttachmentTypeConstants.THUMBNAIL,
                                AttachmentID = attachmentId,
                                Entity = EntityConstants.CatelogVariant,
                                RecordID = newVariant.ID
                            }, businessID);
                        }
                    }
                }
            }

            await unitOfWork.Complete();

            // Log the event
            var events = EventConstants.EditCatelog;
            var saved = await _context.Catalogs.FirstOrDefaultAsync(o => o.ID == product.ID);
            string savedJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS 
            { 
                RecordId = product.ID, 
                Data = savedJson, 
                Description = null, 
                EventName = events, 
                EntityName = EntityConstants.Catelog, 
                Source = PageSource 
            });

            // Add communication queue entry
            await communicationQueueService.InsertCommunicationQueue(
                new CommunicationQueueDTO()
                {
                    MessageData = null,
                    Subject = MessageDataConstants.EditCatelog,
                    NotificationRecipient = UserName,
                    ContactMethod = ContactMethodConstants.EMAIL,
                    CreateBy = UserName
                });

            return product.ID;
		}

        public async Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productParams, int businessID)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var getResult = new GetDataResult<List<ProductResult>>();

            // 1. Filter with BusinessID
            var Products = _context.Catalogs
                .Include(c => c.CatalogVariants)
                .Include(c => c.Category)
                .Where(c => c.BusinessID == businessID && c.IsActive == true);

            // 2. Apply Searching
            var sortedProducts = Products;
            if (!String.IsNullOrEmpty(productParams.searchTerm))
            {
                sortedProducts = Products.Where(row => 
                    row.Name.Contains(productParams.searchTerm.Trim()) || 
                    row.Description.Contains(productParams.searchTerm.Trim()));
            }

            // 3. Apply Sorting
            switch (productParams.sortColumn)
            {
                case "ProductID":
                    sortedProducts = productParams.ascending ? 
                        sortedProducts.OrderBy(c => c.ID) : 
                        sortedProducts.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    sortedProducts = productParams.ascending ? 
                        sortedProducts.OrderBy(c => c.Name) : 
                        sortedProducts.OrderByDescending(c => c.Name);
                    break;
                default:
                    break;
            }

            // Get total count
            int totalCount = await sortedProducts.CountAsync();

            // 4. Apply pagination
            var pagedProducts = sortedProducts
                .Skip((productParams.PageIndex - 1) * productParams.pageSize)
                .Take(productParams.pageSize);

            // 5. Select data with all fields
            var results = await pagedProducts.Select(c => new ProductResult()
            {
                ProductId = c.ID,
                Name = c.Name,
                Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == c.StatusId).Status,
                Category = c.Category.Name,
                NoOfVariants = c.CatalogVariants.Count.ToString(),
                Price = c.CatalogVariants.Any() ? c.CatalogVariants.Min(v => v.SalePrice) : 0,
                Thumbnail = ((from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            join variant in c.CatalogVariants on link.RecordID equals variant.ID
                            where entity.Name == EntityConstants.CatelogVariant && 
                                  link.AttachmentTypeID == attachmentTypeID.ID
                            orderby variant.ID
                            select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}")
                            .FirstOrDefault())
            }).ToListAsync();

            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

		public async Task<List<ProductsMapOptionResult>> GetAllProductForMap(int BusinessID, int BranchId)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

            var returnedProducts = await (from catalogItem in _context.Catalogs
                                    where catalogItem.BusinessID == BusinessID && catalogItem.IsActive == true
                                    select new ProductsMapOptionResult
                                    {
                                        ProductId = catalogItem.ID,
                                        Name = catalogItem.Name,
                                        Description = catalogItem.Description,
                                        Variants = (from v in _context.CatalogVariants
                                                where v.CatalogID == catalogItem.ID
                                                select new VariantResultTemp
                                                {
                                                    VariantId = v.ID,
                                                    Attributes = v.Attributes,
                                                    SKU = v.SKU,
                                                    SalePrice = v.SalePrice,
                                                    Selected = _context.Inventory
                                                        .Where(i => i.OutletID == BranchId)
                                                        .Any(i => i.ProductVariantID == v.ID),
                                                    Thumbnail = (from attachment in _context.Attachments
                                                               join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                                               join entity in _context.Entity on link.EntityID equals entity.ID
                                                               where entity.Name == EntityConstants.CatelogVariant && 
                                                                   link.RecordID == v.ID && 
                                                                   link.AttachmentTypeID == attachmentTypeID.ID
                                                               select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}")
                                                               .FirstOrDefault()
                                                })
                                                .AsEnumerable()
                                                .Select(v => new VariantResult
                                                {
                                                    VariantId = v.VariantId,
                                                    SKU = v.SKU,
                                                    SalePrice = v.SalePrice,
                                                    Attributes = v.Attributes,
                                                    Selected = v.Selected,
                                                    Thumbnail = v.Thumbnail
                                                })
                                                .ToList()
                                    }).ToListAsync();

            return returnedProducts;
        }

        public async Task<GetDataResult<List<ProductForPOSResult>>> GetAllProductsForPOS(ProductPOSParams productParams, int outletId)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var getResult = new GetDataResult<List<ProductForPOSResult>>();

            // 1. Filter with OutletID and active products
            var products = _context.Inventory
                .Include(c => c.CatalogVariants)
                    .ThenInclude(v => v.Catalog)
                        .ThenInclude(c => c.Category)
                .Where(c => c.OutletID == outletId && 
                            c.CatalogVariants.Catalog.IsActive)
                .AsQueryable();

            // 2. Apply Searching
            if (!string.IsNullOrEmpty(productParams.searchTerm))
            {
                var searchTerm = productParams.searchTerm.Trim();
                products = products.Where(row => 
                    row.CatalogVariants.Catalog.Name.Contains(searchTerm) || 
                    row.CatalogVariants.Catalog.Description.Contains(searchTerm));
            }

            // Get total count
            int totalCount = await products.CountAsync();

            // 3. Apply pagination
            var pagedProducts = products
                .Skip((productParams.PageIndex - 1) * productParams.pageSize)
                .Take(productParams.pageSize);

            // 4. Group by category and map to result
            var results = await pagedProducts
                .GroupBy(p => p.CatalogVariants.Catalog.Category.Name)
                .Select(g => new ProductForPOSResult
                {
                    Category = g.Key,
                    Products = g.GroupBy(p => p.CatalogVariants.CatalogID)
                        .Select(p => new ProductsForCategory
                        {
                            ProductId = p.Key,
                            Name = p.First().CatalogVariants.Catalog.Name,
                            VarientResulForPos = p.Select(v => new VarientResulForPos
                            {
                                VariantId = v.CatalogVariants.ID,
                                SKU = v.CatalogVariants.SKU,
                                Attributes = v.CatalogVariants.Attributes,
                                SalePrice = v.CatalogVariants.SalePrice,
                                QuantityAvailable = v.TrackInventory ? v.QuantityAvailable ?? 0 : (v.IsInStock ? 999999 : 0),
                                Barcode = v.CatalogVariants.Barcode,
                                Thumbnail = (from attachment in _context.Attachments
                                           join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                           join entity in _context.Entity on link.EntityID equals entity.ID
                                           where entity.Name == EntityConstants.CatelogVariant && 
                                                 link.RecordID == v.CatalogVariants.ID && 
                                                 link.AttachmentTypeID == attachmentTypeID.ID
                                           select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}")
                                           .FirstOrDefault()
                            }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            return new GetDataResult<List<ProductForPOSResult>>
            {
                items = results,
                Count = totalCount
            };
        }

        public async Task<GetDataResult<List<CustomerResultBaseOnCategory>>> GetAllProductsForMobile(ProductMobileParams parameters)
        {  
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var VerifiedStatusID = _context.StatusOptionSets.FirstOrDefault(c => c.Status == StatusConstants.VERIFIED).Id;
            
            // Base query
            var query = _context.Inventory
                .Include(p => p.CatalogVariants)
                .Include(p => p.CatalogVariants.Catalog)
                .Include(p => p.CatalogVariants.Catalog.Category)
                .Where(p => p.OutletID == parameters.OutletId && 
                            p.CatalogVariants.Catalog.StatusId == VerifiedStatusID &&
                            p.CatalogVariants.Catalog.IsActive)
                .AsQueryable();

            // Apply Category Filter
            if (!string.IsNullOrEmpty(parameters.Category))
            {
                query = query.Where(p => p.CatalogVariants.Catalog.Category.Name == parameters.Category);
            }

            // Apply Search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.CatalogVariants.Catalog.Name.ToLower().Contains(searchTerm) ||
                    p.CatalogVariants.Catalog.Description.ToLower().Contains(searchTerm));
            }

            // Apply Sorting
            query = ApplySorting(query, parameters.SortBy, parameters.SortOrder);

            // Get total count for pagination
            var totalItems = await query.CountAsync();

            // Apply pagination
            var pagedQuery = query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

            // Group and map to result with updated variant information
            var results = await pagedQuery
                .GroupBy(p => p.CatalogVariants.Catalog.Category.Name)
                .Select(g => new CustomerResultBaseOnCategory
                {
                    Category = g.Key,
                    CustomerProductResults = g.GroupBy(p => p.CatalogVariants.CatalogID)
                        .Select(p => new CustomerProductResult
                        {
                            ProductId = p.Key,
                            Name = p.First().CatalogVariants.Catalog.Name,
                            Description = p.First().CatalogVariants.Catalog.Description,
                            Rating = p.First().CatalogVariants.Catalog.Rating,
                            RatingCount = p.First().CatalogVariants.Catalog.ReviewCount,
                            IsRecommended = p.First().CatalogVariants.Catalog.IsRecommended,
                            Variants = p.Select(v => new VarientResultForCustomer
                            {
                                VariantID = v.CatalogVariants.ID,
                                Attributes = v.CatalogVariants.Attributes,
                                SalePrice = v.CatalogVariants.SalePrice,
                                CompareAtPrice = v.CatalogVariants.CompareAtPrice,
                                AdditionalHightlights = v.CatalogVariants.AdditionalHightlights,
                                MaximumOrderQuantity = v.CatalogVariants.MaximumOrderQuantity,
                                MinimumOrderQuantity = v.CatalogVariants.MinimumOrderQuantity
                            }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            return new GetDataResult<List<CustomerResultBaseOnCategory>>
            {
                items = results,
                Count = totalItems
            };
        }
        private IQueryable<Inventory> ApplySorting(
            IQueryable<Inventory> query,
            string sortBy,
            string sortOrder)
        {
            return (sortBy?.ToLower(), sortOrder?.ToLower()) switch
            {
                ("name", "ascending") =>
                    query.OrderBy(p => p.CatalogVariants.Catalog.Name),

                ("name", "descending") =>
                    query.OrderByDescending(p => p.CatalogVariants.Catalog.Name),

                ("price", "ascending") =>
                    query.OrderBy(p => p.CatalogVariants.SalePrice),

                ("price", "descending") =>
                    query.OrderByDescending(p => p.CatalogVariants.SalePrice),

                ("rating", "ascending") =>
                    query.OrderBy(p => p.CatalogVariants.Catalog.Rating),

                ("rating", "descending") =>
                    query.OrderByDescending(p => p.CatalogVariants.Catalog.Rating),

                ("newest", "ascending") =>
                    query.OrderBy(p => p.CatalogVariants.Catalog.CreateDate),

                ("newest", "descending") =>
                    query.OrderByDescending(p => p.CatalogVariants.Catalog.CreateDate),

                ("popular", "ascending") =>
                    query.OrderBy(p => p.CatalogVariants.Catalog.ReviewCount),

                ("popular", "descending") =>
                    query.OrderByDescending(p => p.CatalogVariants.Catalog.ReviewCount),

                // Default sorting by name ascending
                _ => query.OrderBy(p => p.CatalogVariants.Catalog.Name)
            };
        }

        public async Task<ProductsDto> GetProductDetails(int productId)
        {
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);

            return await _context.Catalogs
                .Include(c => c.CatalogVariants)
                .Include(c => c.Category)
                .Where(c => c.ID == productId && c.IsActive == true)
                .Select(c => new ProductsDto
                {
                    Id = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    Category = c.Category.Name,
                    RequiresRefrigeration = c.RequiresRefrigeration,
                    BaseHightlights = c.BaseHightlights,
                    IsRecommended = c.IsRecommended,
                    VariantOptions = c.VariantOptions,
                    Variants = c.CatalogVariants.Where(v => v.IsActive).Select(v => new VariantResultForDetails
                    {
                        VariantId = v.ID,
                        SKU = v.SKU,
                        Attributes = v.Attributes,
                        SalePrice = v.SalePrice,
                        CompareAtPrice = v.CompareAtPrice,
                        AdditionalHightlights = v.AdditionalHightlights,
                        MaximumOrderQuantity = v.MaximumOrderQuantity,
                        MinimumOrderQuantity = v.MinimumOrderQuantity,
                        PackedHeight = v.PackedHeight,
                        PackedWidhth = v.PackedWidth,
                        PackedDepth = v.PackedDepth,
                        WeightUnit = v.WeightUnit,
                        Weight = v.Weight,
                        InStock = v.Inventory.Any(i => i.IsInStock || (i.TrackInventory && i.QuantityAvailable > 0)),
                        IsBackOrder = v.Inventory.Any(i => i.BackOrders),
                        Images = (from attachment in _context.Attachments
                                 join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                 join entity in _context.Entity on link.EntityID equals entity.ID
                                 where entity.Name == EntityConstants.CatelogVariant && 
                                       link.RecordID == v.ID && 
                                       link.AttachmentTypeID == attachmentTypeID_Product.ID
                                 select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}")
                                 .ToList(),
                        Thumbnail = (from attachment in _context.Attachments
                                   join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                   join entity in _context.Entity on link.EntityID equals entity.ID
                                   where entity.Name == EntityConstants.CatelogVariant && 
                                         link.RecordID == v.ID && 
                                         link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                   select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}")
                                   .FirstOrDefault()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> VerifyCatalog(VerifyCatalogDto verifyCatalogDto, string UserName, int AdminPersonID, string PageSource)
        {
            // Check if all catalogs exist and are active
            var allCatalogsExist = verifyCatalogDto.ListOfCatalogIDs.All(id => _context.Catalogs.Any(c => c.ID == id && c.IsActive));
            if (!allCatalogsExist)
            {
                throw new Exception("One or more catalogs do not exist or are deleted.");
            }

            // Check if all catalogs are in not verified state
            var allCatalogsNotVerifiedOrPending = verifyCatalogDto.ListOfCatalogIDs.All(id => !_context.Catalogs.Any(c => c.ID == id && (c.StatusId == _context.StatusOptionSets.Where(s => s.Status == StatusConstants.VERIFIED).FirstOrDefault().Id || c.StatusId == _context.StatusOptionSets.Where(s => s.Status == StatusConstants.PENDING).FirstOrDefault().Id)));
            if (!allCatalogsNotVerifiedOrPending)
            {
                throw new Exception("One or more catalogs are already in the 'Verified' or 'Pending' state.");
            }

            //Check if any are already in send to verification state
            var anyCatalogInSendToVerification = verifyCatalogDto.ListOfCatalogIDs.Any(id => _context.Catalogs.Any(c => c.ID == id && c.StatusId == _context.StatusOptionSets.Where(s => s.Status == StatusConstants.SENDTOVERIFICATION).FirstOrDefault().Id));
            if (anyCatalogInSendToVerification)
            {
                throw new Exception("One or more catalogs are already in the 'Send To Verification' state.");
            }

            //Update all catalogs status to send for verification state
            foreach(var id in verifyCatalogDto.ListOfCatalogIDs)
            {
                var catalog = await _context.Catalogs.FindAsync(id);
                catalog.StatusId = _context.StatusOptionSets.FirstOrDefault(s => s.Status == StatusConstants.SENDTOVERIFICATION).Id;
                catalog.ModifiedBy = UserName;
                catalog.ModifiedDate = DateTime.Now;
                _context.Catalogs.Update(catalog);
            }

            await unitOfWork.Complete();

            var newTaskStatus = await _context.TaskStatusTypes.Where(row => row.Status == "New").FirstOrDefaultAsync();
            var taskTypeID = await _context.TaskTypes.Where(row => row.Name == "Verification").FirstOrDefaultAsync();
            var userName = await _context.Users.Where(row => row.UserName == UserName).FirstOrDefaultAsync();
            var taskEntity = await _context.Entity.Where(row => row.Name == EntityConstants.Catelog).FirstOrDefaultAsync();

            var taskToSave = new Tasks
            {
                Name = VerificationConstants.CatelogName,
                Description = VerificationConstants.CatelogDescription,
                TaskTypeID = taskTypeID.ID,
                ParentID = null,
                TaskStatusID = newTaskStatus.ID,
                TaskPriorityID = 1,
                PrimaryAssignedToPersonID = AdminPersonID,
                TaskEntityID = taskEntity.ID,
                DateAssigned = DateTime.Now,
                SubmittedByPersonID = userName.Id,
                TaskData = string.Join(",", verifyCatalogDto.ListOfCatalogIDs),
                ReceivedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreateBy = userName.Email
            };

            await _context.Tasks.AddAsync(taskToSave);
            await unitOfWork.Complete();

            return taskToSave.ID;
        }

        public async Task<InventoryResult> GetProductInventoryDetails(int productVariantId, int branchId)
        {
            var productAdvanced = await _context.Inventory
                .Where(c => c.OutletID == branchId && c.ProductVariantID == productVariantId)
                .Select(c => new InventoryResult {
                    BackOrders = c.BackOrders,
                    MaximumStockLevel = c.MaximumStockLevel ?? 0,  
                    MinimumStockLevel = c.MinimumStockLevel ?? 0,
                    QuantityAvailable = c.QuantityAvailable ?? 0,
                    ReorderPoint = c.ReorderPoint ?? 0,    
                    ProductVariantId = c.ProductVariantID,
                    BranchId = branchId,
                    TrackInventory = c.TrackInventory,
                    IsInStock = c.IsInStock
                })
                .FirstOrDefaultAsync();

            if (productAdvanced == null)
            {
                throw new Exception($"No inventory record found for product variant {productVariantId} at branch {branchId}");
            }

            return productAdvanced;
        }

        public async Task deleteCatelog(int id, string UserName)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
			if (catalog != null)
			{
				catalog.IsActive = false;
				_context.Catalogs.Update(catalog);
                _context.CatalogVariants.Where(v => v.CatalogID == id).ToList().ForEach(v => v.IsActive = false);
				await unitOfWork.Complete();
			}
			else
			{
				throw new Exception("Product Not Found");
			}
        }

        public async Task<int> QuickActions(QuickActionsParams quickActionsParams, string UserName)
        {
            var catalog = await _context.Catalogs.Where(c=> c.ID == quickActionsParams.ProductId && c.IsActive).FirstOrDefaultAsync();
			if (catalog != null)
			{
				//catalog.InStock= quickActionsParams.InStock == null ? catalog.InStock : quickActionsParams.InStock.Value;
                catalog.IsRecommended = quickActionsParams.IsRecommended == null ? catalog.IsRecommended : quickActionsParams.IsRecommended.Value;
                catalog.ModifiedDate = DateTime.Now;
                catalog.ModifiedBy = UserName;
                _context.Catalogs.Update(catalog);
				await unitOfWork.Complete();
                return catalog.ID;
			}
			else
			{
				throw new Exception("Product Not Found");
			}
        }

        public async Task AddOrUpdateProductInventoryDetails(InventoryResult productAdvanced)
        {
            // First, validate that the product variant belongs to the business that owns the branch
            var isValidProduct = await _context.CatalogVariants
                .Include(cv => cv.Catalog)
                .Where(cv => cv.ID == productAdvanced.ProductVariantId)
                .AnyAsync(cv => 
                    cv.Catalog.BusinessID == _context.Outlets
                        .Where(o => o.ID == productAdvanced.BranchId)
                        .Select(o => o.BussinessID)
                        .FirstOrDefault() &&
                    cv.IsActive && 
                    cv.Catalog.IsActive);

            if (!isValidProduct)
            {
                throw new Exception("Cannot add inventory for a product that doesn't belong to this business branch");
            }

            // Validate QuantityAvailable when TrackInventory is true
            if (productAdvanced.TrackInventory && !productAdvanced.QuantityAvailable.HasValue)
            {
                throw new Exception("Quantity Available is required when Track Inventory is enabled");
            }

            var outletProductInventory = _context.Inventory.FirstOrDefault(c => 
                c.ProductVariantID == productAdvanced.ProductVariantId && 
                c.OutletID == productAdvanced.BranchId);

            if (outletProductInventory == null)
            {
                outletProductInventory = new Inventory
                {
                    TrackInventory = productAdvanced.TrackInventory,
                    IsInStock = productAdvanced.IsInStock,
                    BackOrders = productAdvanced.BackOrders,
                    MaximumStockLevel = productAdvanced.MaximumStockLevel,
                    MinimumStockLevel = productAdvanced.MinimumStockLevel,
                    QuantityAvailable = productAdvanced.TrackInventory ? productAdvanced.QuantityAvailable : null,
                    ReorderPoint = productAdvanced.ReorderPoint,
                    ProductVariantID = productAdvanced.ProductVariantId,
                    OutletID = productAdvanced.BranchId
                };
                await _context.Inventory.AddAsync(outletProductInventory);
            }
            else
            {
                outletProductInventory.TrackInventory = productAdvanced.TrackInventory;
                outletProductInventory.IsInStock = productAdvanced.IsInStock;
                outletProductInventory.BackOrders = productAdvanced.BackOrders;
                outletProductInventory.MaximumStockLevel = productAdvanced.MaximumStockLevel;
                outletProductInventory.MinimumStockLevel = productAdvanced.MinimumStockLevel;
                outletProductInventory.QuantityAvailable = productAdvanced.TrackInventory ? productAdvanced.QuantityAvailable : null;
                outletProductInventory.ReorderPoint = productAdvanced.ReorderPoint;
                _context.Inventory.Update(outletProductInventory);
            }

            await unitOfWork.Complete();
        }



         //all varients
        // public async Task<int> AddProductVariant(int productId, ProductVariantDto variantDto, string userName)
        // {
        //     var product = await _context.Catalogs
        //         .FirstOrDefaultAsync(c => c.ID == productId && c.IsActive);
            
        //     if (product == null)
        //         throw new Exception("Product not found");

        //     var existingVariant = await _context.CatalogVariants.FirstOrDefaultAsync(v => v.CatalogID == productId && v.SKU == variantDto.SKU);
        //     if (existingVariant != null)
        //         throw new Exception("Variant already exists");

        //     var variant = new CatalogVariants
        //     {
        //         CatalogID = productId,
        //         SKU = variantDto.SKU,
        //         SalePrice = variantDto.SalePrice,
        //         CostPrice = variantDto.CostPrice,
        //         AdditionalHightlights = variantDto.AdditionalHightlights,
        //         CreateBy = userName,
        //         CreateDate = DateTime.Now,
        //         IsActive = true
        //     };

        //     await _context.CatalogVariants.AddAsync(variant);
        //     await unitOfWork.Complete();
            
        //     return variant.ID;
        // }
        public async Task<List<ProductVariantResponse>> GetProductVariants(int productId)
        {
            var variants = await _context.CatalogVariants
                .Where(v => v.CatalogID == productId && v.IsActive)
                .Select(v => new ProductVariantResponse
                {
                    VariantID = v.ID,
                    ProductID = v.CatalogID,
                    Attributes = v.Attributes,
                    SKU = v.SKU,
                    SalePrice = v.SalePrice,
                    CostPrice = v.CostPrice,
                    AdditionalHightlights = v.AdditionalHightlights
                })
                .ToListAsync();

            return variants;
        }

        // public async Task<ProductVariantResponse> GetProductVariant(int variantId)
        // {
        //     var variant = await _context.CatalogVariants
        //         .Where(v => v.ID == variantId && v.IsActive)
        //         .Select(v => new ProductVariantResponse
        //         {
        //             VariantID = v.ID,
        //             ProductID = v.CatalogID,
        //             SKU = v.SKU,
        //             SalePrice = v.SalePrice,
        //             CostPrice = v.CostPrice,
        //             AdditionalHightlights = v.AdditionalHightlights
        //         })
        //         .FirstOrDefaultAsync();

        //     if (variant == null)
        //         throw new Exception("Product variant not found");

        //     return variant;
        // }

        // public async Task UpdateProductVariant(int variantId, ProductVariantDto variantDto, string userName)
        // {
        //     var variant = await _context.CatalogVariants
        //         .FirstOrDefaultAsync(v => v.ID == variantId && v.IsActive);

        //     if (variant == null)
        //         throw new Exception("Product variant not found");

        //     variant.SKU = variantDto.SKU;
        //     variant.SalePrice = variantDto.SalePrice;
        //     variant.CostPrice = variantDto.CostPrice;
        //     variant.AdditionalHightlights = variantDto.AdditionalHightlights;
        //     variant.ModifiedBy = userName;
        //     variant.ModifiedDate = DateTime.Now;

        //     _context.CatalogVariants.Update(variant);
        //     await unitOfWork.Complete();
        // }

    }


}
