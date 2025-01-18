using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IEventLogService eventLogService;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(ApplicationDbContext context, IMapper mapper, IApplicationConfigurationService applicationConfiguration, ICommunicationService communicationService, UserManager<AppUser> userManager, IAddressService addressService, IEventLogService eventLogService, IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.applicationConfiguration = applicationConfiguration;
            this.userManager = userManager;
            this.addressService = addressService;
            this.communicationService = communicationService;
            this.eventLogService = eventLogService;
            this.unitOfWork = unitOfWork;
        }


        public async Task<int> CreateOrder(OrdersDto orderdto, string UserName, string PageSource)
        {
            try
            {
                var User = new AppUser();
                var totalItems = 0;
                double totalPrice = 0.0;

                // Get or create unknown user if both phone and email are empty
                if (string.IsNullOrEmpty(orderdto.CustomerPhone) && string.IsNullOrEmpty(orderdto.CustomerEmail))
                {
                    var unknownUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserName == "unknown_user");

                    if (unknownUser == null)
                    {
                        User.FirstName = "Unknown";
                        User.UserName = "unknown_user";
                        User.Email = null;
                        User.PhoneNumber = null;
                        await _context.Users.AddAsync(User);
                        await unitOfWork.Complete();
                    }
                    else
                    {
                        User.Id = unknownUser.Id;
                    }
                }
                else
                {
                    // Find existing user by phone or email if either exists
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => 
                            (!string.IsNullOrEmpty(orderdto.CustomerEmail) && u.UserName == orderdto.CustomerEmail) || 
                            (!string.IsNullOrEmpty(orderdto.CustomerPhone) && u.PhoneNumber == orderdto.CustomerPhone));

                    if (existingUser == null)
                    {
                        User.FirstName = orderdto.CustomerName ?? "Unknown";
                        
                        // Set username based on available info
                        if (!string.IsNullOrEmpty(orderdto.CustomerEmail))
                        {
                            User.UserName = orderdto.CustomerEmail;
                            User.Email = orderdto.CustomerEmail;
                        }
                        else if (!string.IsNullOrEmpty(orderdto.CustomerPhone))
                        {
                            User.UserName = orderdto.CustomerPhone;
                            User.Email = null;
                        }
                        
                        User.PhoneNumber = orderdto.CustomerPhone;
                        await _context.Users.AddAsync(User);
                        await unitOfWork.Complete();
                    }
                    else
                    {
                        User.Id = existingUser.Id;
                    }
                }

                var OrderTypeID = await _context.OrderTypesOptions
                    .FirstOrDefaultAsync(u => u.Ordertype == OrderType.OFFLINE);

                if (OrderTypeID == null)
                {
                    throw new Exception("Order type OFFLINE not found");
                }

                // Check if the outlet exists
                var outletExists = await _context.Outlets.AnyAsync(o => o.ID == orderdto.OutletId && o.IsActive == true);
                if (!outletExists)
                {
                    throw new Exception($"Outlet with ID {orderdto.OutletId} not found");
                }

                var listoforderDetails = new List<OrderDetail>();

                foreach(var orderDetail in orderdto.orderDetailsDtos)
                {
                    var catelogVariant = await _context.ProductVariants.Include(c => c.Catalog)
                        .FirstOrDefaultAsync(c => c.VariantID == orderDetail.VariantID && c.IsActive == true);

                    if (catelogVariant == null)
                    {
                        throw new Exception($"Product Variant with ID {orderDetail.VariantID} not found");
                    }

                    if (orderDetail.Quantity <= 0)
                    {
                        throw new ArgumentException($"Invalid quantity for product {catelogVariant.Catalog.Name}");
                    }

                    var singleOrderDetail = new OrderDetail
                    {
                        VariantID = orderDetail.VariantID,
                        Quantity = orderDetail.Quantity,
                        Rate = catelogVariant.SalePrice,
                        TotalPrice = catelogVariant.SalePrice * orderDetail.Quantity
                    };

                    listoforderDetails.Add(singleOrderDetail);
                    totalItems += orderDetail.Quantity;
                    totalPrice += (catelogVariant.SalePrice * orderDetail.Quantity);
                }

                var newOrder = new Order()
                {
                    PersonID = User.Id,
                    OutletID = orderdto.OutletId,
                    OrderTypeId = OrderTypeID.Id,
                    OrderAt = DateTime.UtcNow,
                    StatusId = orderdto.StatusId,
                    PaymentMode = orderdto.PaymentMode,
                    TotalItems = totalItems,
                    TotalPrice = totalPrice,
                };

                await _context.Orders.AddAsync(newOrder);
                await unitOfWork.Complete();

                listoforderDetails.ForEach(od => od.OrderID = newOrder.ID);
                await _context.OrderDetails.AddRangeAsync(listoforderDetails);
                await unitOfWork.Complete();

                var saved = await _context.Orders.FirstOrDefaultAsync(o => o.ID == newOrder.ID);
                string saveJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS 
                { 
                    RecordId = newOrder.ID,
                    Data = saveJson,
                    Description = null,
                    EventName = EventConstants.AddOrder,
                    EntityName = EntityConstants.Order,
                    Source = PageSource 
                });

                return newOrder.ID;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating order: {ex.Message}");
            }
        }

    
        public async Task<OrdersDetailsResult> GetSingleOrder(int OrderId)
        {

            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);

            List<OrderDetails> OrderDetails = await _context.OrderDetails.Include(c => c.ProductVariants).Where(c => c.OrderID == OrderId)
                .Select(c => new OrderDetails()
                {
                    CatalogID = c.ProductVariants.Catalog.ID,
                    ProductName = c.ProductVariants.Catalog.Name,
                    Quantity = c.Quantity,
                    Rate = c.Rate,
                    TotalPrice = c.TotalPrice,
                    Thumbnail = ((from attachment in _context.Attachments
                                  join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                                  join entity in _context.Entity on link.EntityID equals entity.ID
                                  where entity.Name == EntityConstants.Catelog && link.RecordID == c.ProductVariants.Catalog.ID && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                                  select $"{attachment.DocumentFolderPath}{attachment.DocumentFile}").FirstOrDefault())
                }).ToListAsync();

            string OrderDetailsString = JsonConvert.SerializeObject(OrderDetails);
            var Order = await _context.Orders.Where(c => c.ID == OrderId).Include(c => c.Person).Include(c=>c.OrderTypesOptions).Include(c=>c.Outlet).Include(c=>c.OrderStatusOptions).Include(c => c.Address).Select(
                c => new OrdersDetailsResult()
                {
                    CustomerName = c.Person.FirstName + " " + c.Person.LastName,
                    CustomerEmail = c.Person.Email,
                    CustomerPhone = c.Person.PhoneNumber,
                    OrderDate = c.OrderAt,
                    PaymentMode = c.PaymentMode,
                    OrderMode = c.OrderTypesOptions.Ordertype,
                    Status = c.OrderStatusOptions.OrderStatus,
                    TotalItems = c.TotalItems,
                    TotalPrice = c.TotalPrice,
                    OrderDetails = OrderDetails,
                    OutletName = c.Outlet.Name,
                    OutletId = c.OutletID,
                }).FirstOrDefaultAsync();

            return Order;

        }

        public async Task<int> UpdateStatus(int OrderId, int StatusId)
        {
            var Order = _context.Orders.Find(OrderId);
            Order.StatusId = StatusId;
            _context.Orders.Update(Order);
			await unitOfWork.Complete();
			await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS { RecordId = Order.ID, Data = StatusId.ToString(), Description = null, EventName = EventConstants.EditOrder, EntityName = EntityConstants.Order });
            return Order.ID;

        }

        public async Task<GetDataResult<List<OrderResult>>> GetAllOrders(OrderParams orderParams, int businessID)
        {
            var getData = new GetDataResult<List<OrderResult>>();

            //1. Filter with BusinessID and include related data
            var query = _context.Orders
                .Include(c => c.Outlet)
                .Include(c => c.Person)
                .Include(c => c.OrderStatusOptions)
                .Include(c => c.OrderTypesOptions)
                .Where(c => c.Outlet.BussinessID == businessID);

            //2. Apply searching if search term provided
            if (!string.IsNullOrWhiteSpace(orderParams.searchTerm))
            {
                var searchTerm = orderParams.searchTerm.Trim();
                query = query.Where(o => 
                    o.Person.FirstName.Contains(searchTerm) || 
                    o.ID.ToString().Contains(searchTerm)
                );
            }

            //3. Apply sorting
            query = orderParams.sortColumn?.ToLower() switch
            {
                "orderid" => orderParams.ascending ? 
                    query.OrderBy(c => c.ID) : 
                    query.OrderByDescending(c => c.ID),
                "status" => orderParams.ascending ?
                    query.OrderBy(c => c.StatusId) :
                    query.OrderByDescending(c => c.StatusId),
                _ => query.OrderByDescending(c => c.OrderAt) // Default sort
            };

            //4. Get total count before pagination
            var totalCount = await query.CountAsync();

            //5. Apply pagination and select results
            var results = await query
                .Skip((orderParams.PageIndex - 1) * orderParams.pageSize)
                .Take(orderParams.pageSize)
                .Select(c => new OrderResult
                {
                    ID = c.ID,
                    CustomerName = c.Person.FirstName + " " + c.Person.LastName,
                    Status = c.OrderStatusOptions.OrderStatus,
                    TotalPrice = c.TotalPrice,
                    TotalItems = c.TotalItems,
                    OrderType = c.OrderTypesOptions.Ordertype,
                    Count = totalCount
                })
                .ToListAsync();

            getData.items = results;
            getData.Count = totalCount;

            return getData;
        }


        public async Task<GetDataResult<List<OrderMobileResult>>> GetAllOrdersForMobile(OrderParams orderParams, int businessID)
        {

            var getData = new GetDataResult<List<OrderMobileResult>>();

            //1 . filter with BusinessID
            IQueryable<Order> orders;
            if (orderParams.BranchId == null)
            {
                orders = _context.Orders.Include(c => c.Outlet).
                Where(c => c.Outlet.BussinessID == businessID).Include(c => c.Person);
            }
            else
            {
                orders = _context.Orders.Include(c => c.Outlet).
                Where(c => c.OutletID == orderParams.BranchId).Include(c => c.Person);
            }

            if (orderParams.statusID != null) {

                orders = orders.Where(c => c.StatusId == orderParams.statusID); 
            }


            var sortedOrders = orders;
            //2 . Appling Searching
            if (!String.IsNullOrEmpty(orderParams.searchTerm))
            {

                sortedOrders = orders.Where(row => (row.Person.FirstName.Contains(orderParams.searchTerm.Trim()) || row.ID.ToString().Contains(orderParams.searchTerm.Trim())));

            }

            // 3 .Appying Sorting
            switch (orderParams.sortColumn)
            {
                case "OrderID":
                    sortedOrders = orderParams.ascending ? sortedOrders.OrderBy(c => c.ID) : sortedOrders.OrderByDescending(c => c.ID);
                    break;
                case "Status":
                    sortedOrders = orderParams.ascending ? sortedOrders.OrderBy(c => c.StatusId) : sortedOrders.OrderByDescending(c => c.StatusId);
                    break;
                case "Ordered_At":
                    sortedOrders = orderParams.ascending ? sortedOrders.OrderBy(c => c.OrderAt) : sortedOrders.OrderByDescending(c => c.OrderAt);
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
            var results = await pagedOrders.Select(c => new OrderMobileResult()
            {
                ID = c.ID,
                Status = c.OrderStatusOptions.OrderStatus,
                Payment_Mode = c.PaymentMode,
                Ordered_At = c.CreateDate,
                Branch = new OrderBranchMobileResult()
                {
                    ID = c.Outlet.ID,
                    Name = c.Outlet.Name
                },
                Customer = new OrderCustomerMobileResult()
                {
                    ID = c.Person.Id,
                    Username = c.Person.UserName,
                    Name = c.Person.FirstName + "" + c.Person.LastName
                },
                Items_Peak = new OrderItemMobileResult()
                {
                    ID =c.OrderDetails.FirstOrDefault().ID,
                    Quantity= c.OrderDetails.FirstOrDefault().Quantity,
                    Name= c.OrderDetails.FirstOrDefault().ProductVariants.Catalog.Name,
                    Price=c.OrderDetails.FirstOrDefault().ProductVariants.SalePrice,
                    Total_price=c.OrderDetails.FirstOrDefault().TotalPrice
                },

                Items_count = c.TotalItems,
                Total_price = c.TotalPrice
            }).ToListAsync();


            getData.items = results;
            getData.Count = totalCount;

            return getData;
        }


        public async Task<OrderMobileDeatilsResult> GetOrdersDetailsForMobile(int OrderId)
        {

            var getData = new OrderMobileDeatilsResult();
            
            var results = await _context.Orders.Include(c => c.Outlet).
                Where(c => c.ID == OrderId).Include(c => c.Person).Select(c => new OrderMobileDeatilsResult()
            {
                ID = c.ID,
                Status = c.OrderStatusOptions.OrderStatus,
                Payment_Mode = c.PaymentMode,
                Ordered_At = c.CreateDate,
                Branch = new OrderBranchMobileResult()
                {
                    ID = c.Outlet.ID,
                    Name = c.Outlet.Name
                },
                Customer = new OrderCustomerMobileResult()
                {
                    ID = c.Person.Id,
                    Username = c.Person.UserName,
                    Name = c.Person.FirstName + "" + c.Person.LastName
                },
                Items = c.OrderDetails.Select(od => new OrderItemMobileResult()
                    {
                        ID = od.ID,
                        Quantity = od.Quantity,
                        Name = od.ProductVariants.Catalog.Name,
                        Price = od.ProductVariants.SalePrice,
                        Total_price = od.TotalPrice
                    }).ToList(),

                Items_count = c.TotalItems,
                Total_price = c.TotalPrice
            }).FirstOrDefaultAsync();

            getData = results;
            return getData;
        }

        public async Task<bool> AnyNewOrders(DateTime ordered_after, int businessid , int? outlet_id)
        {
          var listOfNewOrderPlacedAfterTheTimeStamp  = await _context.Orders.Include(c => c.Outlet).Where(c=>c.Outlet.BussinessID == businessid && c.OrderAt > ordered_after).ToListAsync();

           return listOfNewOrderPlacedAfterTheTimeStamp?.Count > 0 ? true : false;


        }

        public async Task<GetDataResult<List<CustomerOrderDto>>> GetCustomerOrderHistoryAsync(
       int customerId,
       OrderHistoryRequest request)
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o=>o.OrderStatusOptions)
                .Include(o => o.Outlet)
                .Where(o => o.PersonID == customerId)
                .AsQueryable();

            // Apply filters
            if (request.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(o => o.OrderAt <= request.EndDate.Value);
            }

            if (request.StatusId != null)
            {
                query = query.Where(o => o.StatusId == request.StatusId);
            }

            // Apply sorting
            query = (request.SortBy?.ToLower(), request.SortOrder?.ToLower()) switch
            {
            ("date", "asc") => query.OrderBy(o => o.OrderAt),
            ("date", "desc") => query.OrderByDescending(o => o.OrderAt),
            ("amount", "asc") => query.OrderBy(o => o.TotalPrice),
            ("amount", "desc") => query.OrderByDescending(o => o.TotalPrice),
            ("status", "asc") => query.OrderBy(o => o.StatusId),
            ("status", "desc") => query.OrderByDescending(o => o.StatusId),
            _ => query.OrderByDescending(o => o.OrderAt)
            };
          
            // Get total count
            var totalItems = await query.CountAsync();

            // Apply pagination
            var orders = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => new CustomerOrderDto
                {
                    OrderId = o.ID,
                    OrderDate = o.OrderAt,
                    Status = o.OrderStatusOptions.OrderStatus,
                    TotalAmount = o.TotalPrice,
                    ItemCount = o.OrderDetails.Count,
                    OutletName = o.Outlet.Name,
                    OutletId = o.OutletID,
                    PaymentMethod = o.PaymentMode
                })
                .ToListAsync();

            return new GetDataResult<List<CustomerOrderDto>>
            {
                items = orders,
                Count = totalItems
            };
        }

        public async Task<int> CreateCustomerOrder(CreateCustomerOrderDto order, int userId)
        {
            try
            {
                double totalPrice = 0.0;
                var totalItems = 0;

                // Get ONLINE order type
                var orderTypeId = await _context.OrderTypesOptions
                    .FirstOrDefaultAsync(u => u.Ordertype == OrderType.ONLINE);

                if (orderTypeId == null)
                {
                    throw new Exception("Order type ONLINE not found");
                }

                // Validate outlet
                var outletExists = await _context.Outlets
                    .AnyAsync(o => o.ID == order.OutletId && o.IsActive == true);
                if (!outletExists)
                {
                    throw new Exception($"Outlet with ID {order.OutletId} not found");
                }

                var listoforderDetails = new List<OrderDetail>();

                // Process order details
                foreach(var orderDetail in order.OrderDetailsDtos)
                {
                    var catalogVariant = await _context.ProductVariants.Include(c => c.Catalog)
                        .FirstOrDefaultAsync(c => c.VariantID == orderDetail.VariantID && c.IsActive == true);

                    if (catalogVariant == null)
                    {
                        throw new Exception($"Product Variant with ID {orderDetail.VariantID} not found");
                    }

                    if (orderDetail.Quantity <= 0)
                    {
                        throw new ArgumentException($"Invalid quantity for product {catalogVariant.Catalog.Name}");
                    }

                    var singleOrderDetail = new OrderDetail
                    {
                        VariantID = orderDetail.VariantID,
                        Quantity = orderDetail.Quantity,
                        Rate = catalogVariant.SalePrice,
                        TotalPrice = catalogVariant.SalePrice * orderDetail.Quantity
                    };

                    listoforderDetails.Add(singleOrderDetail);
                    totalItems += orderDetail.Quantity;
                    totalPrice += (catalogVariant.SalePrice * orderDetail.Quantity);
                }


                //get the order status id for the OrderStatusConstants.OrderPlaced 
                var orderStatusId = await _context.OrderStatusOptions
                    .FirstOrDefaultAsync(u => u.OrderStatus == OrderStatusConstants.OrderPlaced);

                // Create new order
                var newOrder = new Order()
                {
                    PersonID = userId,
                    OutletID = order.OutletId,
                    OrderTypeId = orderTypeId.Id,
                    OrderAt = DateTime.UtcNow,
                    StatusId = orderStatusId.Id, 
                    PaymentMode = order.PaymentMode,
                    TotalItems = totalItems,
                    TotalPrice = totalPrice,
                };

                await _context.Orders.AddAsync(newOrder);
                await unitOfWork.Complete();

                // Add order details
                listoforderDetails.ForEach(od => od.OrderID = newOrder.ID);
                await _context.OrderDetails.AddRangeAsync(listoforderDetails);
                await unitOfWork.Complete();

                // Log the event
                var saved = await _context.Orders.FirstOrDefaultAsync(o => o.ID == newOrder.ID);
                string saveJson = JsonConvert.SerializeObject(saved, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                await this.eventLogService.LogEvent(new EVENT_LOG_SAVE_PARAMS 
                { 
                    RecordId = newOrder.ID,
                    Data = saveJson,
                    Description = null,
                    EventName = EventConstants.AddOrder,
                    EntityName = EntityConstants.Order,
                    Source = "Customer" 
                });

                return newOrder.ID;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating customer order: {ex.Message}");
            }
        }

    }
}
