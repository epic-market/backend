using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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



        public  int CreateOrder(OrdersDto orderdto,string UserName)
        {   
            var User = new AppUser();
            var totalItems = 0;
            double totalPrice = 0.0;
            User.FirstName = orderdto.CustomerName;
            User.UserName = orderdto.CustomerEmail;
            User.PhoneNumber = orderdto.CustomerPhone;
            _context.Users.Add(User);
            _context.SaveChanges();
            var listoforderDetails = new List<OrderDetail>();
            foreach(var orderDetail in orderdto.orderDetailsDtos)
            {
                var catelog = _context.Catalogs.FirstOrDefault(c => c.ID == orderDetail.CatalogID);
                var singleOrderDetail = new OrderDetail();
                singleOrderDetail.CatalogID = orderDetail.CatalogID;
                singleOrderDetail.Quantity = orderDetail.Quantity;
                singleOrderDetail.Rate = catelog.Rate;
                singleOrderDetail.TotalPrice = catelog.Rate * orderDetail.Quantity;
                listoforderDetails.Add(singleOrderDetail);
                totalItems += orderDetail.Quantity;
                totalPrice += (catelog.Rate * orderDetail.Quantity);
            }
            var newOrder = new Order()
            {
                PersonID = User.Id,
				OutletID = orderdto.OutletId,
                OrderType = orderdto.OrderedMode,
                OrderAt = DateTime.Now,
                Status = orderdto.Status,
                PaymentMode = orderdto.PaymentMode,
                TotalItems = totalItems,
                TotalPrice = totalPrice,
            };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            listoforderDetails.ForEach(od => od.OrderID = newOrder.ID);
            _context.OrderDetails.AddRange(listoforderDetails);
            _context.SaveChanges();
            return newOrder.ID;
        }

        public async Task<List<DropDownOptions>> GetOrderStatusOptions()
        {
            return await _context.OrderStatusOptions.Select(c => new DropDownOptions()
            {
                Text = c.OrderStatus,
                Value = c.Id
            }).ToListAsync();
        }

        public async Task<OrdersDto> GetSingleOrder(int OrderId)
        {
			List<OrderDetail> OrderDetails  = await _context.OrderDetails.Where(c => c.OrderID == OrderId ).ToListAsync();
            string OrderDetailsString = JsonConvert.SerializeObject(OrderDetails);


            var Order = await _context.Orders.Where(c => c.ID == OrderId).Include(c => c.Person).Include(c => c.Address).Select(
                c => new OrdersDto()
                {
                    CustomerName = c.Person.FirstName + " " + c.Person.LastName,
                    CustomerEmail = c.Person.Email,
                    CustomerPhone = c.Person.PhoneNumber,
                    OrderDate = c.OrderAt,
                    PaymentMode = c.PaymentMode,
                    OrderedMode = c.OrderType,
                    Status = c.Status,
                    OrderDetails = OrderDetailsString,
                }).FirstOrDefaultAsync();

            return Order;

        }

        public int UpdateStatus(int OrderId, string OrderStatus)
        {
            var Order = _context.Orders.Find(OrderId);
            Order.Status = OrderStatus;
            _context.Orders.Update(Order);
            _context.SaveChanges();

            return Order.ID;

        }

        public async Task<List<OrderResult>> GetAllOrders(OrderParams orderParams, int businessID)
        {

            //1 . filter with BusinessID
            var orders = _context.Orders.Include(c=>c.Outlet).
                Where(c => c.Outlet.BussinessID == businessID).Include(c=> c.Person);


            //2 . Appling Searching
            var sortedOrders = orders.Where(row => row.Person.FirstName.Contains(orderParams.searchTerm.Trim()) || row.ID.ToString() == orderParams.searchTerm.Trim());


            // 3 .Appying Sorting
            switch (orderParams.sortColumn)
            {
                case "OrderID":
                    sortedOrders = orderParams.ascending ? sortedOrders.OrderBy(c => c.ID) : sortedOrders.OrderByDescending(c => c.ID);
                    break;
                case "Status":
                    sortedOrders = orderParams.ascending ? sortedOrders.OrderBy(c => c.Status) : sortedOrders.OrderByDescending(c => c.Status);
                    break;
                default:
                    break;
            }

            //getting the total count
            int totalCount = sortedOrders.Count();


            // 4. Apply pagination (skip and take)
            var pagedOrders = sortedOrders
                .Skip((orderParams.PageIndex - 1) * orderParams.pageSize) // Skip items for previous pages
                .Take(orderParams.pageSize); // Take items for the current page

            // 5. Select data and add SRNO
            var results = await pagedOrders.Select(c => new OrderResult()
            {
                ID = c.ID,
                CustomerName = c.Person.FirstName + "" + c.Person.LastName,
                Status = c.Status,
                TotalPrice = c.TotalPrice,
                TotalItems = c.TotalItems,
                OrderType = c.OrderType,
                Count = totalCount
            }).ToListAsync();

            return results;
        }

    }
}
