using EpicMarket.Business.API.Extension;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Supplies static reference data used across the application such as dropdown options and metadata.
    /// Includes endpoints for public and secured lookup values.
    /// </summary>
    [Route("api/static")]
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

        /// <summary>
        /// Retrieves available business categories for dropdown selections.
        /// Route: GET api/static/business-categories-options
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of business category options.</returns>
        [HttpGet("business-categories-options")]
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

        /// <summary>
        /// Retrieves status options secured by the STATUS_OPTIONS permission.
        /// Route: GET api/static/GetStatusOptions
        /// Auth: CustomAuthorize
        /// </summary>
        /// <returns>List of status dropdown options.</returns>
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

        /// <summary>
        /// Provides the available order status options for public consumers.
        /// Route: GET api/static/GetOderStatusOptions
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of order status options.</returns>
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

        /// <summary>
        /// Provides the available order type options for public consumers.
        /// Route: GET api/static/GetOderTypeOptions
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of order type options.</returns>
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

        /// <summary>
        /// Retrieves all blog categories for public-facing pages.
        /// Route: GET api/static/GetAllblogCategories
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of blog categories.</returns>
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

        /// <summary>
        /// Retrieves all support categories for public support forms.
        /// Route: GET api/static/GetAllSupportCategorys
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of support categories.</returns>
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

        /// <summary>
        /// Subscribes a user email to offer notifications.
        /// Route: POST api/static/subscribe
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="gmail">Email address to subscribe for offers.</param>
        /// <returns>Identifier of the subscription record.</returns>
        [HttpPost("subscribe")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<int>>> SubscribeforOffers(string gmail)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Static Controller -> UpdateStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = gmail }));

            var id = await staticService.SubscribeforOffers(gmail);

            this.logger.LogInformation("Static Controller -> UpdateStatus()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves help items associated with a specific page link.
        /// Route: GET api/static/GetHelpItemsforBypage
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="pagelink">Unique identifier for the help page.</param>
        /// <returns>List of help items for the given page.</returns>
        [HttpGet("GetHelpItemsforBypage")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<HelpItemDTO>>>> GetHelpItemsforBypage(string pagelink)
        {
            var reponse = new OperationResult<List<HelpItemDTO>>();

            this.logger.LogInformation("Static Controller -> GetHelpItemsforBypage()");

            var options = await staticService.GetHelpItemsforBypage(pagelink);
            this.logger.LogInformation("Static Controller-> GetHelpItemsforBypage()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }

        /// <summary>
        /// Retrieves support query options filtered by person type.
        /// Route: GET api/static/GetAllSupportQuery
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="personTypeId">Person type identifier to filter support queries.</param>
        /// <returns>List of support query dropdown options.</returns>
        [HttpGet("GetAllSupportQuery")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetAllSupportQuery(int personTypeId)
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetAllSupportQuery()");

            var options = await staticService.GetAllSupportQuery(personTypeId);
            this.logger.LogInformation("Static Controller-> GetAllSupportQuery()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }

        /// <summary>
        /// Retrieves person type options for selection controls.
        /// Route: GET api/static/GetAllPersonTypes
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of person type dropdown options.</returns>
        [HttpGet("GetAllPersonTypes")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetAllPersonTypes()
        {
            var reponse = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Static Controller -> GetAllPersonTypes()");

            var options = await staticService.GetAllPersonTypes();
            this.logger.LogInformation("Static Controller-> GetAllPersonTypes()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }



        /// <summary>
        /// Retrieves order status options for authenticated business users.
        /// Route: GET api/static/GetOrderStatusOptions
        /// Auth: Authorize (Business roles)
        /// </summary>
        /// <returns>List of order status dropdown options.</returns>
        [HttpGet("GetOrderStatusOptions")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetOrderStatusOptions()
        {
            var response = new OperationResult<List<DropDownOptions>>();

            this.logger.LogInformation("Orders Controller -> GetOrderStatusOptions()");

            var id = await staticService.GetOrderStatusOptions();

            this.logger.LogInformation("Orders Controller -> GetOrderStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves available proof types for identity or verification flows.
        /// Route: GET api/static/proof-types
        /// Auth: AllowAnonymous
        /// </summary>
        /// <returns>List of proof type dropdown options.</returns>
        [HttpGet("proof-types")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<DropDownOptions>>>> GetProofTypes()
        {
            var response = new OperationResult<List<DropDownOptions>>();
            this.logger.LogInformation("Static Controller -> GetProofTypes()");
            var options = await staticService.GetProofTypes();
            this.logger.LogInformation("Static Controller-> GetProofTypes()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));
            response.Data = options;
            return Ok(response);
        }

        

        

    


    }
}
