using EpicMarket.Contracts;
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

        public StaticController(ILogger<StaticController> logger, IBusinessService businessService , IStaticService staticService)
        {
            this.logger = logger;
            this.businessService = businessService;
            this.staticService = staticService;
        }

        [HttpGet("GetBusinessCategoriesOptions")]
        [Authorize]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetBusinessCategoriesOptions()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetBusinessCategoriesOptions()");

            var options = await staticService.BusinessCategoriesOptions();
            this.logger.LogInformation("Static Controller-> GetBusinessCategoriesOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;


			return Ok(reponse);
        }
    }
}
