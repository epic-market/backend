using Amazon.S3.Model.Internal.MarshallTransformations;
using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        private readonly ICommunicationQueueService communicationQueueService;
		private readonly IUnitOfWork unitOfWork;

		public BranchService(
                                ApplicationDbContext context,
                                IMapper mapper,
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                ICommunicationQueueService communicationQueueService,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
            this.communicationQueueService = communicationQueueService;
			this.unitOfWork = unitOfWork;
		}

        public async Task<int> AddBranch(BranchDto branchDto,  string UserName, int BusinessID, string PageSource)
        {
                var addressModel = new AddressDto
                {
                    Address1 = branchDto.Address,
                    City = branchDto.City,
                    State = branchDto.State,
                    Pincode = branchDto.Pincode,
                    Latitude = branchDto.Latitude,
                    Longitude = branchDto.Longitude,
                    CreateBy = UserName,
                    CreateDate = DateTime.Now
                };
            
                int addressId = await addressService.AddAddress(addressModel);

                var outletModel = new Outlet
                {
                    AddressID = addressId,
                    BussinessID = BusinessID,
                    Name = branchDto.Name,
                    Description = branchDto.Description,
                    ContactEmail = branchDto.ContactEmail,
                    ContactNumber = branchDto.ContactNumber,
                    CreateBy = UserName,
                    CreateDate = DateTime.Now
                };
                var status = await _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == Business_Status.BUSINESS_UNVERIFIED);
                outletModel.StatusId = status.Id;



                await _context.Outlets.AddAsync(outletModel);
                await unitOfWork.Complete();

                string events = EventConstants.AddBranch;
                string mailevent = MessageDataConstants.EditBranch;



                var savedOutletModel = _context.Outlets.FirstOrDefault(o => o.ID == outletModel.ID);
                string outletModelJson = JsonConvert.SerializeObject(savedOutletModel, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = outletModel.ID, Data = outletModelJson, Description = null, EventName = events, EntityName = EntityConstants.Branch, Source = PageSource });
                await this.communicationQueueService.InsertCommunicationQueue(
                        new Entities.CommunicationQueueDTO()
                        {
                            MessageData = null,//TODO
                            Subject = mailevent,
                            NotificationRecipient = UserName,
                            ContactMethod = ContactMethodConstants.EMAIL,
                            CreateBy = UserName
                        });

                return outletModel.ID;
        
        }


        public async Task<int> UpdateBranch(int id, BranchDto branchDto, string UserName, int BusinessID, string PageSource)
        {
            var addressModel = new AddressDto();
            var events = "";
            var mailevent = "";
            addressModel.Address1 = branchDto.Address;
            addressModel.City = branchDto.City;
            addressModel.State = branchDto.State;
            addressModel.Pincode = branchDto.Pincode;
            addressModel.Latitude = branchDto.Latitude;
            addressModel.Longitude = branchDto.Longitude;
            addressModel.ID = _context.Outlets.Include(o => o.Address).AsNoTracking().FirstOrDefault(o => o.ID == id).AddressID;

            int addressId = await addressService.AddAddress(addressModel);

            var outletModel = _context.Outlets.Find(id);
            outletModel.AddressID  = addressId;
            outletModel.BussinessID = BusinessID;
            outletModel.Name = branchDto.Name;
            outletModel.Description = branchDto.Description;
            outletModel.ContactEmail = branchDto.ContactEmail;
            outletModel.ContactNumber = branchDto.ContactNumber;
            outletModel.ID = (int)id;
            outletModel.StatusId = _context.StatusOptionSets.FirstOrDefault(c => c.Status == Business_Status.BUSINESS_UNVERIFIED).Id;
            outletModel.ModifiedBy = UserName;
            outletModel.ModifiedDate = DateTime.Now;
            events = EventConstants.EditBranch;
            mailevent = MessageDataConstants.AddBranch;
            _context.Outlets.Update(outletModel);

            await unitOfWork.Complete();
            var savedOutletModel = _context.Outlets.FirstOrDefault(o => o.ID == outletModel.ID);
            string outletModelJson = JsonConvert.SerializeObject(savedOutletModel, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = outletModel.ID, Data = outletModelJson, Description = null, EventName = events, EntityName = EntityConstants.Branch, Source = PageSource });
            await this.communicationQueueService.InsertCommunicationQueue(
                    new Entities.CommunicationQueueDTO()
                    {
                        MessageData = null,//TODO
                        Subject = mailevent,
                        NotificationRecipient = UserName,
                        ContactMethod = ContactMethodConstants.EMAIL,
                        CreateBy = UserName
                    });
            return outletModel.ID;
        }


        public async Task<int> MapBranchToPeople(BranchPeopleMapParams branchPeopleMapParams)
        {

            foreach(var i in branchPeopleMapParams.AddPersonId)
            {
                var branchPeople = new OutletPerson();
                branchPeople.OutletId = branchPeopleMapParams.OutletId;
                branchPeople.PersonId = i;
                _context.OutletPeople.Add(branchPeople);

            }

            foreach (var i in branchPeopleMapParams.RemovedPersonId)
            {
                var branchPeople = await _context.OutletPeople.Where(c => c.PersonId == i && c.OutletId == branchPeopleMapParams.OutletId).FirstOrDefaultAsync();
                _context.OutletPeople.Remove(branchPeople);
            }

            var j = await _context.SaveChangesAsync();

            return j;

        }

        public async Task<int> MapBranchToProducts(BranchProductMapParams branchProductMap)
        {
            
            foreach (var i in branchProductMap?.AddProductsId)
            {
                var branchProduct = new OutletProduct();
                branchProduct.OutletID = branchProductMap.OutletId;
                branchProduct.ProductID = i;
                _context.OutletProducts.Add(branchProduct);
            }

            foreach (var i in branchProductMap?.RemovedProductsId)
            {
                var removedProducts = await _context.OutletProducts.Where(c => c.ProductID == i && c.OutletID == branchProductMap.OutletId).FirstOrDefaultAsync();
                _context.OutletProducts.Remove(removedProducts);
            }

            var j = await _context.SaveChangesAsync();

            return j;
        }

        public async Task<GetDataResult<List<BranchResult>>> GetAllBranches(BranchParams branchParams, int BusinessID)
        {
            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_THUMBNAIL);
            var getResult = new GetDataResult<List<BranchResult>>();

            //1 . filter with BusinessID
            var Outlets = _context.Outlets
                                .Where(c => c.BussinessID == BusinessID && c.IsActive == true);


            //2 . Appling Searching
            var sortedOutlets = Outlets.Where(row => row.Name.Contains(branchParams.searchTerm.Trim()) || row.Description.Contains(branchParams.searchTerm.Trim()));


            // 3 .Appying Sorting
            switch (branchParams.sortColumn)
            {
                case "BranchID":
                    sortedOutlets = branchParams.ascending ? sortedOutlets.OrderBy(c => c.ID) : sortedOutlets.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    sortedOutlets = branchParams.ascending ? sortedOutlets.OrderBy(c => c.Name) : sortedOutlets.OrderByDescending(c => c.Name);
                    break;
                default:
                    break;
            }

            //getting the total count
            int totalCount = sortedOutlets.Count();


            // 4. Apply pagination (skip and take)
            var pagedOutlets = sortedOutlets
                .Skip((branchParams.PageIndex - 1) * branchParams.pageSize) // Skip items for previous pages
                .Take(branchParams.pageSize); // Take items for the current page

            // 5. Select data and add SRNO
            var results =  await pagedOutlets.Include(c=>c.Address).Select(c => new BranchResult()
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                ContactEmail = c.ContactEmail,
                ContactNumber = c.ContactNumber,
                Address = c.Address.Address1,
                City = c.Address.City,
                Pincode = c.Address.Pincode,
                Latitude = c.Address.Latitude,
                Longitude = c.Address.Longitude,
                IsOpen=c.IsOpen,
                State = c.Address.State,
                Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == c.StatusId).Status,
                Thumbnail = ((from attachment in _context.Attachments
                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                              join entity in _context.Entity on link.EntityID equals entity.ID
                              where entity.Name == EntityConstants.Branch && link.RecordID == c.ID && link.AttachmentTypeID == attachmentTypeID.ID
                              select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}").FirstOrDefault()),
            }).ToListAsync();


            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

        public async Task<BranchDetailResult> GetBranchByID(int branchId)
        {

            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_THUMBNAIL);
            var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_PHOTOS);



            var attachments = from attachment in _context.Attachments
                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                              join entity in _context.Entity on link.EntityID equals entity.ID
                              where entity.Name == EntityConstants.Branch && link.RecordID == branchId && link.AttachmentTypeID == attachmentTypeID_Product.ID
                              select new
                              {
                                  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                              };

            var thumbnail = from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            where entity.Name == EntityConstants.Branch && link.RecordID == branchId && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                            orderby attachment.CreateDate descending
                            select new
                            {
                                ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                            };

            return await _context.Outlets.Where(o => o.ID == branchId && o.IsActive == true).Include(o=> o.Address).Select(o => new BranchDetailResult()
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
               AddressID = o.AddressID,
               Latitude=o.Address.Latitude,
               Longitude=o.Address.Longitude,
               Status = _context.StatusOptionSets.FirstOrDefault(s => s.Id == o.StatusId).Status,
               Photos = attachments.Select(a => a.ImagePath).ToList(),
               Thumbnail = thumbnail.Select(a => a.ImagePath).FirstOrDefault(),
            }).FirstOrDefaultAsync();
        }

        public async Task<int> VerifyBranchs (VerifyDto verifyBranchDto, string UserName, int AdminPersonID, string PageSource)
        {
            var newTaskStatus = _context.TaskStatusTypes.Where(row => row.Status == "New").FirstOrDefault();
            var taskTypeID = _context.TaskTypes.Where(row => row.Name == "Verification").FirstOrDefault();
            var userName = _context.Users.Where(row => row.UserName == UserName).FirstOrDefault();
            Tasks taskToSave;
                taskToSave = new Tasks
                {
                    Name = VerificationConstants.BranchName,
                    Description = VerificationConstants.BranchDescription,
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
                _context.Tasks.Add(taskToSave);
			await unitOfWork.Complete(); ;
            return taskToSave.ID;
               
        }

        public async Task DeleteBranch(int branchId, string UserName)
        {
            var outlet = await _context.Outlets.FindAsync(branchId);
            if (outlet != null)
            {
                outlet.IsActive = false;
                _context.Outlets.Update(outlet);
                await unitOfWork.Complete();
            }
            else
            {
                throw new Exception("Branch Not Found");
            }
        }


        public async Task<List<BranchsDropDownOptions>> GetAllOutletsForDropDown(int personID , int businessID)
        {

            if (personID == _context.Businesses.FirstOrDefault(c => c.ID == businessID).PersonID)
            {
                return await _context.Outlets.Where(c => c.IsActive == true).Select(
                     c => new BranchsDropDownOptions()
                     {
                         Text = c.Name,
                         Value = c.ID,
                         IsOpen=c.IsOpen
                     }
                    ).ToListAsync();
            }
            else {
                return await _context.OutletPeople.Include(c => c.Outlet).Where(c => c.PersonId == personID && c.Outlet.IsActive == true).Select(
                      c => new BranchsDropDownOptions()
                      {
                          Text = c.Outlet.Name,
                          Value = c.OutletId,
                          IsOpen = c.Outlet.IsOpen
                      }
                     ).ToListAsync();

            }
            
      
        }

    }
}
