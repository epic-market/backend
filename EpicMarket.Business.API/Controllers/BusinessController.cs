using Amazon.Runtime.Documents;
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
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileStoreService;
        private readonly IApplicationConfigurationService applicationConfigurationService;

        public BusinessController(
                                    ILogger<BusinessController> logger,
                                    UserManager<AppUser> _userManager,
                                    IBusinessService businessService,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IAttachmentService attachmentService,
                                    IFileService fileStoreService,
                                    IApplicationConfigurationService applicationConfigurationService
                                  ) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
			userManager = _userManager;
			this.businessService = businessService;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
        }


        [HttpPost("RegisterDetails")]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> Register([FromForm] BusinessRegisterDto businessRegisterDto)
        {
            var response = new OperationResult<int>();

			this.logger.LogInformation("Business Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = businessRegisterDto }));
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value) ;
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id =  businessService.RegisterBusiness(businessRegisterDto, UserName , UserID,this.PageSource);

            var appuser =  userManager.Users.Where(c=>c.Id == UserID).FirstOrDefault();

			await userManager.AddToRoleAsync(appuser, ROLES.BUSINESS_OWNER);

            this.logger.LogInformation("Business Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            if (businessRegisterDto.LOGOFile.Length>0)
            {
                var filinsertOutput = this.SaveFileGlobalAsync(businessRegisterDto.LOGOFile, EntityConstants.Business, this.fileStoreService, this.applicationConfigurationService, id).Result;
                var attachmentId = this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                {
                    AttachmentTypeName = AttachmentTypeConstants.LOGO,
                    Name = EntityConstants.Business + AttachmentTypeConstants.LOGO,
                    Comment = null,
                    DocumentType = DocumentTypeConstants.FILE,
                    DocumentFileType = businessRegisterDto.LOGOFile.ContentType,
                    DocumentFolderPath = filinsertOutput.FullPathLocation,
                    DocumentFile = filinsertOutput.FileName,
                });
                this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Business,
                    RecordID = id
                });
            }
            if (businessRegisterDto.ProofFile.Length>0)
            {
                var filinsertOutput = this.SaveFileGlobalAsync(businessRegisterDto.ProofFile, EntityConstants.Business, this.fileStoreService, this.applicationConfigurationService, id).Result;
                var attachmentId = this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                {
                    AttachmentTypeName = AttachmentTypeConstants.PROOF,
                    Name = EntityConstants.Business + AttachmentTypeConstants.PROOF,
                    Comment = null,
                    DocumentType = DocumentTypeConstants.FILE,
                    DocumentFileType = businessRegisterDto.ProofFile.ContentType,
                    DocumentFolderPath = filinsertOutput.FullPathLocation,
                    DocumentFile = filinsertOutput.FileName,
                });
                this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Business,
                    RecordID = id
                });
            }
            response.Data = id;


			return Ok(response);
        }

    }
}
