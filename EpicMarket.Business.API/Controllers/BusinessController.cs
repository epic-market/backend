using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    public class BusinessController : BaseApiController
    {
        private readonly ILogger<BusinessController> logger;
		private readonly UserManager<AppUser> userManager;
		private readonly IBusinessService businessService;

        public BusinessController(ILogger<BusinessController> logger , UserManager<AppUser> _userManager, IBusinessService businessService, ApplicationDbContext dbContext) : base(dbContext)
		{
            this.logger = logger;
			userManager = _userManager;
			this.businessService = businessService;
        }


        [HttpPost("RegisterDetails")]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> Register(BusinessRegisterDto businessRegisterDto)
        {
            var response = new OperationResult<int>();

			this.logger.LogInformation("Business Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = businessRegisterDto }));
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value) ;
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id =  businessService.RegisterBusiness(businessRegisterDto, UserName , UserID,this.PageSource);

            var appuser =  userManager.Users.Where(c=>c.Id == UserID).FirstOrDefault();

			await userManager.AddToRoleAsync(appuser, ROLES.BUSINESS_OWNER);

            this.logger.LogInformation("Business Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

			response.Data = id;

			return Ok(response);
        }

    }
}
