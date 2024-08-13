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
    public class OrdersController : BaseApiController
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IOrderService orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.orderService = orderService;
        }

       
        [HttpPost("AddOrder")]
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




        [HttpGet("GetSingleOrder")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<OrdersDto>>> GetSingleOrder(int OrderId)
        {
            var response = new OperationResult<OrdersDto>();

            this.logger.LogInformation("Orders Controller -> GetSingleOrder()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var orders = await orderService.GetSingleOrder(OrderId);

            this.logger.LogInformation("Orders Controller -> GetSingleOrder()-> return {0}", JsonConvert.SerializeObject(new { Value = orders }));

            response.Data = orders;

            return Ok(response);
        }


        [HttpPost("UpdateStatus")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<int>>> UpdateStatus(int OrderId, string OrderStatus)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Orders Controller -> UpdateStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = OrderId }));

            var id =  await orderService.UpdateStatus(OrderId, OrderStatus);

            this.logger.LogInformation("Orders Controller -> UpdateStatus()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }

        [HttpGet("GetOrderStatusOptions")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetOrderStatusOptions()
        {
            var response = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Orders Controller -> GetOrderStatusOptions()");

            var id = await orderService.GetOrderStatusOptions();

            this.logger.LogInformation("Orders Controller -> GetOrderStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }


        [HttpGet("GetAllOrders")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
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
