using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{

	/// <summary>
	/// Outlet and branch management APIs for business owners and customer discovery.
	/// Route prefix: api/outlet
	/// Auth: Business owner for management endpoints; customer endpoints noted individually.
	/// </summary>
	[Authorize]
	[Route("api/outlet")]
	public class OutletController : BaseApiController
    {

        private readonly ILogger<OutletController> logger;
        private readonly IOutletService branchService;
        private readonly IRatingService ratingService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileStoreService;
        private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly IUnitOfWork unitOfWork;

        public OutletController(
                                    ILogger<OutletController> logger,
                                    IOutletService branchService,
                                    ApplicationDbContext dbContext,
                                    IRatingService ratingService,
                                    IHttpContextAccessor httpContextAccessor,
                                    IAttachmentService attachmentService,
                                    IFileService fileStoreService,
                                    IApplicationConfigurationService applicationConfigurationService,
                                    IUnitOfWork unitOfWork) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.branchService = branchService;
            this.ratingService = ratingService;
            this.httpContextAccessor = httpContextAccessor;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
            this.unitOfWork = unitOfWork;
        }

		/// <summary>
		/// Lists outlets for the current business with pagination and filters.
		/// Route: GET api/outlet
		/// Auth: Business owner.
		/// </summary>
		/// <param name="branchParams">Pagination and filter parameters for outlets.</param>
		/// <returns>Paginated outlet collection.</returns>
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


		/// <summary>
		/// Provides lightweight outlet data suitable for dropdown selectors.
		/// Route: GET api/outlet/dropdown-options
		/// Auth: Business owner.
		/// </summary>
		/// <returns>List of dropdown options representing outlets.</returns>
		[HttpGet("dropdown-options")]
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


		/// <summary>
		/// Retrieves details for a specific outlet.
		/// Route: GET api/outlet/{branchId}
		/// Auth: Business owner.
		/// </summary>
		/// <param name="branchId">The outlet identifier.</param>
		/// <returns>Outlet detail result.</returns>
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


		/// <summary>
		/// Creates a new outlet for the current business and links uploaded media.
		/// Route: POST api/outlet
		/// Auth: Business owner.
		/// </summary>
		/// <param name="branchDto">Outlet payload including media storage keys.</param>
		/// <returns>Identifier of the created outlet.</returns>
		[HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> AddBranch([FromBody]BranchDto branchDto)
        {
            var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id =  await branchService.AddBranch(branchDto, UserName, this.BusinessId,this.PageSource);
            if (branchDto.Photos?.Length > 0)
            {
                foreach (var photoKey in branchDto.Photos)
                {
                    var attachmentId = await this.attachmentService.GetAttachmentId(photoKey);
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.BRANCH_PHOTOS,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Branch,
                        RecordID = id
                    }, this.BusinessId);
                }
            }

            if (branchDto.Thumbnail != null)
            {
                var attachmentId = await this.attachmentService.GetAttachmentId(branchDto.Thumbnail);
                await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentTypeName = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Branch,
                    RecordID = id
                }, this.BusinessId);
            }

            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
			return Ok(response);
        }


		/// <summary>
		/// Updates outlet details and refreshes associated media links.
		/// Route: PUT api/outlet/{id}
		/// Auth: Business owner.
		/// </summary>
		/// <param name="id">Identifier of the outlet.</param>
		/// <param name="branchDto">Updated outlet data.</param>
		/// <returns>Identifier of the updated outlet.</returns>
		[HttpPut("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> UpdateBranch(int id, [FromBody]BranchDto branchDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var branchID = await branchService.UpdateBranch(id,branchDto, UserName, this.BusinessId, this.PageSource);
            if (branchDto.Photos?.Length > 0)
            {
                foreach (var photo in branchDto.Photos)
                {
                    var attachmentId = await this.attachmentService.GetAttachmentId(photo);
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.BRANCH_PHOTOS,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Branch,
                        RecordID = branchID
                    }, this.BusinessId);
                }


            }

            if (branchDto.Thumbnail != null)
            {
                var attachmentId = await this.attachmentService.GetAttachmentId(branchDto.Thumbnail);
                await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                {
                    AttachmentTypeName = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                    AttachmentID = attachmentId,
                    Entity = EntityConstants.Branch,
                    RecordID = branchID
                }, this.BusinessId);

            }

            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }






		/// <summary>
		/// Maps employees to an outlet for staffing and permissions.
		/// Route: POST api/outlet/map/employees
		/// Auth: Business owner.
		/// </summary>
		/// <param name="branchPeopleMap">Mapping payload linking employees to the outlet.</param>
		/// <returns>Identifier indicating result of the mapping.</returns>
		[HttpPost("map/employees")]
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

		/// <summary>
		/// Associates product variants with an outlet's catalog.
		/// Route: POST api/outlet/map/product-variants
		/// Auth: Business owner.
		/// </summary>
		/// <param name="branchProductVariantMap">Mapping payload specifying product variants and outlet.</param>
		/// <returns>Identifier representing the mapping operation.</returns>
		[HttpPost("map/product-variants")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> MapBranchToProductVariant(BranchProductVariantMapParams branchProductVariantMap)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = branchProductVariantMap }));

            var id = await branchService.MapBranchToProductVariants(branchProductVariantMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

			response.Data = id;

			return Ok(response);
        }
		/// <summary>
		/// Updates verification status for one or more outlets.
		/// Route: POST api/outlet/verify
		/// Auth: Business owner.
		/// </summary>
		/// <param name="verifyBranchDto">Verification parameters including outlet IDs.</param>
		/// <returns>Identifier corresponding to the verification record.</returns>
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


		/// <summary>
		/// Permanently deletes an outlet.
		/// Route: DELETE api/outlet/{id}
		/// Auth: Business owner.
		/// </summary>
		/// <param name="id">The outlet identifier.</param>
		/// <returns>True when deletion succeeds.</returns>
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

		/// <summary>
		/// Updates the open or closed status flag for an outlet.
		/// Route: PATCH api/outlet
		/// Auth: Business owner.
		/// </summary>
		/// <param name="branchParams">Payload specifying outlet ID and desired open status.</param>
		/// <returns>True when the status update is saved.</returns>
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

		/// <summary>
		/// Discovers nearby outlets using optional geolocation and filters.
		/// Route: GET api/outlet/nearby/outlets
		/// Auth: AllowAnonymous.
		/// </summary>
		/// <param name="latitude">Latitude of the search origin.</param>
		/// <param name="longitude">Longitude of the search origin.</param>
		/// <param name="radiusKm">Search radius in kilometers.</param>
		/// <param name="category">Optional category filter.</param>
		/// <param name="minRating">Optional minimum rating filter.</param>
		/// <param name="sortBy">Field to sort by (rating, distance, etc.).</param>
		/// <param name="sortDirection">Sort direction.</param>
		/// <param name="page">Page number starting from 1.</param>
		/// <param name="pageSize">Page size.</param>
		/// <returns>Paginated list of outlet search results.</returns>
		[HttpGet("NearBy/Outlets")]
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

		/// <summary>
		/// Returns outlets the current customer has subscribed to.
		/// Route: GET api/outlet/subscribed-outlets
		/// Auth: Authenticated customers.
		/// </summary>
		/// <param name="page">Page number starting from 1.</param>
		/// <param name="pageSize">Number of records per page.</param>
		/// <returns>Paginated list of subscribed outlets.</returns>
		[HttpGet("subscribed-outlets")]
		public async Task<ActionResult<GetDataResult<SubscribedOutletDto>>> GetSubscribedOutlets([FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var customerUserName = this.LoggedInUserName;
            var result = await branchService.GetSubscribedOutletsAsync(customerUserName, page, pageSize);
            return Ok(result);
        }

      

		/// <summary>
		/// Subscribes the logged-in customer to outlet updates.
		/// Route: POST api/outlet/subscribe/{outletId}
		/// Auth: Authenticated customers.
		/// </summary>
		/// <param name="outletId">Outlet identifier to subscribe to.</param>
		/// <returns>True when subscription succeeds.</returns>
		[HttpPost("subscribe/{outletId}")]
		public async Task<ActionResult<bool>> SubscribeOutlet(int outletId)
        {
            var customerUserName = this.LoggedInUserName;
            var result = await branchService.SubscribeOutletAsync(outletId, customerUserName);
            
            if (!result)
            {
                return BadRequest("Failed to subscribe to outlet");
            }

            return Ok(result);
        }

        // [HttpGet("customer/outlet")]
        // public async Task<ActionResult<GetDataResult<SubscribedOutletDto>>> GetSubscribedOutlets([FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        // {
        //     var customerUserName = this.LoggedInUserName;
        //     var result = await branchService.GetSubscribedOutletsAsync(customerUserName, page, pageSize);
        //     return Ok(result);
        // }

		/// <summary>
		/// Returns customer-facing details for a specific outlet, including personalized state.
		/// Route: GET api/outlet/customer/{outletId}
		/// Auth: AllowAnonymous (uses customer context when authenticated).
		/// </summary>
		/// <param name="outletId">Outlet identifier.</param>
		/// <returns>Customer outlet detail response.</returns>
		[HttpGet("customer/{outletId}")]
		[AllowAnonymous]
		public async Task<ActionResult<CustomerOutletDetailResult>> GetCustomerOutletDetail(int outletId)
        {
            string customerUserName = null;
            
            // Check if user is authenticated before getting username
            if (User.Identity.IsAuthenticated)
            {
                customerUserName = this.LoggedInUserName;
            }
            
            var response = new OperationResult<CustomerOutletDetailResult>();
            
            this.logger.LogInformation("Outlet Controller -> GetCustomerOutletDetail()-> params {0}", 
                JsonConvert.SerializeObject(new { OutletId = outletId, CustomerUserName = customerUserName }));
            
            var result = await branchService.GetCustomerOutletDetailAsync(outletId, customerUserName);
            
            this.logger.LogInformation("Outlet Controller -> GetCustomerOutletDetail()-> return {0}", 
                JsonConvert.SerializeObject(new { Result = result }));
            
            response.Data = result;
            return Ok(response);
        }
    }
}
