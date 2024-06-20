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

        public StaticController(ILogger<StaticController> logger, IBusinessService businessService , IStaticService staticService, ApplicationDbContext dbContext) : base(dbContext)
		{
            this.logger = logger;
            this.businessService = businessService;
            this.staticService = staticService;
        }

        [HttpGet("GetBusinessCategoriesOptions")]
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
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetAllblogCategories()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetAllblogCategories()");

            var options = await staticService.GetAllblogCategories();
            this.logger.LogInformation("Static Controller-> GetAllblogCategories()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }
    }
}
