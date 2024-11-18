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
        private readonly IUnitOfWork unitOfWork;

        public branchesController(
                                    ILogger<branchesController> logger,
                                    IBranchService branchService,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IAttachmentService attachmentService,
                                    IFileService fileStoreService,
                                    IApplicationConfigurationService applicationConfigurationService,
                                    IUnitOfWork unitOfWork) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.branchService = branchService;
            this.httpContextAccessor = httpContextAccessor;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
            this.unitOfWork = unitOfWork;
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
        public async Task<ActionResult<List<BranchsDropDownOptions>>> GetAllOutletsForDropDown()
        {
            var response = new OperationResult<List<BranchsDropDownOptions>> ();

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

        [HttpPatch]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<bool>>> UpdateBrancheStatus( UpdateBrancheStatusParams branchParams)
        {
            var response = new OperationResult<bool>();

            this.logger.LogInformation("Branch Controller -> UpdateBrancheStatus()-> params {0}", JsonConvert.SerializeObject(new { Params = branchParams }));

            var outlet = await dbContext.Outlets.FindAsync(branchParams.BranchId);
            if (outlet != null)
            {
                outlet.IsOpen = branchParams.Is_Open;
                outlet.ModifiedDate= DateTime.UtcNow;
                outlet.ModifiedBy = this.LoggedInUserName;
                dbContext.Outlets.Update(outlet);
                await unitOfWork.Complete();
                response.Data = true;
            }
            else
            {
                throw new Exception("Branch Not Found");
            }

            return Ok(response);
        }

        [HttpGet] 
         public async Task<ActionResult<GetDataResult<List<OutletSeachDto>>>> GetNearbyOutlets(
          [FromQuery] double? latitude,
          [FromQuery] double? longitude,
          [FromQuery] double radiusKm = 10,
          [FromQuery] string category = null,
          [FromQuery] double? minRating = null,
          [FromQuery] string sortBy = "rating",
          [FromQuery] SortDirection sortDirection = SortDirection.Desc,
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 10)
        {
            var request = new OutletSearchRequest
            {
                Latitude = latitude,
                Longitude = longitude,
                RadiusKm = radiusKm,
                Category = category,
                MinRating = minRating,
                SortBy = sortBy,
                SortDirection = sortDirection,
                Page = page,
                PageSize = pageSize
            };

            var result = await branchService.GetNearbyOutletsAsync(request);
            return Ok(result);
        }

    }
}
