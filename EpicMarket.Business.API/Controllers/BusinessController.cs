using Amazon.Runtime.Documents;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Business management API. Handles registration, profile updates, category discovery,
    /// and public business listings for the business portal.
    /// </summary>
    /// <remarks>
    /// Route prefix: <c>api/business</c>
    /// </remarks>
    [Route("api/business")]
    public class BusinessController : BaseApiController
    {
        private readonly ILogger<BusinessController> logger;
		private readonly UserManager<AppUser> userManager;
		private readonly IBusinessService businessService;
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileStoreService;
        private readonly ICommunicationService communicationService;
        private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly IConfiguration _configuration;

        public BusinessController(
                                    ILogger<BusinessController> logger,
                                    UserManager<AppUser> _userManager,
                                    IBusinessService businessService,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IAttachmentService attachmentService,
                                    IFileService fileStoreService,
                                    ICommunicationService communicationService,
                                    IApplicationConfigurationService applicationConfigurationService,
                                    IConfiguration configuration
                                  ) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
			userManager = _userManager;
			this.businessService = businessService;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.communicationService = communicationService;
            this.applicationConfigurationService = applicationConfigurationService;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers the current authenticated user as a business owner and creates a business record.
        /// </summary>
        /// <remarks>
        /// Route: <c>POST api/business</c>
        /// Auth: <c>Authorize</c>
        /// Body: multipart form-data containing <see cref="BusinessRegisterDto"/>.
        /// </remarks>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OperationResult<BusinessDTO_Result>>> RegisterBusiness([FromForm] BusinessRegisterDto businessRegisterDto)
        {
            // Check for null reference cases
            if (businessRegisterDto == null)
            {
                this.logger.LogError("BusinessRegisterDto is null");
                return BadRequest("Invalid request");
            }

            var response = new OperationResult<BusinessDTO_Result>();

            this.logger.LogInformation("Business Controller -> RegisterBusiness()-> params {0}", JsonConvert.SerializeObject(new { Params = businessRegisterDto }));

            // Safely parse UserID from claims
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var result = await businessService.RegisterBusiness(businessRegisterDto, UserName, this.AdminPersonID, UserID, this.PageSource) as BusinessDTO_Result;

            // Check if business registration was successful
            if (result == null)
            {
                this.logger.LogError("Failed to register business");
                return BadRequest("Failed to register business");
            }

            // Add user to BUSINESS_OWNER role
            var appuser = userManager.Users.Where(c => c.Id == UserID).FirstOrDefault();
            if (appuser == null)
            {
                this.logger.LogError("User not found for ID: {0}", UserID);
                return BadRequest("User not found");
            }
            await userManager.AddToRoleAsync(appuser, ROLES.BUSINESS_OWNER);

            this.logger.LogInformation("Business Controller -> RegisterBusiness()-> return {0}", JsonConvert.SerializeObject(new { Value = result }));

            // Handle logo file upload
            if (businessRegisterDto.LogoFile?.Length > 0)
            {
                var filinsertOutput = await this.SaveFileBusinessAsync(businessRegisterDto.LogoFile, this.fileStoreService, this.applicationConfigurationService, result.BusinessId);
                if (filinsertOutput == null)
                {
                    this.logger.LogError("Failed to save logo file");
                    return BadRequest("Failed to save logo file");
                }
                 var attachmentId = await this.attachmentService.GetAttachmentId(filinsertOutput.Key);
                await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentTypeName = AttachmentTypeConstants.LOGO,
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Business,
                    RecordID = result.BusinessId
                }, result.BusinessId);
            }

            // Handle proof files upload
            if (businessRegisterDto.ProofFile?.Length > 0)
            {
                foreach (var proof in businessRegisterDto.ProofFile)
                {
                    var filinsertOutput = await this.SaveFileBusinessAsync( proof , this.fileStoreService  , this.applicationConfigurationService, result.BusinessId);
                    if (filinsertOutput == null)
                    {
                        this.logger.LogError("Failed to save proof file");
                        return BadRequest("Failed to save proof file");
                    }
                    var attachmentId = await this.attachmentService.GetAttachmentId(filinsertOutput.Key);
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.PROOF,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Proof,
                        RecordID = result.ProofId
                    }, result.BusinessId);
                }
            }
            var emailModel = EmailModel.GetUnderReviewForBusinessModel(businessRegisterDto.BusinessName);

            await communicationService.SendTemplatedEmailAsync(
                this.LoggedInUserName,
                EmailSubjectConstants.UnderReviewForBusiness,
                EmailTemplateConstants.UnderReviewForBusiness,
                emailModel
            );

            response.Data = new BusinessDTO_Result
            {
                BusinessId = result.BusinessId,
                ProofId = result.ProofId
            };

            return CreatedAtAction(nameof(RegisterBusiness), new { result.BusinessId }, response);
        }

        /// <summary>
        /// Retrieves the business details associated with the authenticated user.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/business</c>
        /// Auth: <c>Authorize</c>
        /// </remarks>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OperationResult<BusinessDetailResult>>> GetBusinessByID()
        {
            // Initialize the response object
            var response = new OperationResult<BusinessDetailResult>();

            // Log the request parameters
            this.logger.LogInformation("Business Controller -> GetBusinessByID()-> params {0}", JsonConvert.SerializeObject(new { Params = new { businessId = this.BusinessId } }));

            // Attempt to retrieve the business details by ID
            var results = await businessService.GetBusinessByID(this.BusinessId);

            // Check if the result is null and log the error if so
            if (results == null)
            {
                this.logger.LogError("Failed to retrieve business details for ID {0}", this.BusinessId);
                return NotFound("Business details not found");
            }

            // Log the successful retrieval of business details
            this.logger.LogInformation("Business Controller -> GetBusinessByID()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            // Set the response data
            response.Data = results;

            // Return the response
            return Ok(response);
        }

        /// <summary>
        /// Updates the business profile information for the authenticated business owner.
        /// </summary>
        /// <remarks>
        /// Route: <c>PUT api/business</c>
        /// Auth: <c>Authorize</c>
        /// Body: JSON <see cref="UpdateBusinessRegisterDto"/>.
        /// </remarks>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> UpdateBusiness( [FromBody] UpdateBusinessRegisterDto businessRegisterDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Business Controller -> UpdateBusiness()-> params {0}", JsonConvert.SerializeObject(new { Params = businessRegisterDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var branchID = await businessService.UpdateBusiness(this.BusinessId, businessRegisterDto, UserName, this.AdminPersonID,this.PageSource);
            if (businessRegisterDto.LogoFile?.Length > 0)
            {
                var attachmentId = await this.attachmentService.GetAttachmentId(businessRegisterDto.LogoFile);
                await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentTypeName = AttachmentTypeConstants.LOGO,
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Business,
                    RecordID = this.BusinessId
                }, this.BusinessId);
            }
            if (businessRegisterDto.ProofFile?.Length > 0)
            {
                foreach (var proof in businessRegisterDto.ProofFile)
                {
                    var attachmentId = await this.attachmentService.GetAttachmentId(proof);
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.PROOF,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Business,
                        RecordID = this.BusinessId
                    }, this.BusinessId);
                }
            }

            this.logger.LogInformation("Business Controller -> UpdateBusiness()-> return {0}", JsonConvert.SerializeObject(new { Value = this.BusinessId }));
            response.Data = this.BusinessId;
            return Ok(response);
        }

        /// <summary>
        /// Gets all available business categories with their details and image.
        /// </summary>
        /// <summary>
        /// Gets all available business categories with their details and cover image.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/business/categories</c>
        /// Auth: <c>AllowAnonymous</c>
        /// </remarks>
        [HttpGet("categories")]
        public async Task<ActionResult<OperationResult<List<BusinessCategoryDto>>>> GetBusinessCategories()
        {
            var response = new OperationResult<List<BusinessCategoryDto>>();
            
            this.logger.LogInformation("Business Controller -> GetBusinessCategories() -> Retrieving categories");
            
            try
            {
                var categories = await businessService.GetBusinessCategories();
                
                if (categories == null || !categories.Any())
                {
                    this.logger.LogWarning("No business categories found");
                    return NotFound("No business categories available");
                }
                
                this.logger.LogInformation("Business Controller -> GetBusinessCategories() -> Found {0} categories", categories.Count);
                
                response.Data = categories;
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving business categories");
                return StatusCode(500, "An error occurred while retrieving business categories");
            }
        }

        /// <summary>
        /// Gets business listings grouped by type (Trending, New, Featured)
        /// </summary>
        /// <param name="category">Optional category filter</param>
        /// <summary>
        /// Gets curated business listings grouped by type (Trending, New, Featured).
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/business/listings</c>
        /// Query: optional <c>category</c> filter.
        /// Auth: <c>AllowAnonymous</c>
        /// </remarks>
        [HttpGet("listings")]
        public async Task<ActionResult<OperationResult<BusinessGroupsResponseDto>>> GetBusinessListings([FromQuery] string category = null)
        {
            var response = new OperationResult<BusinessGroupsResponseDto>();
            
            this.logger.LogInformation("Business Controller -> GetBusinessListings() -> Retrieving business listings, Category Filter: {0}", category ?? "None");
            
            try
            {
                var listings = await businessService.GetBusinessListings(category);
                
                if (listings == null || listings.BusinessGroups == null || !listings.BusinessGroups.Any())
                {
                    this.logger.LogWarning("No business listings found for category: {0}", category ?? "None");
                    return NotFound("No business listings available");
                }
                
                this.logger.LogInformation("Business Controller -> GetBusinessListings() -> Found {0} business groups", listings.BusinessGroups.Count);
                
                response.Data = listings;
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving business listings for category: {0}", category ?? "None");
                return StatusCode(500, "An error occurred while retrieving business listings");
            }
        }
    }
}
