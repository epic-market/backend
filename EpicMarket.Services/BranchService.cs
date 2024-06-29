using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public BranchService(ApplicationDbContext context, IMapper mapper,IAddressService addressService, IEventLogService eventLogService)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
        }

        public int AddOrUpdateBranch(BranchDto branchDto,  string UserName, int BusinessID, string PageSource)
        {
            var addressModel = new AddressDto();
            var events="";
            addressModel.Address1 = branchDto.Address;
            addressModel.City = branchDto.City;
            addressModel.State = branchDto.State;
            addressModel.Pincode = branchDto.Pincode;
            addressModel.Latitude = branchDto.Latitude;
            addressModel.Longitude = branchDto.Longitude;

            if (branchDto.ID != null || branchDto.ID > 0)
            {
                addressModel.ModifiedBy = UserName;
                addressModel.ModifiedDate = DateTime.Now;
                addressModel.ID = branchDto.AddressID;
            }
            else {
                addressModel.CreateBy = UserName;
                addressModel.CreateDate = DateTime.Now;
            }
           
            
            int addressId =  addressService.AddAddress(addressModel); 

            var outletModel = new Outlet();
            outletModel.AddressID = addressId;
            outletModel.BussinessID = BusinessID;
            outletModel.Name = branchDto.Name;
            outletModel.Description = branchDto.Description;
            outletModel.ContactEmail = branchDto.ContactEmail;
            outletModel.ContactNumber = branchDto.ContactNumber;
            outletModel.ID = (int)branchDto.ID;

            if (branchDto.ID != null || branchDto.ID > 0)
            {
                outletModel.StatusId = _context.StatusOptionSets.FirstOrDefault(c => c.Status == Business_Status.BUSINESS_UNVERIFIED).Id;
                outletModel.ModifiedBy = UserName;
                outletModel.ModifiedDate = DateTime.Now;
                events = EventConstants.EditBranch;
                _context.Outlets.Update(outletModel);
            }
            else
            {
                outletModel.CreateBy = UserName;
                outletModel.CreateDate = DateTime.Now;
                events = EventConstants.AddBranch;
                _context.Outlets.Add(outletModel);
            }
      
             _context.SaveChanges();
            var savedOutletModel = _context.Outlets.FirstOrDefault(o => o.ID == outletModel.ID);
            string outletModelJson = JsonConvert.SerializeObject(savedOutletModel, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = outletModel.ID, Data = outletModelJson, Description = null, EventName = events, EntityName = EntityConstants.Branch,Source=PageSource });

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
            
            foreach (var i in branchProductMap.AddProductsId)
            {
                var branchProduct = new OutletProduct();
                branchProduct.OutletID = branchProductMap.OutletId;
                branchProduct.ProductID = i;
                _context.OutletProducts.Add(branchProduct);
            }

            foreach (var i in branchProductMap.RemovedProductsId)
            {
                var removedProducts = await _context.OutletProducts.Where(c => c.ProductID == i && c.OutletID == branchProductMap.OutletId).FirstOrDefaultAsync();
                _context.OutletProducts.Remove(removedProducts);
            }

            var j = await _context.SaveChangesAsync();

            return j;
        }

        public async Task<GetDataResult<List<BranchResult>>> GetAllBranches(BranchParams branchParams, int BusinessID)
        {

            var getResult = new GetDataResult<List<BranchResult>>();

            //1 . filter with BusinessID
            var Outlets = _context.Outlets
                                .Where(c => c.BussinessID == BusinessID);


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
                State = c.Address.State,
            }).ToListAsync();


            getResult.items = results;
            getResult.Count = totalCount;

            return getResult;
        }

        public async Task<BranchResult> GetBranchByID(int branchId)
        {
           return await _context.Outlets.Where(o => o.ID == branchId).Include(o=> o.Address).Select(o => new BranchResult()
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
               AddressID = o.AddressID
           }).FirstOrDefaultAsync();
        }


    }
}
