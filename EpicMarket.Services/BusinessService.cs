using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public BusinessService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public async Task<int> RegisterBusiness(BusinessRegisterDto businessRegisterDto)
        {
            var addressModel = new Address();
            addressModel.Address1 = businessRegisterDto.Address;
            addressModel.City  = businessRegisterDto.City;
            addressModel.State = businessRegisterDto.State;
            addressModel.Pincode = businessRegisterDto.PinCode;
            addressModel.Latitude = businessRegisterDto.Latitude;
            addressModel.Longitude = businessRegisterDto.Longitude;
            _context.Addresses.Add(addressModel);
             await _context.SaveChangesAsync();
            int addressId = addressModel.Id;

            var businessModel = new Business();
            businessModel.AddressID = addressId;
            businessModel.PersonID  = businessRegisterDto.UserID;
            businessModel.BusinessCategoryID = businessRegisterDto.BusinessCategoryID;
            businessModel.Name = businessRegisterDto.BussinessName;
            businessModel.Description = businessRegisterDto.Description;
            businessModel.Logo = businessRegisterDto.LogoURL;
            businessModel.ContactNumber = businessRegisterDto.ContactNumber;
            businessModel.ContactEmail = businessRegisterDto.ContactEmail;

            _context.Businesses.Add(businessModel);
            await _context.SaveChangesAsync();



            return businessModel.ID;
        }
    }
}
