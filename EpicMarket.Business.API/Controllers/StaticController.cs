using EpicMarket.Business.API.Extension;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{

    public class StaticController : BaseApiController
    {
        private readonly ILogger<StaticController> logger;
        private readonly IBusinessService businessService;
        private readonly IStaticService staticService;

        public StaticController(ILogger<StaticController> logger, IBusinessService businessService , IStaticService staticService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.businessService = businessService;
            this.staticService = staticService;
        }

        [HttpGet("GetBusinessCategoriesOptions")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetBusinessCategoriesOptions()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetBusinessCategoriesOptions()");

            var options = await staticService.BusinessCategoriesOptions();
            this.logger.LogInformation("Static Controller-> GetBusinessCategoriesOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;


			return Ok(reponse);
        }

        [HttpGet("GetStatusOptions")]
        [CustomAuthorize(Securable = Securables.STATUS_OPTIONS)]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetStatusOptions()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetStatusOptions()");

            var options = await staticService.GetStatusOptions();
            this.logger.LogInformation("Static Controller-> GetStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }

        [HttpGet("GetOderStatusOptions")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetOderStatusOptions()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetOderStatusOptions()");

            var options = await staticService.GetOderStatusOptions();
            this.logger.LogInformation("Static Controller-> GetOderStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }

        [HttpGet("GetOderTypeOptions")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetOderTypeOptions()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetOderTypeOptions()");

            var options = await staticService.GetOderTypeOptions();
            this.logger.LogInformation("Static Controller-> GetOderTypeOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }

        [HttpGet("GetAllblogCategories")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetAllblogCategories()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetAllblogCategories()");

            var options = await staticService.GetAllblogCategories();
            this.logger.LogInformation("Static Controller-> GetAllblogCategories()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }

        [HttpGet("GetAllSupportCategorys")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetAllSupportCategorys()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetAllSupportCategorys()");

            var options = await staticService.GetAllSupportCategorys();
            this.logger.LogInformation("Static Controller-> GetAllSupportCategorys()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }
        [HttpPost("subscribeforOffers")]
        [AllowAnonymous]
        public ActionResult<OperationResult<int>> SubscribeforOffers( string gmail)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Static Controller -> UpdateStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = gmail }));

            var id = staticService.SubscribeforOffers(gmail);

            this.logger.LogInformation("Static Controller -> UpdateStatus()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }

        [HttpGet("GetHelpItemsforBypage")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<HelpItemDTO>>>> GetHelpItemsforBypage(string pagename)
        {
            var reponse = new OperationResult<List<HelpItemDTO>>();

            this.logger.LogInformation("Static Controller -> GetHelpItemsforBypage()");

            var options = await staticService.GetHelpItemsforBypage(pagename);
            this.logger.LogInformation("Static Controller-> GetHelpItemsforBypage()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }
    }
}
