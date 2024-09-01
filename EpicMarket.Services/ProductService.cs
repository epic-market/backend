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
        {
            var product = mapper.Map<Catalog>(productsDto);
            product.BusinessID = businessID;
            product.CreateBy = UserName;
            product.CreateDate = DateTime.Now;
            var status = await _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == Business_Status.BUSINESS_UNVERIFIED);
		    product.StatusId = status.Id;
            var events = EventConstants.AddCatelog;
            var mailevent = MessageDataConstants.AddCatelog;
            await _context.Catalogs.AddAsync(product);
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

			var product = await _context.Catalogs.FirstOrDefaultAsync(c => c.ID == id);

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
            var returnedProducts = await (from catalogItem in _context.Catalogs
                    join outletProduct in (_context.OutletProducts.Where(a => a.OutletID == BranchId))
                    on catalogItem.ID equals outletProduct.ProductID into joinedProducts
                    from matchedProduct in joinedProducts.DefaultIfEmpty()
                    where catalogItem.BusinessID == BusinessID
                    select new ProductsMapOptionResult
                    {
                        Id = catalogItem.ID,
                        Name = catalogItem.Name,
                        Description = catalogItem.Description,
                        Rate = catalogItem.Rate,
                        Selected = matchedProduct == null ? false : true,
                    }).ToListAsync();

            return returnedProducts;
        }

        public async Task<GetDataResult<List<ProductResult>>> GetAllProducts(ProductParams productParams, int businessID)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

			var getResult = new GetDataResult<List<ProductResult>>();


            //1 . filter with BusinessID
            var Products = _context.Catalogs
                                .Where(c => c.BusinessID == businessID && c.IsActive == true);


            //2 . Appling Searching
            var sortedProducts = Products.Where(row => row.Name.Contains(productParams.searchTerm.Trim()) || row.Description.Contains(productParams.searchTerm.Trim()));


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
                InStock = c.InStock,
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
				InStock = c.InStock,
				Category = c.Category,
                IsActive = c.IsActive,
                Barcode = c.Barcode,
				Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == c.StatusId).Status,
				Images = attachments.Select(a => a.ImagePath).ToList(),
				Thumbnail = thumbnail.Select(a => a.ImagePath).FirstOrDefault(),
				MaximumOrderPurchase = (int)c.MaximumOrderPurchase,
				IsRecommended = c.IsRecommended
			}).FirstOrDefaultAsync();

		}
        public async Task<int> VerifyCatalog(VerifyDto verifyBranchDto, string UserName, int AdminPersonID, string PageSource)
        {
            var newTaskStatus = await _context.TaskStatusTypes.Where(row => row.Status == "New").FirstOrDefaultAsync();
            var taskTypeID = await _context.TaskTypes.Where(row => row.Name == "Verification").FirstOrDefaultAsync();
            var userName = await _context.Users.Where(row => row.UserName == UserName).FirstOrDefaultAsync();
            var taskToSave = new Tasks
            {
                Name = VerificationConstants.CatelogName,
                Description = VerificationConstants.CatelogDescription,
                TaskTypeID = taskTypeID.ID,
                ParentID = null,
                TaskStatusID = newTaskStatus.Id,
                TaskPriorityID = 1,
                PrimaryAssignedToPersonID = AdminPersonID,
                DateAssigned = DateTime.Now,
                SubmittedByPersonID = userName.Id,
                TaskData = string.Join(",", verifyBranchDto.ListOfProductIDs),
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
    }


}
