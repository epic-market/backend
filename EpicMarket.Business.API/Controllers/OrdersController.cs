using EpicMarket.Contracts;
using EpicMarket.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IOrderService orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            this.logger = logger;
            this.orderService = orderService;
        }
        [HttpPost("AddOrder")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> AddOrder(OrdersDto ordersDto)
        {
            this.logger.LogInformation("Orders Controller -> AddOrder()-> params {0}", JsonConvert.SerializeObject(new { Params = ordersDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await orderService.CreateOrder(ordersDto , UserName);
            this.logger.LogInformation("Orders Controller -> AddOrder()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }


    }
}
