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
		private readonly IUnitOfWork unitOfWork;

		public ProductService(
                                ApplicationDbContext context,
                                IMapper mapper, 
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                ICommunicationQueueService communicationQueueService,
                                IApplicationConfigurationService applicationConfigurationService,
                                IFileService fileService,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
            this.communicationQueueService = communicationQueueService;
			this.applicationConfigurationService = applicationConfigurationService;
			this.fileService = fileService;
			this.unitOfWork = unitOfWork;
		}

        public async Task<int> AddProduct(AddProductsDto productsDto, string UserName, int businessID, string PageSource)
        {  //TODO: Add Product Variant

          // First deserialize the string to get the JSON string
            string jsonString = JsonConvert.DeserializeObject<string>(productsDto.Variants);
            // Then deserialize the JSON string to get the array of variants
            List<ProductVariantDto> variants = JsonConvert.DeserializeObject<List<ProductVariantDto>>(jsonString);

            if(variants == null || variants.Count == 0)
            {
              throw new Exception("Variants are required");
            }

            var product = mapper.Map<Catalog>(productsDto);
            product.BusinessID = businessID;
            product.CreateBy = UserName;
            product.CreateDate = DateTime.Now;
            var status = await _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == StatusConstants.UNVERIFIED);
		    product.StatusId = status.Id;
            var events = EventConstants.AddCatelog;
            var mailevent = MessageDataConstants.AddCatelog;
            await _context.Catalogs.AddAsync(product);
            await unitOfWork.Complete();

            var newVariants = variants.Select(v => new ProductVariants 
                {   
                ProductID = product.ID,
                SKU = v.SKU,
                Price = v.Price,
                Attributes = JsonConvert.SerializeObject(v.Attributes),
                CreateBy = UserName,
                CreateDate = DateTime.Now
            }).ToList();

            await _context.ProductVariants.AddRangeAsync(newVariants);
            await unitOfWork.Complete();
          
            
			var saved = await _context.Catalogs.FirstOrDefaultAsync(o => o.ID == product.ID);
            string savedJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = product.ID, Data = savedJson, Description = null, EventName = events, EntityName = EntityConstants.Catelog,Source=PageSource });
			await this.communicationQueueService.InsertCommunicationQueue(
                    new Entities.CommunicationQueueDTO()
                    {
                        MessageData = null,//TODO
                        Subject = mailevent,
                        NotificationRecipient = UserName,
                        ContactMethod = ContactMethodConstants.EMAIL,
                        CreateBy = UserName
                    });
            return product.ID;
        }

		public async Task<int> UpdateProducts(AddProductsDto productsDto,int id, string UserName, int businessID, string PageSource)
		{

			var product = await _context.Catalogs.FirstOrDefaultAsync(c => c.ID == id && c.IsActive);

			if (product == null)
			{
				throw new Exception("Product not found.");
			}
			mapper.Map(productsDto, product);
			product.ModifiedBy = UserName;
			product.ModifiedDate = DateTime.Now;
			_context.Entry(product).State = EntityState.Modified;
			await unitOfWork.Complete();
			var events  =   EventConstants.EditCatelog;
			var mailevent = MessageDataConstants.EditCatelog;

			var saved = await _context.Catalogs.FirstOrDefaultAsync(o => o.ID == product.ID);
			string savedJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
			await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = product.ID, Data = savedJson, Description = null, EventName = events, EntityName = EntityConstants.Catelog, Source = PageSource });
			await this.communicationQueueService.InsertCommunicationQueue(
					new Entities.CommunicationQueueDTO()
					{
						MessageData = null,//TODO
						Subject = mailevent,
						NotificationRecipient = UserName,
						ContactMethod = ContactMethodConstants.EMAIL,
						CreateBy = UserName
					});
			return product.ID;
		}


		public async Task<List<ProductsMapOptionResult>> GetAllProductForMap(int BusinessID,int BranchId)
        {

            var attachmentTypeID = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

            var returnedProducts = await (from catalogItem in _context.Catalogs
                                    where catalogItem.BusinessID == BusinessID && catalogItem.IsActive == true
                                    select new ProductsMapOptionResult
                                    {
                                        ProductId = catalogItem.ID,
                                        Name = catalogItem.Name,
                                        Description = catalogItem.Description,
                                        Thumbnail = (from attachment in _context.Attachments
                                                    join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                                    join entity in _context.Entity on link.EntityID equals entity.ID
                                                    where entity.Name == EntityConstants.Catelog && 
                                                        link.RecordID == catalogItem.ID && 
                                                        link.AttachmentTypeID == attachmentTypeID.ID
                                                    select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                                ).FirstOrDefault(),
                                        Variants = (from v in _context.ProductVariants
                                                where v.ProductID == catalogItem.ID
                                                select new VariantResultTemp
                                                {
                                                    VariantId = v.VariantID,
                                                    SKU = v.SKU,
                                                    Price = v.Price,
                                                    Attributes = v.Attributes,
                                                    Selected = _context.Inventory
                                                        .Where(i => i.OutletID == BranchId)
                                                        .Any(i => i.ProductVariantID == v.VariantID)
                                                })
                                                .AsEnumerable()
                                                .Select(v => new VariantResult
                                                {
                                                    VariantId = v.VariantId,
                                                    SKU = v.SKU,
                                                    Price = v.Price,
                                                    Selected = v.Selected,
                                                    Attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(v.Attributes ?? "{}")
                                                })
                                                .ToList()
                                    }).ToListAsync();

            return returnedProducts;
                        
        }




        public async Task<GetDataResult<List<ProductResult>>> GetAllProductsForPOS(ProductPOSParams productParams, int outletId)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

            var getResult = new GetDataResult<List<ProductResult>>();

            //1 . filter with BusinessID
            var Products = _context.Inventory
                                .Where(c => c.OutletID == outletId)
                                .Include(c => c.ProductVariants)
                                .Where(c => c.ProductVariants.Catalog.IsActive == true)
                                .IgnoreQueryFilters();

            //2 . Appling Searching
            var sortedProducts = Products;
            if (!string.IsNullOrEmpty(productParams.searchTerm))
            {
                var searchTerm = productParams.searchTerm.Trim();
                sortedProducts = Products.Where(row => 
                    row.ProductVariants.Catalog.Name.Contains(searchTerm) || 
                    row.ProductVariants.Catalog.Description.Contains(searchTerm));
            }

            int totalCount = await sortedProducts.CountAsync();

            // 4. Apply pagination (skip and take)
            var pagedProducts = sortedProducts
                .Skip((productParams.PageIndex - 1) * productParams.pageSize)
                .Take(productParams.pageSize);

            // 5. Select data and add SRNO
            var results = await pagedProducts
                .Select(c => new ProductResult()
                {
                    ProductId = c.ProductVariants.Catalog.ID,
                    Name = c.ProductVariants.Catalog.Name,
                    Rate = c.ProductVariants.Catalog.Rate,
                    Status = c.ProductVariants.Catalog.StatusOptionSets.Status,
                    CostPrice = c.ProductVariants.Catalog.CostPrice,
                    Count = c.QuantityAvailable,
                    Thumbnail = _context.Attachments
                        .Join(_context.AttachmentLinks,
                            a => a.ID,
                            l => l.AttachmentID,
                            (a, l) => new { a, l })
                        .Join(_context.Entity,
                            al => al.l.EntityID,
                            e => e.ID,
                            (al, e) => new { al.a, al.l, e })
                        .Where(x => 
                            x.e.Name == EntityConstants.Catelog && 
                            x.l.RecordID == c.ProductVariants.Catalog.ID && 
                            x.l.AttachmentTypeID == attachmentTypeID.ID)
                        .Select(x => $"{x.a.DocumentFolderPath}{x.a.DocumentFile}")
                        .FirstOrDefault()
                }).ToListAsync();

            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

        public async Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productParams, int businessID)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

			var getResult = new GetDataResult<List<ProductResult>>();


            //1 . filter with BusinessID
            var Products = _context.Catalogs
                                .Where(c => c.BusinessID == businessID && c.IsActive == true);


            //2 . Appling Searching
            var sortedProducts = Products;
            if (!String.IsNullOrEmpty(productParams.searchTerm))
            {
                sortedProducts = Products.Where(row => row.Name.Contains(productParams.searchTerm.Trim()) || row.Description.Contains(productParams.searchTerm.Trim()));
            }

            // 3 .Appying Sorting
            switch (productParams.sortColumn)
            {
                case "ProductID":
                    sortedProducts = productParams.ascending ? sortedProducts.OrderBy(c => c.ID) : sortedProducts.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    sortedProducts = productParams.ascending ? sortedProducts.OrderBy(c => c.Name) : sortedProducts.OrderByDescending(c => c.Name);
                    break;
                default:
                    break;
            }

            //getting the total count
            int totalCount = sortedProducts.Count();


            // 4. Apply pagination (skip and take)
            var pagedProducts = sortedProducts
                .Skip((productParams.PageIndex - 1) * productParams.pageSize) // Skip items for previous pages
                .Take(productParams.pageSize); // Take items for the current page

            // 5. Select data and add SRNO
            var results = await pagedProducts.Select(c => new ProductResult()
            {
                ProductId = c.ID,
                Name = c.Name,
                Description = c.Description,
                Rate = c.Rate,
                CostPrice = c.CostPrice,
                Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == c.StatusId).Status,
                Thumbnail = ((from attachment in _context.Attachments
                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                              join entity in _context.Entity on link.EntityID equals entity.ID
                              where entity.Name == EntityConstants.Catelog && link.RecordID == c.ID && link.AttachmentTypeID == attachmentTypeID.ID
                              select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}").FirstOrDefault()),
                Count = totalCount,
            }).ToListAsync();

            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

        public async Task<ProductsDto> GetProductDetails(int productId)
		{

			var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
			var attachmentTypeID_Product = await  _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);



			var attachments = from attachment in _context.Attachments
							  join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
							  join entity in _context.Entity on link.EntityID equals entity.ID
							  where entity.Name == EntityConstants.Catelog && link.RecordID == productId && link.AttachmentTypeID == attachmentTypeID_Product.ID
							  select new
							  {
								  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
							  };

            var thumbnail =   from attachment in _context.Attachments
							  join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
							  join entity in _context.Entity on link.EntityID equals entity.ID
							  where entity.Name == EntityConstants.Catelog && link.RecordID == productId && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                              orderby attachment.CreateDate descending
						        select new
							  {
								  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
							  };


			return await _context.Catalogs.Where(c => c.ID == productId && c.IsActive == true).Select(c =>  
			new ProductsDto
			{
				Id = c.ID,
				Name = c.Name,
				Description = c.Description,
				Rate = c.Rate,
				RequiresRefrigeration= c.RequiresRefrigeration,
                PackedDepth = c.PackedDepth,
                PackedHeight = c.PackedHeight,
                PackedWidhth = c.PackedWidhth,
                CostPrice = c.CostPrice,
                Weight = c.Weight,
                Category = c.Category,
                Barcode = c.Barcode,
				Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == c.StatusId).Status,
				Images = attachments.Select(a => a.ImagePath).ToList(),
				Thumbnail = thumbnail.Select(a => a.ImagePath).FirstOrDefault(),
				MaximumOrderPurchase = (int)c.MaximumOrderPurchase,
				IsRecommended = c.IsRecommended
			}).FirstOrDefaultAsync();

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
                TaskStatusID = newTaskStatus.Id,
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

		


        public async Task deleteCatelog(int id, string UserName)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
			if (catalog != null)
			{
				catalog.IsActive = false;
				_context.Catalogs.Update(catalog);
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

        public async Task AddOrUpdateProductInventoryDetails(ProductAdvanced productAdvanced)
        {

           var outletProductInventory =  _context.Inventory.FirstOrDefault(c=> c.ProductVariantID == productAdvanced.ProductVariantId && c.OutletID == productAdvanced.BranchId);

            if (outletProductInventory == null){
                outletProductInventory = new Inventory();
                outletProductInventory.BackOrders = productAdvanced.BackOrders;
                outletProductInventory.MaximumStockLevel = productAdvanced.MaximumStockLevel;
                outletProductInventory.MinimumStockLevel = productAdvanced.MinimumStockLevel;
                outletProductInventory.QuantityAvailable = productAdvanced.QuantityAvailable;
                outletProductInventory.ReorderPoint = productAdvanced.ReorderPoint;
                outletProductInventory.ProductVariantID = productAdvanced.ProductVariantId;
                outletProductInventory.OutletID = productAdvanced.BranchId;
                await _context.Inventory.AddAsync(outletProductInventory);
                
            }else{
                outletProductInventory.BackOrders = productAdvanced.BackOrders;
                outletProductInventory.MaximumStockLevel = productAdvanced.MaximumStockLevel;
                outletProductInventory.MinimumStockLevel = productAdvanced.MinimumStockLevel;
                outletProductInventory.QuantityAvailable = productAdvanced.QuantityAvailable;
                outletProductInventory.ReorderPoint = productAdvanced.ReorderPoint;
                _context.Inventory.Update(outletProductInventory);
            }
            await unitOfWork.Complete();
        }

        public async Task<ProductAdvanced> GetProductInventoryDetails(int productVariantId, int branchId)
        {
           var productAdvanced = await _context.Inventory.Where(c => c.OutletID == branchId && c.ProductVariantID == productVariantId).Select(c=> new ProductAdvanced {
              BackOrders = c.BackOrders,
              MaximumStockLevel = c.MaximumStockLevel,  
              MinimumStockLevel = c.MinimumStockLevel,
              QuantityAvailable = c.QuantityAvailable,
              ReorderPoint = c.ReorderPoint,    
              ProductVariantId = c.ProductVariantID,
              BranchId = branchId
            }).FirstOrDefaultAsync();

            return productAdvanced;
        }

        public async Task<GetDataResult<List<CustomerResultBaseOnCatefory>>> GetAllProductsForMobile(ProductMobileParams parameters)
        {  

                var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

                var VerifiedStatusID = _context.StatusOptionSets.FirstOrDefault(c => c.Status == StatusConstants.VERIFIED).Id;
                var query = _context.Inventory
                    .IgnoreQueryFilters()
                    .Include(p => p.Outlet)
                    .Include(p => p.ProductVariants)
                    .Where(p => p.Outlet.ID == parameters.OutletId && p.ProductVariants.Catalog.StatusId == VerifiedStatusID)
                    .AsQueryable();

                // Apply Category Filter
                if (!string.IsNullOrEmpty(parameters.Category))
                {
                    query = query.Where(p => p.ProductVariants.Catalog.Category == parameters.Category);
                }

                // Apply Search
                if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
                {
                    var searchTerm = parameters.SearchTerm.Trim().ToLower();
                    query = query.Where(p =>
                        p.ProductVariants.Catalog.Name.ToLower().Contains(searchTerm) ||
                        p.ProductVariants.Catalog.Description.ToLower().Contains(searchTerm));
                }

                // Get total count for pagination
                var totalItems = await query.CountAsync();

                // Check if there are any products for the outlet
                if (totalItems == 0)
                {
                    throw new Exception("No products found for the outlet");
                }

            var recommendedProducts = await query
                                    .Where(p => p.ProductVariants.Catalog.IsRecommended)
                                    .Select(p => new CustomerProductResult
                                    {
                                        ProductId = p.ProductVariants.Catalog.ID,
                                        Name = p.ProductVariants.Catalog.Name,
                                        Description = p.ProductVariants.Catalog.Description,
                                        Rate = p.ProductVariants.Catalog.CostPrice,
                                        Thumbnail = ((from attachment in _context.Attachments
                                                      join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                                      join entity in _context.Entity on link.EntityID equals entity.ID
                                                      where entity.Name == EntityConstants.Catelog && link.RecordID == p.ProductVariants.Catalog.ID && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                                      select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}").FirstOrDefault()),
                                        Rating = p.ProductVariants.Catalog.Rating,
                                        RatingCount = p.ProductVariants.Catalog.ReviewCount
                                    })
                                    .ToListAsync();

            var categoryProducts = await query
                    .Skip((parameters.Page - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .GroupBy(p => p.ProductVariants.Catalog.Category)
                    .Select(g => new CustomerResultBaseOnCatefory
                    {
                        Category = g.Key,
                        CustomerProductResults = g.Select(p => new CustomerProductResult
                        {
                            ProductId = p.ProductVariants.Catalog.ID,
                            Name = p.ProductVariants.Catalog.Name,
                            Description = p.ProductVariants.Catalog.Description,
                            Rate = p.ProductVariants.Catalog.CostPrice,
                            Thumbnail = ((from attachment in _context.Attachments
                                          join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                          join entity in _context.Entity on link.EntityID equals entity.ID
                                          where entity.Name == EntityConstants.Catelog && link.RecordID == p.ProductVariants.Catalog.ID && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                          select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}").FirstOrDefault()),
                            Rating = p.ProductVariants.Catalog.Rating,
                            RatingCount = p.ProductVariants.Catalog.ReviewCount
                        }).ToList()
                    })
                    .ToListAsync();

            // Apply Sorting
            var sortedCategoryProducts = categoryProducts.OrderBy(x => x.Category).ToList();

            var finalResults = new List<CustomerResultBaseOnCatefory>();


            if (recommendedProducts.Any())
            {
                finalResults.Add(new CustomerResultBaseOnCatefory
                {
                    Category = "Recommended",
                    CustomerProductResults = recommendedProducts
                });
            }
            finalResults.AddRange(sortedCategoryProducts);


            return new GetDataResult<List<CustomerResultBaseOnCatefory>>
                {
                   items = finalResults,
                   Count = totalItems
                };
        }
        private IQueryable<T> ApplySorting<T>(
            IQueryable<T> query,
            string sortBy,
            string  sortOrder) where T : class
        {
            return (sortBy?.ToLower(), sortOrder?.ToLower()) switch
            {
                ("name", "ascending") =>
                    query.OrderBy(p => EF.Property<string>(p, "Name")),

                ("name", "descending") =>
                    query.OrderByDescending(p => EF.Property<string>(p, "Name")),

                ("price", "ascending") =>
                    query.OrderBy(p => EF.Property<decimal>(p, "Price")),

                ("price", "descending") =>
                    query.OrderByDescending(p => EF.Property<decimal>(p, "Price")),

                ("rating", "ascending") =>
                    query.OrderBy(p => EF.Property<double>(p, "AverageRating")),

                ("rating", "descending") =>
                    query.OrderByDescending(p => EF.Property<double>(p, "AverageRating")),

                ("newest", "ascending") =>
                    query.OrderBy(p => EF.Property<DateTime>(p, "CreatedAt")),

                ("newest", "descending") =>
                    query.OrderByDescending(p => EF.Property<DateTime>(p, "CreatedAt")),

                ("popular", "ascending") =>
                    query.OrderBy(p => EF.Property<int>(p, "OrderCount")),

                ("popular", "descending") =>
                    query.OrderByDescending(p => EF.Property<int>(p, "OrderCount")),

                // Default sorting by name ascending
                _ => query.OrderBy(p => EF.Property<string>(p, "Name"))
            };
        }

        public async Task<int> AddProductVariant(int productId, ProductVariantDto variantDto, string userName)
        {
            var product = await _context.Catalogs
                .FirstOrDefaultAsync(c => c.ID == productId && c.IsActive);
            
            if (product == null)
                throw new Exception("Product not found");

            var existingVariant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.ProductID == productId && v.SKU == variantDto.SKU);
            if (existingVariant != null)
                throw new Exception("Variant already exists");

            var variant = new ProductVariants
            {
                ProductID = productId,
                SKU = variantDto.SKU,
                Price = variantDto.Price,
                Attributes = JsonConvert.SerializeObject(variantDto.Attributes),
                CreateBy = userName,
                CreateDate = DateTime.Now,
                IsActive = true
            };

            await _context.ProductVariants.AddAsync(variant);
            await unitOfWork.Complete();
            
            return variant.VariantID;
        }
        public async Task<List<ProductVariantResponse>> GetProductVariants(int productId)
        {
            var variants = await _context.ProductVariants
                .Where(v => v.ProductID == productId && v.IsActive)
                .Select(v => new ProductVariantResponse
                {
                    VariantID = v.VariantID,
                    ProductID = v.ProductID,
                    SKU = v.SKU,
                    Price = v.Price,
                    Attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(v.Attributes)
                })
                .ToListAsync();

            return variants;
        }

        public async Task<ProductVariantResponse> GetProductVariant(int variantId)
        {
            var variant = await _context.ProductVariants
                .Where(v => v.VariantID == variantId && v.IsActive)
                .Select(v => new ProductVariantResponse
                {
                    VariantID = v.VariantID,
                    ProductID = v.ProductID,
                    SKU = v.SKU,
                    Price = v.Price,
                    Attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(v.Attributes)
                })
                .FirstOrDefaultAsync();

            if (variant == null)
                throw new Exception("Product variant not found");

            return variant;
        }

        public async Task UpdateProductVariant(int variantId, ProductVariantDto variantDto, string userName)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.VariantID == variantId && v.IsActive);

            if (variant == null)
                throw new Exception("Product variant not found");

            variant.SKU = variantDto.SKU;
            variant.Price = variantDto.Price;
            variant.Attributes = JsonConvert.SerializeObject(variantDto.Attributes);
            variant.ModifiedBy = userName;
            variant.ModifiedDate = DateTime.Now;

            _context.ProductVariants.Update(variant);
            await unitOfWork.Complete();
        }

    }


}
