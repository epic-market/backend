using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IApplicationConfigurationService applicationConfiguration;
        private readonly UserManager<AppUser> userManager;
        private readonly IAddressService addressService;
        private readonly ICommunicationService communicationService;

        public OrderService(ApplicationDbContext context, IMapper mapper, IApplicationConfigurationService applicationConfiguration, ICommunicationService communicationService, UserManager<AppUser> userManager, IAddressService addressService)
        {
            _context = context;
            this.mapper = mapper;
            this.applicationConfiguration = applicationConfiguration;
            this.userManager = userManager;
            this.addressService = addressService;
            this.communicationService = communicationService;
        }

        public int ID { get; set; }
        public int PersonID { get; set; }
        public int BusinessID { get; set; }
        public string OrderType { get; set; }
        public double TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public DateTime OrderAt { get; set; }
        public string Status { get; set; }
        public string PaymentMode { get; set; }
        public int AddressID { get; set; }
        public async Task<int> CreateOrder(OrdersDto orderdto,string UserName)
        {

            var ListOfOrderDetails = JsonConvert.DeserializeObject<List<OrderDetailsDto>>(orderdto.OrderDetails);

            var User = new AppUser();

            User.FirstName = orderdto.CustomerName;

            User.UserName = orderdto.CustomerEmail;

            User.PhoneNumber = orderdto.CustomerPhone;

            _context.Users.Add(User);
            _context.SaveChanges();
             

            return 1;
        }

        public Task<DropDownOptions> GetOrderStatusOptions()
        {
            throw new NotImplementedException();
        }

        public Task<OrdersDto> GetSingleOrder(int OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateStatus(int OrderId, string OrderStatus)
        {
            throw new NotImplementedException();
        }
    }
}
