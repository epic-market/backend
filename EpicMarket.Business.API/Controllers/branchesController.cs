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

    [Authorize]
    public class branchesController : BaseApiController
    {

        private readonly ILogger<branchesController> logger;
        private readonly IBranchService branchService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileStoreService;
        private readonly IApplicationConfigurationService applicationConfigurationService;

        public branchesController(ILogger<branchesController> logger, IBranchService branchService, ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor,IAttachmentService attachmentService,IFileService fileStoreService,IApplicationConfigurationService applicationConfigurationService) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.branchService = branchService;
            this.httpContextAccessor = httpContextAccessor;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
        }

        [HttpGet]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<GetDataResult<List<BranchResult>>>>> GetAllBranches([FromQuery]BranchParams branchParams)
        {
            var response = new OperationResult<GetDataResult<List<BranchResult>>>();

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> params {0}", JsonConvert.SerializeObject(new { Params = branchParams }));

            var results = await branchService.GetAllBranches(branchParams, this.BusinessId);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

			response.Data = results;

			return Ok(response);
        }


        [HttpGet("DropDown")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<List<DropDownOptions>>> GetAllOutletsForDropDown()
        {
            var response = new OperationResult<List<DropDownOptions>> ();

            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> params {0}", JsonConvert.SerializeObject(new { Params = UserID }));



            var results = await branchService.GetAllOutletsForDropDown(UserID, this.BusinessId);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        [HttpGet("{branchId}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<BranchDetailResult>>> GetBranchByID(int branchId)
        {
            var response = new OperationResult<BranchDetailResult>();

            this.logger.LogInformation("Branch Controller -> GetBranchByID()-> params {0}", JsonConvert.SerializeObject(new { Params = new { branchID = branchId } }));

            var results = await branchService.GetBranchByID(branchId);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        [HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> AddBranch([FromForm]BranchDto branchDto)
        {
            var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id =  await branchService.AddBranch(branchDto, UserName, this.BusinessId,this.PageSource);
            if (branchDto.Photos?.Length > 0)
            {
                foreach (var photo in branchDto.Photos)
                {
                    var filinsertOutput = await this.SaveFileGlobalAsync(photo, ApplicationConfigurationConstants.BranchesPhotos, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
                    var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                    {
                        Name = EntityConstants.Branch + AttachmentTypeConstants.BRANCH_PHOTOS,
                        Comment = null,
                        DocumentType = DocumentTypeConstants.FILE,
                        DocumentFileType = photo.ContentType,
                        DocumentFolderPath = filinsertOutput.FullPathLocation,
                        DocumentFile = filinsertOutput.FileName,
                    });
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.BRANCH_PHOTOS,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Branch,
                        RecordID = id
                    });
                }

 
            }

            if (branchDto.Thumbnail != null)
            {
                var filinsertOutput = await this.SaveFileGlobalAsync(branchDto.Thumbnail, ApplicationConfigurationConstants.BranchThumbnail, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
                var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                {
                    Name = EntityConstants.Branch + AttachmentTypeConstants.BRANCH_THUMBNAIL,
                    Comment = null,
                    DocumentType = DocumentTypeConstants.FILE,
                    DocumentFileType = branchDto.Thumbnail.ContentType,
                    DocumentFolderPath = filinsertOutput.FullPathLocation,
                    DocumentFile = filinsertOutput.FileName,
                });
                await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentTypeName = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Branch,
                    RecordID = id
                });

            }


            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
			return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> UpdateBranch(int id, [FromForm]BranchDto branchDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var branchID = await branchService.UpdateBranch(id,branchDto, UserName, this.BusinessId, this.PageSource);
            if (branchDto.Photos?.Length > 0)
            {
                foreach (var photo in branchDto.Photos)
                {
                    var filinsertOutput = await this.SaveFileGlobalAsync(photo, ApplicationConfigurationConstants.BranchesPhotos, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
                    var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                    {
                        Name = EntityConstants.Branch + AttachmentTypeConstants.BRANCH_PHOTOS,
                        Comment = null,
                        DocumentType = DocumentTypeConstants.FILE,
                        DocumentFileType = photo.ContentType,
                        DocumentFolderPath = filinsertOutput.FullPathLocation,
                        DocumentFile = filinsertOutput.FileName,
                    });
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.BRANCH_PHOTOS,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Branch,
                        RecordID = branchID
                    });
                }


            }

            if (branchDto.Thumbnail != null)
            {
                var filinsertOutput = await this.SaveFileGlobalAsync(branchDto.Thumbnail, ApplicationConfigurationConstants.BranchThumbnail, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
                var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                {
                    Name = EntityConstants.Branch + AttachmentTypeConstants.BRANCH_THUMBNAIL,
                    Comment = null,
                    DocumentType = DocumentTypeConstants.FILE,
                    DocumentFileType = branchDto.Thumbnail.ContentType,
                    DocumentFolderPath = filinsertOutput.FullPathLocation,
                    DocumentFile = filinsertOutput.FileName,
                });
                await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentTypeName = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Branch,
                    RecordID = branchID
                });

            }

            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }






        [HttpPost("MapEmployees")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> MapBranchToPeople(BranchPeopleMapParams branchPeopleMap)
        {
			var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> MapBranchToPeople()-> params {0}", JsonConvert.SerializeObject(new { Params = branchPeopleMap }));
            
            var id = await branchService.MapBranchToPeople(branchPeopleMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToPeople()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            
            response.Data = id;

            return Ok(response);
        }

        [HttpPost("MapProducts")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> MapBranchToProduct(BranchProductMapParams branchProductMap)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = branchProductMap }));

            var id = await branchService.MapBranchToProducts(branchProductMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

			response.Data = id;

			return Ok(response);
        }
        [HttpPost("verify")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> VerifyBranchs(VerifyDto verifyBranchDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Branch Controller -> verifyBranchs()-> params {0}", JsonConvert.SerializeObject(new { Params = verifyBranchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id =await  branchService.VerifyBranchs(verifyBranchDto, UserName, this.AdminPersonID, this.PageSource);
            this.logger.LogInformation("Branch Controller -> verifyBranchs()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<bool>>> Delete(int id)
        {
            var response = new OperationResult<bool>();
            this.logger.LogInformation("Branch Controller -> deleteBranch()-> params {0}", id);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            response.Data = true;
            await branchService.DeleteBranch(id, UserName);
            return Ok(response);
        }

    }
}
