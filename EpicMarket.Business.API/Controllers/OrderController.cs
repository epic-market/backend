using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Order management APIs for business operators and customers.
    /// Route prefix: api/order
    /// Auth: Business owner/employee for back-office flows, authenticated customers where noted.
    /// </summary>
    [Route("api/order")]
    public class OrderController : BaseApiController
    {
        private readonly ILogger<OrderController> logger;
        private readonly IOrderService orderService;
        private readonly ApplicationDbContext dbContext;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.orderService = orderService;
            this.dbContext = dbContext;
        }

       
		/// <summary>
		/// Creates a new order from the business web application.
		/// Route: POST api/order
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="ordersDto">Order payload containing items, customer, and outlet info.</param>
		/// <returns>Identifier of the newly created order.</returns>
		[HttpPost]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<int>>> AddOrder(OrdersDto ordersDto)
		{
            var response = new OperationResult<int>();
			this.logger.LogInformation("Orders Controller -> AddOrder()-> params {0}", JsonConvert.SerializeObject(new { Params = ordersDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await orderService.CreateOrder(ordersDto , UserName,this.PageSource);
            this.logger.LogInformation("Orders Controller -> AddOrder()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
			response.Data = id;


			return Ok(response);
        }



		/// <summary>
		/// Retrieves full details for a specific order.
		/// Route: GET api/order/{OrderId}
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="OrderId">The order identifier.</param>
		/// <returns>Order detail DTO including line items and history.</returns>
		[HttpGet("{OrderId}")]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<OrdersDetailsResult>>> GetSingleOrder(int OrderId)
		{
            var response = new OperationResult<OrdersDetailsResult>();

            this.logger.LogInformation("Orders Controller -> GetSingleOrder()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var orders = await orderService.GetSingleOrder(OrderId);

            this.logger.LogInformation("Orders Controller -> GetSingleOrder()-> return {0}", JsonConvert.SerializeObject(new { Value = orders }));

            response.Data = orders;

            return Ok(response);
        }


		/// <summary>
		/// Updates the status of an existing order.
		/// Route: PUT api/order/{OrderId}?OrderStatusId=VALUE
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="OrderId">The order identifier.</param>
		/// <param name="OrderStatusId">Target order status identifier.</param>
		/// <returns>Updated order identifier after status change.</returns>
		[HttpPut("{OrderId}")]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<int>>> UpdateStatus([FromRoute]int OrderId, [FromQuery]int OrderStatusId)
		{
            var response = new OperationResult<int>();

            this.logger.LogInformation("Orders Controller -> UpdateStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var id =  await orderService.UpdateStatus(OrderId, OrderStatusId);

            this.logger.LogInformation("Orders Controller -> UpdateStatus()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }



		/// <summary>
		/// Retrieves paginated order listings for business operators.
		/// Route: GET api/order
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="orderParams">Pagination, filtering, and sorting options.</param>
		/// <returns>Paginated list of orders.</returns>
		[HttpGet]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<GetDataResult<List<OrderResult>>>>> GetAllOrders([FromQuery]OrderParams orderParams)
		{
            var response = new OperationResult<GetDataResult<List<OrderResult>>>();

            this.logger.LogInformation("Orders Controller -> GetAllBranches()-> params {0}", JsonConvert.SerializeObject(new { Params = orderParams }));

            var orderResults = await orderService.GetAllOrders(orderParams,this.BusinessId);

            this.logger.LogInformation("Orders Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Value = orderResults }));

            response.Data = orderResults;

            return Ok(response);
        }


		/// <summary>
		/// Retrieves paginated orders optimized for mobile clients.
		/// Route: GET api/order/mobile
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="orderParams">Pagination and filter parameters for mobile view.</param>
		/// <returns>Paginated list of mobile order summaries.</returns>
		[HttpGet("Mobile")]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<GetDataResult<List<OrderMobileResult>>>>> GetAllOrdersForMobile([FromQuery] OrderParams orderParams)
		{
            var response = new OperationResult<GetDataResult<List<OrderMobileResult>>>();

            this.logger.LogInformation("Orders Controller -> GetAllOrdersForMobile()-> params {0}", JsonConvert.SerializeObject(new { Params = orderParams }));

            var orderResults = await orderService.GetAllOrdersForMobile(orderParams, this.BusinessId);

            this.logger.LogInformation("Orders Controller -> GetAllOrdersForMobile()-> return {0}", JsonConvert.SerializeObject(new { Value = orderResults }));

            response.Data = orderResults;
            return Ok(response);
        }

		/// <summary>
		/// Retrieves detailed order information for mobile views.
		/// Route: GET api/order/mobile/{OrderId}
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="OrderId">The order identifier.</param>
		/// <returns>Order details formatted for mobile consumption.</returns>
		[HttpGet("Mobile/{OrderId}")]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<OrderMobileDeatilsResult>>> GetOrdersDetailsForMobile(int OrderId)
		{
            var response = new OperationResult<OrderMobileDeatilsResult>();

            this.logger.LogInformation("Orders Controller -> GetOrdersDetailsForMobile()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var orderResults = await orderService.GetOrdersDetailsForMobile(OrderId);

            this.logger.LogInformation("Orders Controller -> GetOrdersDetailsForMobile()-> return {0}", JsonConvert.SerializeObject(new { Value = orderResults }));

            response.Data = orderResults;

            return Ok(response);
        }


		/// <summary>
		/// Checks for new orders since a timestamp, optionally filtered by outlet.
		/// Route: GET api/order/new
		/// Auth: Business owner or employee.
		/// </summary>
		/// <param name="ordered_after">UTC timestamp to check for new orders after.</param>
		/// <param name="outlet_id">Optional outlet identifier filter.</param>
		/// <returns>Boolean flag indicating if new orders exist.</returns>
		[HttpGet("new")]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<GetDataResult<List<OrderMobileResult>>>>> GetNewOrders(DateTime ordered_after , int ? outlet_id)
		{
            var response = new OperationResult<object>();

            this.logger.LogInformation("Orders Controller -> GetOrdersDetailsForMobile()-> params {0}", JsonConvert.SerializeObject(new { ordered_after = ordered_after , outlet_id = outlet_id }));

            var orderResults = await orderService.AnyNewOrders(ordered_after,this.BusinessId, outlet_id);

            this.logger.LogInformation("Orders Controller -> GetOrdersDetailsForMobile()-> return {0}", JsonConvert.SerializeObject(new { Value = orderResults }));

            response.Data = new { newOrders = orderResults };

            if (orderResults)
            {
                return Ok(response);
            }
            else { 
                return NotFound(response);
            }

        }



		/// <summary>
		/// Retrieves paginated order history for the logged-in customer.
		/// Route: GET api/order/customer/orderhistory
		/// Auth: Implicit; relies on current user context.
		/// </summary>
		/// <param name="request">Pagination and sorting parameters.</param>
		/// <returns>Paginated order history for the current customer.</returns>
		[HttpGet("Customer/OrderHistory")]
		public async Task<ActionResult<GetDataResult<CustomerOrderDto>>> GetOrderHistory([FromQuery] OrderHistoryRequest request)
		{
            var UserID = dbContext.Users.Where(c => c.UserName == this.LoggedInUserName).FirstOrDefault().Id;
            if (request.Page < 1)
                return BadRequest("Page number must be greater than 0");

            if (request.PageSize < 1)
                return BadRequest("Page size must be greater than 0");

            // Validate sort parameters
            var validSortFields = new[] { "date", "amount", "status" };
            var validSortOrders = new[] { "asc", "desc" };

            if (!string.IsNullOrEmpty(request.SortBy) && !validSortFields.Contains(request.SortBy.ToLower()))
                return BadRequest($"Invalid sort field. Valid values are: {string.Join(", ", validSortFields)}");

            if (!string.IsNullOrEmpty(request.SortOrder) && !validSortOrders.Contains(request.SortOrder.ToLower()))
                return BadRequest($"Invalid sort order. Valid values are: {string.Join(", ", validSortOrders)}");

            var result = await orderService.GetCustomerOrderHistoryAsync(UserID, request);

            return Ok(result);
        }

		/// <summary>
		/// Creates an order on behalf of the currently authenticated customer.
		/// Route: POST api/order/customer
		/// Auth: Any authenticated user.
		/// </summary>
		/// <param name="ordersDto">Order payload with cart items and delivery info.</param>
		/// <returns>Identifier of the created customer order.</returns>
		[HttpPost("customer")]
		[Authorize] // This ensures only logged-in customers can create orders
		public async Task<ActionResult<OperationResult<int>>> CreateCustomerOrder(CreateCustomerOrderDto ordersDto)
		{
            var response = new OperationResult<int>();
            
            try
            {
                this.logger.LogInformation("Orders Controller -> CreateCustomerOrder()-> params {0}", 
                    JsonConvert.SerializeObject(new { Params = ordersDto }));

                // Get the current user's ID
                var userId = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
                var id = await orderService.CreateCustomerOrder(ordersDto, userId);
                
                this.logger.LogInformation("Orders Controller -> CreateCustomerOrder()-> return {0}", 
                    JsonConvert.SerializeObject(new { Value = id }));
                
                response.Data = id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }

}
