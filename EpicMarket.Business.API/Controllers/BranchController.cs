using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Manages branch operations including CRUD operations and branch-product/people mappings
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BranchController : BaseApiController
    {
        private readonly ILogger<BranchController> logger;
        private readonly IBranchService branchService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BranchController(ILogger<BranchController> logger, IBranchService branchService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.branchService = branchService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves all branches for the current business with pagination and filtering
        /// </summary>
        /// <param name="branchParams">Filter parameters including pagination, search criteria</param>
        /// <returns>Paginated list of branches</returns>
        /// <response code="200">Returns list of branches with pagination metadata</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <remarks>
        /// Only business owners can access this endpoint.
        /// Results are filtered by the current business context.
        /// </remarks>
        [HttpGet("GetAllBranches")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<GetDataResult<List<BranchResult>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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
        /// Retrieves details of a specific branch by ID
        /// </summary>
        /// <param name="branchId">The unique identifier of the branch</param>
        /// <returns>Branch details including associated products and employees</returns>
        /// <response code="200">Returns the branch details</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="404">Branch not found</response>
        [HttpGet("GetBranchByID")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<BranchResult>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
		public async Task<ActionResult<OperationResult<BranchResult>>> GetBranchByID(int branchId)
        {
            var response = new OperationResult<BranchResult>();

            this.logger.LogInformation("Branch Controller -> GetBranchByID()-> params {0}", JsonConvert.SerializeObject(new { Params = new { branchID = branchId } }));

            var results = await branchService.GetBranchByID(branchId);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        /// <summary>
        /// Creates a new branch or updates an existing branch
        /// </summary>
        /// <param name="branchDto">Branch information including name, address, contact details</param>
        /// <returns>The ID of the created or updated branch</returns>
        /// <response code="200">Branch successfully created or updated</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid branch data provided</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/branch/AddOrUpdateBranch
        ///     {
        ///        "branchName": "Downtown Branch",
        ///        "address": "123 Main Street",
        ///        "city": "New York",
        ///        "state": "NY",
        ///        "zipCode": "10001",
        ///        "phone": "+1234567890",
        ///        "email": "branch@example.com"
        ///     }
        /// </remarks>
        [HttpPost("AddOrUpdateBranch")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
		public async Task<ActionResult<OperationResult<int>>> AddBranch(BranchDto branchDto)
        {
            var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id =  await branchService.AddOrUpdateBranch(branchDto, UserName, this.BusinessId,this.PageSource);
            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
			return Ok(response);
        }


        /// <summary>
        /// Associates employees with a specific branch
        /// </summary>
        /// <param name="branchPeopleMap">Mapping information containing branch ID and employee IDs</param>
        /// <returns>Number of successful mappings created</returns>
        /// <response code="200">Employees successfully mapped to branch</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid mapping data provided</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/branch/MapBranchToPeople
        ///     {
        ///        "branchId": 1,
        ///        "employeeIds": [10, 20, 30]
        ///     }
        /// </remarks>
        [HttpPost("MapBranchToPeople")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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
        /// Associates products with a specific branch
        /// </summary>
        /// <param name="branchProductMap">Mapping information containing branch ID and product IDs</param>
        /// <returns>Number of successful product mappings created</returns>
        /// <response code="200">Products successfully mapped to branch</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid mapping data provided</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/branch/MapBranchToProduct
        ///     {
        ///        "branchId": 1,
        ///        "productIds": [100, 200, 300]
        ///     }
        /// </remarks>
        [HttpPost("MapBranchToProduct")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
		public async Task<ActionResult<OperationResult<int>>> MapBranchToProduct(BranchProductMapParams branchProductMap)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = branchProductMap }));

            var id = await branchService.MapBranchToProducts(branchProductMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

			response.Data = id;

			return Ok(response);
        }
        /// <summary>
        /// Submits branches for verification by admin
        /// </summary>
        /// <param name="verifyBranchDto">Verification information including branch IDs and verification notes</param>
        /// <returns>Number of branches submitted for verification</returns>
        /// <response code="200">Branches successfully submitted for verification</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid verification data provided</response>
        /// <remarks>
        /// This endpoint initiates the verification process for branches.
        /// Once submitted, branches will be reviewed by system administrators.
        /// 
        /// Sample request:
        /// 
        ///     POST /api/branch/verifyBranchs
        ///     {
        ///        "branchIds": [1, 2, 3],
        ///        "verificationNotes": "All documents uploaded"
        ///     }
        /// </remarks>
        [HttpPost("verifyBranchs")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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

    }
}
