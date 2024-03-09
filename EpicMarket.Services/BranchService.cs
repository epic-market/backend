using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;
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

        public BranchService(ApplicationDbContext context, IMapper mapper,IAddressService addressService)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
        }

        public async Task<int> AddBranch(BranchDto branchDto,  string UserName)
        {
            var addressModel = new AddressDto();
            addressModel.Address1 = branchDto.Address;
            addressModel.City = branchDto.City;
            addressModel.State = branchDto.State;
            addressModel.Pincode = branchDto.Pincode;
            addressModel.Latitude = branchDto.Latitude;
            addressModel.Longitude = branchDto.Longitude;
            addressModel.CreateBy = UserName;
            addressModel.CreateDate = DateTime.Now;

            int addressId = await addressService.AddAddress(addressModel); 

            var outletModel = new Outlet();
            outletModel.AddressID = addressId;
            outletModel.BussinessID = branchDto.BussinessID;
            outletModel.Name = branchDto.Name;
            outletModel.Description = branchDto.Description;
            outletModel.ContactEmail = branchDto.ContactEmail;
            outletModel.ContactNumber = branchDto.ContactNumber;
            outletModel.CreateBy = UserName;
            outletModel.CreateDate = DateTime.Now;

            _context.Outlets.Add(outletModel);
            await _context.SaveChangesAsync();

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
                var branchPeople = new OutletPerson();
                branchPeople.OutletId = branchProductMap.OutletId;
                branchPeople.PersonId = i;
                _context.OutletPeople.Add(branchPeople);
            }

            foreach (var i in branchProductMap.RemovedProductsId)
            {
                var branchPeople = await _context.OutletPeople.Where(c => c.PersonId == i && c.OutletId == branchProductMap.OutletId).FirstOrDefaultAsync();
                _context.OutletPeople.Remove(branchPeople);
            }

            var j = await _context.SaveChangesAsync();

            return j;
        }

        public async Task<List<BranchResult>> GetAllBranches(BranchParams branchParams)
        {

            //1 . filter with BusinessID
            var Outlets = _context.Outlets
                                .Where(c => c.BussinessID == branchParams.BusinessId);


            //2 . Appling Searching
            var sortedOutlets = Outlets.Where(row => row.Name.Contains(branchParams.searchTerm) || row.Description.Contains(branchParams.searchTerm));


            // 3 .Appying Sorting
            switch (branchParams.sortColumn)
            {
                case "BussinessID":
                    sortedOutlets = branchParams.ascending ? sortedOutlets.OrderBy(c => c.BussinessID) : sortedOutlets.OrderByDescending(c => c.BussinessID);
                    break;
                case "Name":
                    sortedOutlets = branchParams.ascending ? sortedOutlets.OrderBy(c => c.Name) : sortedOutlets.OrderByDescending(c => c.Name);
                    break;
                default:
                    break;
            }

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
                State = c.Address.State
            }).ToListAsync();

            return results;
        }



    }
}
