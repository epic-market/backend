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
using System;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{   
    [Route("api/orders")]
    public class OrdersController : BaseApiController
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IOrderService orderService;
        private readonly ApplicationDbContext dbContext;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.orderService = orderService;
            this.dbContext = dbContext;
        }

       
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
