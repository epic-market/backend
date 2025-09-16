using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Manages order operations including creation, tracking, and status updates
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : BaseApiController
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IOrderService orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.orderService = orderService;
        }

       
        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="ordersDto">Order information including customer details, items, and delivery information</param>
        /// <returns>The ID of the created order</returns>
        /// <response code="200">Order successfully created</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized (not a business owner or employee)</response>
        /// <response code="400">Invalid order data</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/orders/AddOrder
        ///     {
        ///        "customerId": 123,
        ///        "customerName": "John Doe",
        ///        "customerPhone": "+1234567890",
        ///        "deliveryAddress": "123 Main St",
        ///        "orderType": "Delivery",
        ///        "items": [
        ///            {
        ///                "productId": 1,
        ///                "productName": "Product A",
        ///                "quantity": 2,
        ///                "unitPrice": 19.99
        ///            }
        ///        ],
        ///        "totalAmount": 39.98,
        ///        "paymentMethod": "Card",
        ///        "notes": "Please deliver before 5 PM"
        ///     }
        /// </remarks>
        [HttpPost("AddOrder")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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
        /// Retrieves details of a specific order
        /// </summary>
        /// <param name="OrderId">The unique identifier of the order</param>
        /// <returns>Complete order details including items and status history</returns>
        /// <response code="200">Returns order details</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        /// <response code="404">Order not found</response>
        /// <remarks>
        /// Returns comprehensive order information including:
        /// - Customer details
        /// - Order items with pricing
        /// - Delivery/pickup information
        /// - Payment status
        /// - Order status history
        /// - Associated branch and employee information
        /// </remarks>
        [HttpGet("GetSingleOrder")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<OrdersDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<OrdersDto>>> GetSingleOrder(int OrderId)
        {
            var response = new OperationResult<OrdersDto>();

            this.logger.LogInformation("Orders Controller -> GetSingleOrder()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var orders = await orderService.GetSingleOrder(OrderId);

            this.logger.LogInformation("Orders Controller -> GetSingleOrder()-> return {0}", JsonConvert.SerializeObject(new { Value = orders }));

            response.Data = orders;

            return Ok(response);
        }


        /// <summary>
        /// Updates the status of an order
        /// </summary>
        /// <param name="OrderId">The unique identifier of the order</param>
        /// <param name="OrderStatus">New status for the order (e.g., "Processing", "Shipped", "Delivered")</param>
        /// <returns>The updated order ID</returns>
        /// <response code="200">Order status successfully updated</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        /// <response code="404">Order not found</response>
        /// <response code="400">Invalid status transition</response>
        /// <remarks>
        /// Valid status transitions:
        /// - Pending → Processing
        /// - Processing → Shipped
        /// - Shipped → Delivered
        /// - Any status → Cancelled (with proper authorization)
        /// 
        /// Status updates trigger customer notifications.
        /// </remarks>
        [HttpPost("UpdateStatus")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<int>>> UpdateStatus(int OrderId, string OrderStatus)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Orders Controller -> UpdateStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var id =  await orderService.UpdateStatus(OrderId, OrderStatus);

            this.logger.LogInformation("Orders Controller -> UpdateStatus()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves available order status options
        /// </summary>
        /// <returns>List of order status options for dropdown menus</returns>
        /// <response code="200">Returns list of status options</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        /// <remarks>
        /// Returns all possible order statuses including:
        /// - Pending
        /// - Processing
        /// - Confirmed
        /// - Preparing
        /// - Ready
        /// - Shipped
        /// - Out for Delivery
        /// - Delivered
        /// - Cancelled
        /// - Refunded
        /// </remarks>
        [HttpGet("GetOrderStatusOptions")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetOrderStatusOptions()
        {
            var response = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Orders Controller -> GetOrderStatusOptions()");

            var id = await orderService.GetOrderStatusOptions();

            this.logger.LogInformation("Orders Controller -> GetOrderStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }


        /// <summary>
        /// Retrieves all orders for the business with filtering and pagination
        /// </summary>
        /// <param name="orderParams">Filter parameters including date range, status, customer, and pagination</param>
        /// <returns>Paginated list of orders</returns>
        /// <response code="200">Returns paginated order list</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/orders/GetAllOrders
        ///     {
        ///        "pageNumber": 1,
        ///        "pageSize": 20,
        ///        "status": "Processing",
        ///        "fromDate": "2024-01-01",
        ///        "toDate": "2024-01-31",
        ///        "customerSearch": "john",
        ///        "orderType": "Delivery"
        ///     }
        /// 
        /// Results are sorted by order date (newest first) by default.
        /// </remarks>
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<GetDataResult<List<OrderResult>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<OperationResult<GetDataResult<List<OrderResult>>>>> GetAllOrders([FromBody] OrderParams orderParams)
        {
            var response = new OperationResult<GetDataResult<List<OrderResult>>>();

            this.logger.LogInformation("Orders Controller -> GetAllBranches()-> params {0}", JsonConvert.SerializeObject(new { Params = orderParams }));

            var orderResults = await orderService.GetAllOrders(orderParams,this.BusinessId);

            this.logger.LogInformation("Orders Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Value = orderResults }));

            response.Data = orderResults;

            return Ok(response);
        }



    }
}
