using Amazon.Runtime.Documents;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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
    [Route("api/business")]
    public class BusinessController : BaseApiController
    {
        private readonly ILogger<BusinessController> logger;
		private readonly UserManager<AppUser> userManager;
		private readonly IBusinessService businessService;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileStoreService;
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
                                    IApplicationConfigurationService applicationConfigurationService,
                                    IConfiguration configuration
                                  ) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
			userManager = _userManager;
			this.businessService = businessService;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
            _configuration = configuration;
        }

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
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EmailTemplates");
            var emailService = new EmailService(templatePath, _configuration); // Pass IConfiguration to EmailService
           var emailModel = new 
            {
                 Business_Name = businessRegisterDto.BusinessName, // Assuming UserName is the business name
               Processing_Days = "5", // Example processing days
               Support_Email = "support@epicmarket.com", // Replace with actual support email
               Support_Phone = "123-456-7890", // Replace with actual support phone
               Current_Year = DateTime.Now.Year.ToString(),
               Company_Address = "123 Epic Market St, Epic City, EC 12345", // Replace with actual address
            };

            await emailService.SendEmailAsync("UnderReviewForBusiness", emailModel,this.LoggedInUserName , "we are reviewing your business details");

            response.Data = new BusinessDTO_Result
            {
                BusinessId = result.BusinessId,
                ProofId = result.ProofId
            };

            return CreatedAtAction(nameof(RegisterBusiness), new { result.BusinessId }, response);
        }

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

    }
}
