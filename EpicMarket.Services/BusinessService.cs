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
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace EpicMarket.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        private readonly ICommunicationQueueService communicationQueueService;
		private static readonly object _lock = new object();
		public BusinessService(
                                ApplicationDbContext context,
                                IMapper mapper ,
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                ICommunicationQueueService communicationQueueService)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
            this.communicationQueueService = communicationQueueService;
        }
        public async Task<int> RegisterBusiness(BusinessRegisterDto businessRegisterDto, string UserName , int userid, string PageSource)
        {
			var statusid = await _context.StatusOptionSets.FirstOrDefaultAsync(c => c.Status == Business_Status.BUSINESS_UNVERIFIED);
			var addressModel = new AddressDto();
            addressModel.Address1 = businessRegisterDto.Address;
            addressModel.City  = businessRegisterDto.City;
            addressModel.State = businessRegisterDto.State;
            addressModel.Pincode = businessRegisterDto.PinCode;
            addressModel.Latitude = businessRegisterDto.Latitude;
            addressModel.Longitude = businessRegisterDto.Longitude;
            addressModel.CreateBy = UserName;
            addressModel.CreateDate = DateTime.Now;
            int addressId = await addressService.AddAddress(addressModel);

            var businessModel = new Business();
            businessModel.AddressID = addressId;
            businessModel.PersonID  = userid;
            businessModel.BusinessCategoryID = businessRegisterDto.BusinessCategoryID;
            businessModel.Name = businessRegisterDto.BussinessName;
            businessModel.Description = businessRegisterDto.Description;
           // businessModel.Logo = businessRegisterDto.LogoURL;
            businessModel.ContactNumber = businessRegisterDto.ContactNumber;
            businessModel.ContactEmail = businessRegisterDto.ContactEmail;
            businessModel.CreateBy = UserName;
            businessModel.CreateDate = DateTime.Now;
			businessModel.StatusId = statusid.Id;
                 
			await _context.Businesses.AddAsync(businessModel);
			await _context.SaveChangesAsync();

            string savedJson = JsonConvert.SerializeObject(businessModel, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = businessModel.ID, Data = savedJson, Description = null, EventName = EventConstants.AddBusiness, EntityName = EntityConstants.Business, Source = PageSource });
            await this.communicationQueueService.InsertCommunicationQueue(
                    new Entities.CommunicationQueueDTO()
                    {
                        MessageData = null,//TODO
                        Subject = MessageDataConstants.AddBusiness,
                        NotificationRecipient = UserName,
                        ContactMethod = ContactMethodConstants.EMAIL,
                        CreateBy = UserName
                    });

            return businessModel.ID;
        }
    }
}
