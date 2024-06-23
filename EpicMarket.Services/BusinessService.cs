using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EpicMarket.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        public BusinessService(ApplicationDbContext context, IMapper mapper , IAddressService addressService, IEventLogService eventLogService)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
        }
        public int RegisterBusiness(BusinessRegisterDto businessRegisterDto, string UserName , int userid, string PageSource)
        {
          
            var addressModel = new AddressDto();
            addressModel.Address1 = businessRegisterDto.Address;
            addressModel.City  = businessRegisterDto.City;
            addressModel.State = businessRegisterDto.State;
            addressModel.Pincode = businessRegisterDto.PinCode;
            addressModel.Latitude = businessRegisterDto.Latitude;
            addressModel.Longitude = businessRegisterDto.Longitude;
            addressModel.CreateBy = UserName;
            addressModel.CreateDate = DateTime.Now;
            int addressId = addressService.AddAddress(addressModel);

            var businessModel = new Business();
            businessModel.AddressID = addressId;
            businessModel.PersonID  = userid;
            businessModel.BusinessCategoryID = businessRegisterDto.BusinessCategoryID;
            businessModel.Name = businessRegisterDto.BussinessName;
            businessModel.Description = businessRegisterDto.Description;
            businessModel.Logo = businessRegisterDto.LogoURL;
            businessModel.ContactNumber = businessRegisterDto.ContactNumber;
            businessModel.ContactEmail = businessRegisterDto.ContactEmail;
            businessModel.CreateBy = UserName;
            businessModel.CreateDate = DateTime.Now;
            businessModel.StatusId = _context.StatusOptionSets.FirstOrDefault(c => c.Status == Business_Status.BUSINESS_UNVERIFIED).Id;
            _context.Businesses.Add(businessModel);
            _context.SaveChanges();
            string data = JsonSerializer.Serialize(businessModel);
            this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = businessModel.ID, Data = data, Description = null, EventName = EventConstants.AddBusiness, EntityName = EntityConstants.Business ,Source=PageSource});
            return businessModel.ID;
        }
    }
}
