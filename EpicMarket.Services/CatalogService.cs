using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
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

        public async Task<int> AddProduct(AddProductsParams productsDto, string UserName, int businessID, string PageSource)
        {
            // Validate at least one variant exists
            if (productsDto.Variants == null || !productsDto.Variants.Any())
            {
                throw new Exception("At least one product variant is required.");
            }

            // Create the base product
            var product = new Catalog
            {
                Name = productsDto.Name,
                Description = productsDto.Description,
                CategoryID = productsDto.CategoryId,
                RequiresRefrigeration = productsDto.RequiresRefrigeration,
                IsRecommended = productsDto.IsRecommended,
                BaseHightlights = JsonConvert.SerializeObject(productsDto.BaseHightlights),
                VariantOptions = JsonConvert.SerializeObject(productsDto.VariantOptions),
                BusinessID = businessID,
                CreateBy = UserName,
                CreateDate = DateTime.Now,
                StatusId =  _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == StatusConstants.UNVERIFIED).GetAwaiter().GetResult().Id
            };
            
            await _context.Products.AddAsync(product);
            await unitOfWork.Complete();

            // Add variants
            if (productsDto.Variants != null && productsDto.Variants.Any())
            {
                foreach (var variantDto in productsDto.Variants)
                {

                    var attributesString = JsonConvert.SerializeObject(variantDto.Attributes);
                    var additionalHightlightsString = JsonConvert.SerializeObject(variantDto.AdditionalHightlights);

                    var variant = new CatalogVariants
                    {
                        ProductID = product.ID,
                        SKU = variantDto.SKU,
                        Barcode = variantDto.Barcode,
                        Attributes = attributesString,
                        SalePrice = variantDto.SalePrice,
                        CostPrice = variantDto.CostPrice,
                        CompareAtPrice = variantDto.CompareAtPrice,
                        AdditionalHightlights = additionalHightlightsString,
                        MaximumOrderQuantity = variantDto.MaximumOrderQuantity,
                        MinimumOrderQuantity = variantDto.MinimumOrderQuantity,
                        PackedHeight = variantDto.PackedHeight,
                        PackedWidth = variantDto.PackedWidhth,
                        PackedDepth = variantDto.PackedDepth,
                        WeightUnit = variantDto.WeightUnit,
                        Weight = variantDto.Weight,
                        CreateBy = UserName,
                        CreateDate = DateTime.Now,
                        IsDefaultVariant = variantDto.IsDefaultVariant
                    };

                    await _context.ProductVariants.AddAsync(variant);
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
            var saved = await _context.Products.FirstOrDefaultAsync(o => o.ID == product.ID);
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
            return product.ID;
        }

		public async Task<int> UpdateProducts(AddProductsParams productsDto, int id, string UserName, int businessID, string PageSource)
		{
            // Validate at least one variant exists
            if (productsDto.Variants == null || !productsDto.Variants.Any())
            {
                throw new Exception("At least one product variant is required.");
            }

            var product = await _context.Products
                .Include(c => c.ProductVariants)
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
                var variantsToDeactivate = product.ProductVariants
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
                    var existingVariant = product.ProductVariants
                        .FirstOrDefault(v => v.ID == variantDto.VariantId && v.IsActive);

                    if (existingVariant != null)
                    {
                        var attributesString = JsonConvert.SerializeObject(variantDto.Attributes);
                        var additionalHightlightsString = JsonConvert.SerializeObject(variantDto.AdditionalHightlights);
                        // Update existing variant
                        existingVariant.SKU = variantDto.SKU;
                        existingVariant.Barcode = variantDto.Barcode;
                        existingVariant.Attributes = attributesString;
                        existingVariant.SalePrice = variantDto.SalePrice;
                        existingVariant.CostPrice = variantDto.CostPrice;
                        existingVariant.CompareAtPrice = variantDto.CompareAtPrice;
                        existingVariant.AdditionalHightlights = additionalHightlightsString;
                        existingVariant.MaximumOrderQuantity = variantDto.MaximumOrderQuantity;
                        existingVariant.MinimumOrderQuantity = variantDto.MinimumOrderQuantity;
                        existingVariant.PackedHeight = variantDto.PackedHeight;
                        existingVariant.PackedWidth = variantDto.PackedWidhth;
                        existingVariant.PackedDepth = variantDto.PackedDepth;
                        existingVariant.WeightUnit = variantDto.WeightUnit;
                        existingVariant.Weight = variantDto.Weight;
                        existingVariant.ModifiedBy = UserName;
                        existingVariant.ModifiedDate = DateTime.Now;
                        _context.ProductVariants.Update(existingVariant);
                    }
                    else
                    {
                        // Add new variant with all fields
                        var attributesString = JsonConvert.SerializeObject(variantDto.Attributes);
                        var additionalHightlightsString = JsonConvert.SerializeObject(variantDto.AdditionalHightlights);
                        var newVariant = new CatalogVariants
                        {
                            ProductID = product.ID,
                            SKU = variantDto.SKU,
                            Barcode = variantDto.Barcode,
                            Attributes = attributesString,
                            SalePrice = variantDto.SalePrice,
                            CostPrice = variantDto.CostPrice,
                            CompareAtPrice = variantDto.CompareAtPrice,
                            AdditionalHightlights = additionalHightlightsString,
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
                        await _context.ProductVariants.AddAsync(newVariant);
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
            var saved = await _context.Products.FirstOrDefaultAsync(o => o.ID == product.ID);
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

            return product.ID;
		}

        public async Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductListParams productParams, int businessID)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var getResult = new GetDataResult<List<ProductResult>>();

            // 1. Filter with BusinessID
            var Products = _context.Products
                .Include(c => c.ProductVariants)
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
                NoOfVariants = c.ProductVariants.Count.ToString(),
                Price = c.ProductVariants.Any() ? c.ProductVariants.Min(v => v.SalePrice) : 0,
                Thumbnail = ((from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            join variant in c.ProductVariants on link.RecordID equals variant.ID
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

            var returnedProducts = await (from catalogItem in _context.Products
                                    where catalogItem.BusinessID == BusinessID && catalogItem.IsActive == true
                                    select new ProductsMapOptionResult
                                    {
                                        ProductId = catalogItem.ID,
                                        Name = catalogItem.Name,
                                        Description = catalogItem.Description,
                                        Variants = (from v in _context.ProductVariants
                                                where v.ProductID == catalogItem.ID
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
                .Include(c => c.ProductVariants)
                    .ThenInclude(v => v.Product)
                        .ThenInclude(c => c.Category)
                .Where(c => c.OutletID == outletId && 
                            c.ProductVariants.Product.IsActive)
                .AsQueryable();

            // 2. Apply Searching
            if (!string.IsNullOrEmpty(productParams.searchTerm))
            {
                var searchTerm = productParams.searchTerm.Trim();
                products = products.Where(row => 
                    row.ProductVariants.Product.Name.Contains(searchTerm) || 
                    row.ProductVariants.Product.Description.Contains(searchTerm));
            }

            // Get total count
            int totalCount = await products.CountAsync();

            // 3. Apply pagination
            var pagedProducts = products
                .Skip((productParams.PageIndex - 1) * productParams.pageSize)
                .Take(productParams.pageSize);

            // 4. Group by category and map to result
            var results = await pagedProducts
                .GroupBy(p => p.ProductVariants.Product.Category.Name)
                .Select(g => new ProductForPOSResult
                {
                    Category = g.Key,
                    Products = g.GroupBy(p => p.ProductVariants.ProductID)
                        .Select(p => new ProductsForCategory
                        {
                            ProductId = p.Key,
                            Name = p.First().ProductVariants.Product.Name,
                            VarientResulForPos = p.Select(v => new VarientResulForPos
                            {
                                VariantId = v.ProductVariants.ID,
                                SKU = v.ProductVariants.SKU,
                                Attributes = v.ProductVariants.Attributes,
                                SalePrice = v.ProductVariants.SalePrice,
                                QuantityAvailable = v.TrackInventory ? v.QuantityAvailable ?? 0 : (v.IsInStock ? 999999 : 0),
                                Barcode = v.ProductVariants.Barcode,
                                Thumbnail = (from attachment in _context.Attachments
                                           join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                           join entity in _context.Entity on link.EntityID equals entity.ID
                                           where entity.Name == EntityConstants.CatelogVariant && 
                                                 link.RecordID == v.ProductVariants.ID && 
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

        public async Task<GetDataResult<List<CustomerProductResult>>> GetAllProductsForMobile(ProductMobileParams parameters)
        {  
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var VerifiedStatusID = _context.StatusOptionSets.FirstOrDefault(c => c.Status == StatusConstants.VERIFIED).Id;
            
            // Base query
            var query = _context.Inventory
                .Include(p => p.ProductVariants)
                .Include(p => p.ProductVariants.Product)
                .Include(p => p.ProductVariants.Product.Category)
                .Where(p => p.OutletID == parameters.OutletId && 
                            p.ProductVariants.Product.StatusId == VerifiedStatusID &&
                            p.ProductVariants.Product.IsActive)
                .AsQueryable();

            // Apply Category Filter
            if (parameters.CategoryId > 0)
            {
                query = query.Where(p => p.ProductVariants.Product.Category.ID == parameters.CategoryId);
            }

            // Apply Search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.ProductVariants.Product.Name.ToLower().Contains(searchTerm) ||
                    p.ProductVariants.Product.Description.ToLower().Contains(searchTerm));
            }

            // Apply Sorting
            query = ApplySorting(query, parameters.SortBy, parameters.SortOrder);

            // Get total count for pagination
            var totalItems = await query.CountAsync();

            // Apply pagination
            var pagedQuery = query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

            // Map to result directly without category grouping
            var results = await pagedQuery
                .GroupBy(p => p.ProductVariants.ProductID)
                .Select(p => new CustomerProductResult
                {
                    ProductId = p.Key,
                    Name = p.First().ProductVariants.Product.Name,
                    Description = p.First().ProductVariants.Product.Description,
                    Rating = p.First().ProductVariants.Product.Rating,
                    RatingCount = p.First().ProductVariants.Product.ReviewCount,
                    IsRecommended = p.First().ProductVariants.Product.IsRecommended,
                    Variants = p.Select(v => new VarientResultForCustomer
                    {
                        VariantID = v.ProductVariants.ID,
                        Attributes = string.IsNullOrEmpty(v.ProductVariants.Attributes) ? null : JsonConvert.DeserializeObject<List<AttributeDto>>(v.ProductVariants.Attributes),
                        SalePrice = v.ProductVariants.SalePrice,
                        CompareAtPrice = v.ProductVariants.CompareAtPrice,
                        AdditionalHightlights = string.IsNullOrEmpty(v.ProductVariants.AdditionalHightlights) ? null : JsonConvert.DeserializeObject<List<HighlightDto>>(v.ProductVariants.AdditionalHightlights),
                        MaximumOrderQuantity = v.ProductVariants.MaximumOrderQuantity,
                        MinimumOrderQuantity = v.ProductVariants.MinimumOrderQuantity
                    }).ToList()
                })
                .ToListAsync();

            return new GetDataResult<List<CustomerProductResult>>
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
                    query.OrderBy(p => p.ProductVariants.Product.Name),

                ("name", "descending") =>
                    query.OrderByDescending(p => p.ProductVariants.Product.Name),

                ("price", "ascending") =>
                    query.OrderBy(p => p.ProductVariants.SalePrice),

                ("price", "descending") =>
                    query.OrderByDescending(p => p.ProductVariants.SalePrice),

                ("rating", "ascending") =>
                    query.OrderBy(p => p.ProductVariants.Product.Rating),

                ("rating", "descending") =>
                    query.OrderByDescending(p => p.ProductVariants.Product.Rating),

                ("newest", "ascending") =>
                    query.OrderBy(p => p.ProductVariants.Product.CreateDate),

                ("newest", "descending") =>
                    query.OrderByDescending(p => p.ProductVariants.Product.CreateDate),

                ("popular", "ascending") =>
                    query.OrderBy(p => p.ProductVariants.Product.ReviewCount),

                ("popular", "descending") =>
                    query.OrderByDescending(p => p.ProductVariants.Product.ReviewCount),

                // Default sorting by name ascending
                _ => query.OrderBy(p => p.ProductVariants.Product.Name)
            };
        }

        public async Task<ProductDetailsResult> GetProductDetails(int productId)
        {
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);

            return await _context.Products
                .Include(c => c.ProductVariants)
                .Include(c => c.Category)
                .Where(c => c.ID == productId && c.IsActive == true)
                .Select(c => new ProductDetailsResult
                {
                    ProductId = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    Category = new CategoryDto { CategoryId = c.Category.ID, CategoryName = c.Category.Name },
                    RequiresRefrigeration = c.RequiresRefrigeration,
                    BaseHightlights = JsonConvert.DeserializeObject<List<HighlightDto>>(c.BaseHightlights),
                    IsRecommended = c.IsRecommended,
                    VariantOptions = JsonConvert.DeserializeObject<List<VarientOptionDto>>(c.VariantOptions),
                    Variants = c.ProductVariants.Where(v => v.IsActive).Select(v => new ProductVariantResult
                    {
                        VariantId = v.ID,
                        SKU = v.SKU,
                        Attributes = JsonConvert.DeserializeObject<List<AttributeDto>>(v.Attributes),
                        SalePrice = v.SalePrice,
                        CompareAtPrice = v.CompareAtPrice,
                        CostPrice = v.CostPrice,
                        AdditionalHightlights = JsonConvert.DeserializeObject<List<HighlightDto>>(v.AdditionalHightlights),
                        MaximumOrderQuantity = v.MaximumOrderQuantity,
                        MinimumOrderQuantity = v.MinimumOrderQuantity,
                        PackedHeight = v.PackedHeight,
                        PackedWidth = v.PackedWidth,
                        PackedDepth = v.PackedDepth,
                        WeightUnit = v.WeightUnit,
                        Weight = v.Weight,
                        IsDefaultVariant = v.IsDefaultVariant,
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

        public async Task<CustomerProductDetailsResult> GetCustomerProductDetails(int productId)
        {
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);
            var verifiedStatusId = _context.StatusOptionSets.FirstOrDefault(c => c.Status == StatusConstants.VERIFIED).Id;

            var product = await _context.Products
                .Include(c => c.ProductVariants)
                .Include(c => c.Category)
                .Where(c => c.ID == productId && c.IsActive == true && c.StatusId == verifiedStatusId)
                .Select(c => new CustomerProductDetailsResult
                {
                    ProductId = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    Category = new CategoryDto { CategoryId = c.Category.ID, CategoryName = c.Category.Name },
                    Rating = c.Rating,
                    RatingCount = c.ReviewCount,
                    IsRecommended = c.IsRecommended,
                    BaseHightlights = string.IsNullOrEmpty(c.BaseHightlights) ? 
                        new List<HighlightDto>() : 
                        JsonConvert.DeserializeObject<List<HighlightDto>>(c.BaseHightlights) ?? new List<HighlightDto>(),
                    Variants = c.ProductVariants
                        .Where(v => v.IsActive)
                        .Select(v => new CustomerProductVariantResult
                        {
                            VariantId = v.ID,
                            SKU = v.SKU,
                            Attributes = string.IsNullOrEmpty(v.Attributes) ? 
                                new List<AttributeDto>() : 
                                JsonConvert.DeserializeObject<List<AttributeDto>>(v.Attributes) ?? new List<AttributeDto>(),
                            SalePrice = v.SalePrice,
                            CompareAtPrice = v.CompareAtPrice,
                            AdditionalHightlights = string.IsNullOrEmpty(v.AdditionalHightlights) ? 
                                new List<HighlightDto>() : 
                                JsonConvert.DeserializeObject<List<HighlightDto>>(v.AdditionalHightlights) ?? new List<HighlightDto>(),
                            MaximumOrderQuantity = v.MaximumOrderQuantity,
                            MinimumOrderQuantity = v.MinimumOrderQuantity,
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
            
            if (product == null)
            {
                throw new Exception($"Product not found or not verified: {productId}");
            }

            if (product.Variants == null || !product.Variants.Any())
            {
                throw new Exception($"No available variants found for product {productId}");
            }

            return product;
        }

        public async Task<int> VerifyProduct(VerifyProductDto verifyProductDto, string UserName, int AdminPersonID, string PageSource)
        {
            // Check if all catalogs exist and are active
            var allProductsExist = verifyProductDto.ListOfProductIDs.All(id => _context.Products.Any(c => c.ID == id && c.IsActive));
            if (!allProductsExist)
            {
                throw new Exception("One or more catalogs do not exist or are deleted.");
            }

            // Check if all catalogs are in not verified state
            var allProductsNotVerifiedOrPending = verifyProductDto.ListOfProductIDs.All(id => !_context.Products.Any(c => c.ID == id && (c.StatusId == _context.StatusOptionSets.Where(s => s.Status == StatusConstants.VERIFIED).FirstOrDefault().Id || c.StatusId == _context.StatusOptionSets.Where(s => s.Status == StatusConstants.PENDING).FirstOrDefault().Id)));
            if (!allProductsNotVerifiedOrPending)
            {
                throw new Exception("One or more catalogs are already in the 'Verified' or 'Pending' state.");
            }

            //Check if any are already in send to verification state
            var anyProductInSendToVerification = verifyProductDto.ListOfProductIDs.Any(id => _context.Products.Any(c => c.ID == id && c.StatusId == _context.StatusOptionSets.Where(s => s.Status == StatusConstants.SENDTOVERIFICATION).FirstOrDefault().Id));
            if (anyProductInSendToVerification)
            {
                throw new Exception("One or more catalogs are already in the 'Send To Verification' state.");
            }

            //Update all catalogs status to send for verification state
            foreach(var id in verifyProductDto.ListOfProductIDs)
            {
                var catalog = await _context.Products.FindAsync(id);
                catalog.StatusId = _context.StatusOptionSets.FirstOrDefault(s => s.Status == StatusConstants.SENDTOVERIFICATION).Id;
                catalog.ModifiedBy = UserName;
                catalog.ModifiedDate = DateTime.Now;
                _context.Products.Update(catalog);
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
                TaskData = string.Join(",", verifyProductDto.ListOfProductIDs),
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
            var catalog = await _context.Products.FindAsync(id);
			if (catalog != null)
			{
				catalog.IsActive = false;
				_context.Products.Update(catalog);
                _context.ProductVariants.Where(v => v.ProductID == id).ToList().ForEach(v => v.IsActive = false);
				await unitOfWork.Complete();
			}
			else
			{
				throw new Exception("Product Not Found");
			}
        }

        public async Task<int> QuickActions(QuickActionsParams quickActionsParams, string UserName)
        {
            var catalog = await _context.Products.Where(c=> c.ID == quickActionsParams.ProductId && c.IsActive).FirstOrDefaultAsync();
			if (catalog != null)
			{
				//catalog.InStock= quickActionsParams.InStock == null ? catalog.InStock : quickActionsParams.InStock.Value;
                catalog.IsRecommended = quickActionsParams.IsRecommended == null ? catalog.IsRecommended : quickActionsParams.IsRecommended.Value;
                catalog.ModifiedDate = DateTime.Now;
                catalog.ModifiedBy = UserName;
                _context.Products.Update(catalog);
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
            var isValidProduct = await _context.ProductVariants
                .Include(cv => cv.Product)
                .Where(cv => cv.ID == productAdvanced.ProductVariantId)
                .AnyAsync(cv => 
                    cv.Product.BusinessID == _context.Outlets
                        .Where(o => o.ID == productAdvanced.BranchId)
                        .Select(o => o.BussinessID)
                        .FirstOrDefault() &&
                    cv.IsActive && 
                    cv.Product.IsActive);

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
        //     var product = await _context.Products
        //         .FirstOrDefaultAsync(c => c.ID == productId && c.IsActive);
            
        //     if (product == null)
        //         throw new Exception("Product not found");

        //     var existingVariant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.ProductID == productId && v.SKU == variantDto.SKU);
        //     if (existingVariant != null)
        //         throw new Exception("Variant already exists");

        //     var variant = new ProductVariants
        //     {
        //         ProductID = productId,
        //         SKU = variantDto.SKU,
        //         SalePrice = variantDto.SalePrice,
        //         CostPrice = variantDto.CostPrice,
        //         AdditionalHightlights = variantDto.AdditionalHightlights,
        //         CreateBy = userName,
        //         CreateDate = DateTime.Now,
        //         IsActive = true
        //     };

        //     await _context.ProductVariants.AddAsync(variant);
        //     await unitOfWork.Complete();
            
        //     return variant.ID;
        // }
        public async Task<List<SingleProductVariantsResult>> GetProductVariants(int productId)
        {
            var variants = await _context.ProductVariants
                .Where(v => v.ProductID == productId && v.IsActive)
                .Select(v => new SingleProductVariantsResult
                {
                    VariantID = v.ID,
                    ProductID = v.ProductID,
                    Attributes = JsonConvert.DeserializeObject<List<AttributeDto>>(v.Attributes),
                    SKU = v.SKU,
                    SalePrice = v.SalePrice,
                    CostPrice = v.CostPrice,
                    AdditionalHightlights = JsonConvert.DeserializeObject<List<HighlightDto>>(v.AdditionalHightlights)
                })
                .ToListAsync();

            return variants;
        }

        // public async Task<ProductVariantResponse> GetProductVariant(int variantId)
        // {
        //     var variant = await _context.ProductVariants
        //         .Where(v => v.ID == variantId && v.IsActive)
        //         .Select(v => new ProductVariantResponse
        //         {
        //             VariantID = v.ID,
        //             ProductID = v.ProductID,
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
        //     var variant = await _context.ProductVariants
        //         .FirstOrDefaultAsync(v => v.ID == variantId && v.IsActive);

        //     if (variant == null)
        //         throw new Exception("Product variant not found");

        //     variant.SKU = variantDto.SKU;
        //     variant.SalePrice = variantDto.SalePrice;
        //     variant.CostPrice = variantDto.CostPrice;
        //     variant.AdditionalHightlights = variantDto.AdditionalHightlights;
        //     variant.ModifiedBy = userName;
        //     variant.ModifiedDate = DateTime.Now;

        //     _context.ProductVariants.Update(variant);
        //     await unitOfWork.Complete();
        // }

        public async Task<ProductDetailsV2Result> GetProductDetailsV2(int productId)
        {
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
            var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);

            var product = await _context.Products
                .Include(c => c.ProductVariants)
                .Include(c => c.Category)
                .Where(c => c.ID == productId && c.IsActive == true)
                .Select(c => new ProductDetailsV2Result
                {
                    ProductId = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    Category = new CategoryDto { CategoryId = c.Category.ID, CategoryName = c.Category.Name },
                    RequiresRefrigeration = c.RequiresRefrigeration,
                    IsRecommended = c.IsRecommended,
                    VariantOptions = string.IsNullOrEmpty(c.VariantOptions) 
                        ? new List<VarientOptionDto>() 
                        : JsonConvert.DeserializeObject<List<VarientOptionDto>>(c.VariantOptions) ?? new List<VarientOptionDto>(),
                    BaseHightlights = string.IsNullOrEmpty(c.BaseHightlights) 
                        ? new List<HighlightDto>() 
                        : JsonConvert.DeserializeObject<List<HighlightDto>>(c.BaseHightlights) ?? new List<HighlightDto>(),
                    Variants = c.ProductVariants.Where(v => v.IsActive).Select(v => new ProductVariantV2Result
                    {
                        VariantId = v.ID,
                        Attributes = string.IsNullOrEmpty(v.Attributes) 
                            ? new List<AttributeDto>() 
                            : JsonConvert.DeserializeObject<List<AttributeDto>>(v.Attributes) ?? new List<AttributeDto>(),
                        SKU = v.SKU,
                        SalePrice = v.SalePrice,
                        CompareAtPrice = v.CompareAtPrice,
                        AdditionalHightlights = string.IsNullOrEmpty(v.AdditionalHightlights) 
                            ? new List<HighlightDto>() 
                            : JsonConvert.DeserializeObject<List<HighlightDto>>(v.AdditionalHightlights) ?? new List<HighlightDto>(),
                        MaximumOrderQuantity = v.MaximumOrderQuantity,
                        MinimumOrderQuantity = v.MinimumOrderQuantity,
                        PackedHeight = v.PackedHeight,
                        PackedWidth = v.PackedWidth,
                        PackedDepth = v.PackedDepth,
                        WeightUnit = v.WeightUnit,
                        Weight = v.Weight,
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
                                .FirstOrDefault(),
                        IsDefaultVariant = v.IsDefaultVariant
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (product != null)
            {
                // Get similar products from the same category
                var categoryId = product.Category.CategoryId;
                var similarProducts = await _context.Products
                    .Where(c => c.CategoryID == categoryId && c.ID != productId && c.IsActive == true)
                    .OrderBy(c => Guid.NewGuid()) // Random ordering
                    .Take(5) // Limit to 5 similar products
                    .Select(c => new SimilarProductResult
                    {
                        ProductId = c.ID,
                        Name = c.Name,
                        Description = c.Description,
                        SalePrice = c.ProductVariants
                            .Where(v => v.IsActive && v.IsDefaultVariant)
                            .Select(v => v.SalePrice)
                            .FirstOrDefault(),
                        Thumbnail = (from attachment in _context.Attachments
                                    join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                    join entity in _context.Entity on link.EntityID equals entity.ID
                                    join variant in c.ProductVariants on link.RecordID equals variant.ID
                                    where entity.Name == EntityConstants.CatelogVariant && 
                                        variant.IsActive &&
                                        variant.IsDefaultVariant &&
                                        link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                    select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}")
                                    .FirstOrDefault()
                    })
                    .ToListAsync();

                product.SimilarProducts = similarProducts;
            }

            return product;
        }

    }


}
