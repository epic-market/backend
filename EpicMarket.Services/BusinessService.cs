using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        private readonly ICommunicationQueueService communicationQueueService;
        private readonly ITasksService tasksService;
        private readonly IUnitOfWork unitOfWork;
		public BusinessService(
                                ApplicationDbContext context,
                                IMapper mapper ,
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                ICommunicationQueueService communicationQueueService,
                                ITasksService tasksService,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
            this.communicationQueueService = communicationQueueService;
            this.tasksService = tasksService;
            this.unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Registers a new business in the system.
        /// </summary>
        /// <param name="businessRegisterDto">The business registration details.</param>
        /// <param name="UserName">The username of the user registering the business.</param>
        /// <param name="AdminPersonId">The ID of the admin person.</param>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="PageSource">The source of the page.</param>
        /// <returns>The ID of the newly registered business.</returns>
        public async Task<BusinessDTO_Result> RegisterBusiness(BusinessRegisterDto businessRegisterDto, string UserName, int AdminPersonId, int userid, string PageSource)
        {
            // Check for null or missing required fields in businessRegisterDto
            if (businessRegisterDto == null || string.IsNullOrEmpty(businessRegisterDto.BusinessName) || string.IsNullOrEmpty(businessRegisterDto.Address) || string.IsNullOrEmpty(businessRegisterDto.State) || string.IsNullOrEmpty(businessRegisterDto.City) || businessRegisterDto.PinCode == 0 || businessRegisterDto.Latitude == 0 || businessRegisterDto.Longitude == 0)
            {
                throw new ArgumentNullException(nameof(businessRegisterDto), "Business registration details are missing or invalid.");
            }

            var statusid = await _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == StatusConstants.UNVERIFIED);
            if (statusid == null)
            {
                throw new InvalidOperationException("Business status option set is missing.");
            }

            var addressModel = new AddressDto();
            addressModel.Address1 = businessRegisterDto.Address;
            addressModel.City = businessRegisterDto.City;
            addressModel.State = businessRegisterDto.State;
            addressModel.Pincode = businessRegisterDto.PinCode;
            addressModel.Latitude = businessRegisterDto.Latitude;
            addressModel.Longitude = businessRegisterDto.Longitude;
            addressModel.CreateBy = UserName;
            addressModel.CreateDate = DateTime.Now;
            int addressId = await addressService.AddUpdateAddress(addressModel);

            var businessModel = new Business();
            businessModel.AddressID = addressId;
            businessModel.PersonID = userid;
            businessModel.BusinessCategoryID = businessRegisterDto.BusinessCategoryID;
            businessModel.Name = businessRegisterDto.BusinessName;
            businessModel.Description = businessRegisterDto.Description;
            businessModel.ContactNumber = businessRegisterDto.ContactNumber;
            businessModel.ContactEmail = businessRegisterDto.ContactEmail;
            businessModel.CreateBy = UserName;
            businessModel.CreateDate = DateTime.Now;
            businessModel.StatusId = statusid.Id;

            //var proofEntity = new Proof
            //{
            //    EntityType = EntityConstants.Business,
            //    EntityId = businessModel.ID,
            //    ProofTypeId = businessRegisterDto.ProofTypeId,
            //    ProofNumber = businessRegisterDto.ProofNumber
            //};

            //await _context.Proofs.AddAsync(proofEntity);
            await _context.Businesses.AddAsync(businessModel);
            await _context.SaveChangesAsync();

            var BusinssID = new List<int>() { businessModel.ID };

            var taskID = await tasksService.SaveTask(new TasksParams()
            {
                TaskPriorityID = 5,
                TaskEntity = EntityConstants.Business,
                TaskData = string.Join(",", BusinssID),
                Name = EntityConstants.Business,
                Description = TaskDescriptions.Business,
                TaskType = TaskTypeConstants.Verification
            }, AdminPersonId, UserName);

            string savedJson = JsonConvert.SerializeObject(businessModel, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = businessModel.ID, Data = savedJson, Description = null, EventName = EventConstants.AddBusiness, EntityName = EntityConstants.Business, Source = PageSource });


            return new BusinessDTO_Result(){BusinessId = businessModel.ID /*, ProofId = proofEntity.Id*/ };
        }


        public async Task<BusinessDetailResult> GetBusinessByID(int businessId)
        {

            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.LOGO);
            var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PROOF);

            var attachments = from attachment in _context.Attachments
                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                              join entity in _context.Entity on link.EntityID equals entity.ID
                              where entity.Name == EntityConstants.Business && link.RecordID == businessId && link.AttachmentTypeID == attachmentTypeID_Product.ID
                              select new
                              {
                                  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                              };

            var thumbnail = from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            where entity.Name == EntityConstants.Business && link.RecordID == businessId && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                            orderby attachment.CreateDate descending
                            select new
                            {
                                ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                            };

            return await _context.Businesses.Where(o => o.ID == businessId && o.IsActive == true).Include(o => o.Address).Select(o => new BusinessDetailResult()
            {
                ID = o.ID,
                Name = o.Name,
                Description = o.Description,
                ContactEmail = o.ContactEmail,
                ContactNumber = o.ContactNumber,
                Address = o.Address.Address1,
                City = o.Address.City,
                Pincode = o.Address.Pincode,
                State = o.Address.State,
                AddressID = (int)o.AddressID,
                Latitude = o.Address.Latitude,
                Longitude = o.Address.Longitude,
                Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == o.StatusId).Status,
                Proofs = attachments.Select(a => a.ImagePath).ToList(),
                Thumbnail = thumbnail.Select(a => a.ImagePath).FirstOrDefault(),
            }).FirstOrDefaultAsync();
        }


        public async Task<int> UpdateBusiness(int id, UpdateBusinessRegisterDto businessRegisterDto, string UserName, int AdminPersonId , string PageSource)
        {
            var addressModel = new AddressDto();
            var events = "";
            var mailevent = "";
            addressModel.Address1 = businessRegisterDto.Address;
            addressModel.City = businessRegisterDto.City;
            addressModel.State = businessRegisterDto.State;
            addressModel.Pincode = businessRegisterDto.PinCode;
            addressModel.Latitude = businessRegisterDto.Latitude;
            addressModel.Longitude = businessRegisterDto.Longitude;
            addressModel.ID = _context.Businesses.Include(o => o.Address).AsNoTracking().FirstOrDefault(o => o.ID == id).AddressID;

            int addressId = await addressService.AddUpdateAddress(addressModel);

            var businessModel = _context.Businesses.Find(id);
            businessModel.AddressID = addressId;
            businessModel.Name = businessRegisterDto.BusinessName;
            businessModel.Description = businessRegisterDto.Description;
            businessModel.ContactEmail = businessRegisterDto.ContactEmail;
            businessModel.ContactNumber = businessRegisterDto.ContactNumber;
            businessModel.ID = (int)id;
            businessModel.StatusId = _context.StatusOptionSets.FirstOrDefault(c => c.Status == StatusConstants.UNVERIFIED).Id;
            businessModel.ModifiedBy = UserName;
            businessModel.ModifiedDate = DateTime.Now;
            events = EventConstants.EditBusiness;
            mailevent = MessageDataConstants.EditBusiness;
            _context.Businesses.Update(businessModel);

            await unitOfWork.Complete();


            var BusinssID = new List<int>() { businessModel.ID };
            var taskID = await tasksService.SaveTask(new TasksParams()
            {
                TaskPriorityID = 5,
                TaskEntity = EntityConstants.Business,
                TaskData = string.Join(",", BusinssID),
                Name = EntityConstants.Business,
                Description = TaskDescriptions.Business,
                TaskType = TaskTypeConstants.Verification
            }, AdminPersonId, UserName);


            var savedOutletModel = _context.Businesses.FirstOrDefault(o => o.ID == businessModel.ID);
            string outletModelJson = JsonConvert.SerializeObject(savedOutletModel, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = businessModel.ID, Data = outletModelJson, Description = null, EventName = events, EntityName = EntityConstants.Business, Source = PageSource });
            return businessModel.ID;
        }

        /// <summary>
        /// Retrieves all business categories with counts of businesses using them and their images.
        /// </summary>
        /// <returns>A list of business categories with their details.</returns>
        public async Task<List<BusinessCategoryDto>> GetBusinessCategories()
        {
            // Get all business categories from the database
            var categories = await _context.BusinessCategories
                .Where(c => c.IsActive == true)
                .Select(c => new BusinessCategoryDto
                {
                    Id = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    // Count businesses that use this category
                    Count = _context.Businesses.Count(b => b.BusinessCategoryID == c.ID && b.IsActive == true),
                    // Default to empty image, will be filled if available
                    Image = ""
                })
                .ToListAsync();

            // Get the attachment type ID for logos
            var attachmentTypeID_BusinessCategory = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BUSINESS_CATEGORY);
            
            if (attachmentTypeID_BusinessCategory != null)
            {
                // For each category, find the image associated with the business category ID
                foreach (var category in categories)
                {
                    // Find the image directly associated with the business category
                    var categoryImage = await (from link in _context.AttachmentLinks
                                         join attachment in _context.Attachments on link.AttachmentID equals attachment.ID
                                         join entity in _context.Entity on link.EntityID equals entity.ID
                                         where entity.Name == EntityConstants.BusinessCategory 
                                         && link.RecordID == category.Id
                                         && link.AttachmentTypeID == attachmentTypeID_BusinessCategory.ID
                                         orderby attachment.CreateDate descending
                                         select new
                                         {
                                             ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                         }).FirstOrDefaultAsync();

                    if (categoryImage != null)
                    {
                        category.Image = categoryImage.ImagePath;
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// Retrieves outlet listings grouped by category
        /// Currently retrieves only the 5 most recently added outlets
        /// </summary>
        /// <param name="categoryFilter">Optional category filter to limit results</param>
        /// <returns>Outlet listings grouped by category</returns>
        public async Task<BusinessGroupsResponseDto> GetBusinessListings(string categoryFilter = null)
        {
            var response = new BusinessGroupsResponseDto
            {
                BusinessGroups = new List<BusinessGroupDto>()
            };

            // Get the attachment type ID for outlet thumbnails
            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes
                .FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_THUMBNAIL);

            // Query base for getting outlets
            var outletsQuery = _context.Outlets
                .Where(o => o.IsActive == true && o.StatusId == _context.StatusOptionSets
                    .FirstOrDefault(s => s.Status == StatusConstants.VERIFIED).Id);

            // Apply category filter if specified
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                // Join with business categories to filter by category name
                outletsQuery = outletsQuery.Where(o => o.Bussiness.BusinessCategory.Name == categoryFilter);
            }

            // Get the 5 most recently added outlets
            var newOutlets = await outletsQuery
                .OrderByDescending(o => o.CreateDate)
                .Take(5)
                .ToListAsync();

            if (newOutlets.Count > 0)
            {
                var newOutletsGroup = new BusinessGroupDto
                {
                    Title = "New Outlets",
                    Description = "Discover the best new outlets in your area",
                    Category = categoryFilter ?? "All",
                    Businesses = new List<BusinessListingDto>()
                };

                // Get outlet images and convert to listing DTOs
                foreach (var outlet in newOutlets)
                {
                    string imagePath = "";
                    
                    // Get thumbnail image for the outlet if available
                    if (attachmentTypeID_Thumbnail != null)
                    {
                        var image = await (from link in _context.AttachmentLinks
                                         join attachment in _context.Attachments on link.AttachmentID equals attachment.ID
                                         join entity in _context.Entity on link.EntityID equals entity.ID
                                         where entity.Name == EntityConstants.Outlet
                                         && link.RecordID == outlet.ID
                                         && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                         orderby attachment.CreateDate descending
                                         select new
                                         {
                                             ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                                         }).FirstOrDefaultAsync();

                        imagePath = image?.ImagePath ?? "";
                    }

                    // Get category name from the business associated with the outlet
                    string categoryName = "General";
                    if (outlet.Bussiness?.BusinessCategoryID > 0)
                    {
                        var category = await _context.BusinessCategories
                            .Where(c => c.ID == outlet.Bussiness.BusinessCategoryID)
                            .FirstOrDefaultAsync();
                        
                        if (category != null)
                        {
                            categoryName = category.Name;
                        }
                    }

                    // Create features list from the outlet's services or offerings
                    // Using default values for now, but could be expanded to use actual data
                    var features = new List<string> { "Quality Service", "Customer Satisfaction" };

                    // Create business listing DTO
                    var outletListing = new BusinessListingDto
                    {
                        Id = outlet.ID.ToString(),
                        Name = outlet.Name,
                        Image = !string.IsNullOrEmpty(imagePath) ? imagePath : "https://www.dummyimage.co.uk/200x200",
                        Category = categoryName,
                        Type = "New",
                        Rating = outlet.Rating ?? 0,
                        ReviewCount = outlet.ReviewCount ?? 0,
                        Badge = "New",
                        WaitTime = outlet.IsOpen ? "15-30 min" : "Closed",
                        Features = features
                    };

                    newOutletsGroup.Businesses.Add(outletListing);
                }

                response.BusinessGroups.Add(newOutletsGroup);
            }
            
            return response;
        }

    }
}
