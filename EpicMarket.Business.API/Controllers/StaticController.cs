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
    /// <summary>
    /// Provides static data and lookup values for dropdown menus and reference data
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        /// Retrieves all available business category options
        /// </summary>
        /// <returns>List of business categories for dropdown selection</returns>
        /// <response code="200">Returns list of business categories</response>
        /// <remarks>
        /// Categories include:
        /// - Retail
        /// - Restaurant
        /// - Service
        /// - Manufacturing
        /// - Technology
        /// - Healthcare
        /// - Education
        /// </remarks>
        [HttpGet("GetBusinessCategoriesOptions")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Retrieves available status options based on user permissions
        /// </summary>
        /// <returns>List of status options</returns>
        /// <response code="200">Returns list of status options</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User lacks required permissions</response>
        /// <remarks>
        /// Status options include:
        /// - Active
        /// - Inactive
        /// - Pending
        /// - Approved
        /// - Rejected
        /// - Under Review
        /// - Suspended
        /// </remarks>
        [HttpGet("GetStatusOptions")]
        [CustomAuthorize(Securable = Securables.STATUS_OPTIONS)]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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
        /// Retrieves all order status options
        /// </summary>
        /// <returns>List of order status options</returns>
        /// <response code="200">Returns list of order statuses</response>
        /// <remarks>
        /// Order statuses include:
        /// - New
        /// - Confirmed
        /// - Processing
        /// - Ready
        /// - Shipped
        /// - Delivered
        /// - Cancelled
        /// - Refunded
        /// </remarks>
        [HttpGet("GetOderStatusOptions")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Retrieves all order type options
        /// </summary>
        /// <returns>List of order types</returns>
        /// <response code="200">Returns list of order types</response>
        /// <remarks>
        /// Order types include:
        /// - Delivery
        /// - Pickup
        /// - Dine-in
        /// - Takeaway
        /// - Catering
        /// - Subscription
        /// </remarks>
        [HttpGet("GetOderTypeOptions")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Retrieves all blog category options
        /// </summary>
        /// <returns>List of blog categories</returns>
        /// <response code="200">Returns list of blog categories</response>
        /// <remarks>
        /// Blog categories include:
        /// - Business Tips
        /// - Industry News
        /// - Product Updates
        /// - Success Stories
        /// - Marketing
        /// - Technology
        /// - Tutorials
        /// </remarks>
        [HttpGet("GetAllblogCategories")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Retrieves all support ticket categories
        /// </summary>
        /// <returns>List of support categories</returns>
        /// <response code="200">Returns list of support categories</response>
        /// <remarks>
        /// Support categories include:
        /// - Technical Issue
        /// - Billing Question
        /// - Account Problem
        /// - Feature Request
        /// - General Inquiry
        /// - Bug Report
        /// - Complaint
        /// </remarks>
        [HttpGet("GetAllSupportCategorys")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Subscribes an email address to receive promotional offers
        /// </summary>
        /// <param name="gmail">Email address to subscribe</param>
        /// <returns>Subscription ID</returns>
        /// <response code="200">Successfully subscribed</response>
        /// <response code="400">Invalid email format or already subscribed</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/static/subscribeforOffers?gmail=user@example.com
        /// 
        /// Subscribers will receive:
        /// - Promotional offers
        /// - New feature announcements
        /// - Newsletter updates
        /// </remarks>
        [HttpPost("subscribeforOffers")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<OperationResult<int>>> SubscribeforOffers( string gmail)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Static Controller -> UpdateStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = gmail }));

            var id = await staticService.SubscribeforOffers(gmail);

            this.logger.LogInformation("Static Controller -> UpdateStatus()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves contextual help items for a specific page
        /// </summary>
        /// <param name="pagename">Name of the page requesting help items</param>
        /// <returns>List of relevant help items for the page</returns>
        /// <response code="200">Returns help items</response>
        /// <remarks>
        /// Page names include:
        /// - dashboard
        /// - products
        /// - orders
        /// - employees
        /// - settings
        /// - reports
        /// 
        /// Each help item includes:
        /// - Title
        /// - Description
        /// - Help text
        /// - Related links
        /// - Video tutorials (if available)
        /// </remarks>
        [HttpGet("GetHelpItemsforBypage")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<HelpItemDTO>>), 200)]
        public async Task<ActionResult<OperationResult<List<HelpItemDTO>>>> GetHelpItemsforBypage(string pagename)
        {
            var reponse = new OperationResult<List<HelpItemDTO>>();

            this.logger.LogInformation("Static Controller -> GetHelpItemsforBypage()");

            var options = await staticService.GetHelpItemsforBypage(pagename);
            this.logger.LogInformation("Static Controller-> GetHelpItemsforBypage()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            reponse.Data = options;
            return Ok(reponse);
        }
        /// <summary>
        /// Retrieves support query types based on person type
        /// </summary>
        /// <param name="personTypeId">ID of the person type (1=Customer, 2=Business, 3=Employee)</param>
        /// <returns>List of relevant support query types</returns>
        /// <response code="200">Returns support query types</response>
        /// <remarks>
        /// Person types:
        /// - 1: Customer queries (orders, refunds, delivery)
        /// - 2: Business queries (verification, payments, features)
        /// - 3: Employee queries (access, training, technical)
        /// </remarks>
        [HttpGet("GetAllSupportQuery")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Retrieves all person type options
        /// </summary>
        /// <returns>List of person types</returns>
        /// <response code="200">Returns list of person types</response>
        /// <remarks>
        /// Person types include:
        /// - Customer
        /// - Business Owner
        /// - Employee
        /// - Administrator
        /// - Support Staff
        /// - Vendor
        /// </remarks>
        [HttpGet("GetAllPersonTypes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
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
        /// Retrieves available proof document types
        /// </summary>
        /// <returns>List of proof types for identity verification</returns>
        /// <response code="200">Returns list of proof types</response>
        /// <remarks>
        /// Currently supported proof types:
        /// - PAN (Permanent Account Number)
        /// - Aadhaar (Indian national ID)
        /// 
        /// These are used for business and identity verification.
        /// </remarks>
        [HttpGet("proofType")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<List<DropDownOptions>>), 200)]
        public ActionResult<OperationResult<List<DropDownOptions>>> GetProofTypes()
        {
            var response = new OperationResult<List<DropDownOptions>>();
            this.logger.LogInformation("Static Controller -> GetProofTypes()");

            var options = new List<DropDownOptions>
            {
                 new DropDownOptions { Value = 1, Text = "PAN" },
                 new DropDownOptions { Value = 2, Text = "Aadhaar" }
            };

            this.logger.LogInformation("Static Controller-> GetProofTypes()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = options }));

            response.Data = options;
            return Ok(response);
        }

    }
}
