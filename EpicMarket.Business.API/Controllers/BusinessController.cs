using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    public class BusinessController : BaseApiController
    {
        private readonly ILogger<BusinessController> logger;
        private readonly IBusinessService businessService;

        public BusinessController(ILogger<BusinessController> logger , IBusinessService businessService)
        {
            this.logger = logger;
            this.businessService = businessService;
        }


        [HttpPost("RegisterDetails")]
        [Authorize]
        public async Task<ActionResult<int>> Register(BusinessRegisterDto businessRegisterDto)
        {
            this.logger.LogInformation("Business Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = businessRegisterDto }));
            businessRegisterDto.UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value) ;
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await businessService.RegisterBusiness(businessRegisterDto, UserName);
            this.logger.LogInformation("Business Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }

    }
}
