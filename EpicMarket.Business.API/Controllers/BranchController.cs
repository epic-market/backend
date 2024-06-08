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

    [Authorize]
    public class BranchController : BaseApiController
    {
        private readonly ILogger<BranchController> logger;
        private readonly IBranchService branchService;

        public BranchController(ILogger<BranchController> logger, IBranchService branchService, ApplicationDbContext dbContext) : base(dbContext)
		{
            this.logger = logger;
            this.branchService = branchService;
       
        }

        [HttpGet("GetAllBranches")]
		[Authorize(Roles = "businessOwner")]
		public async Task<ActionResult<OperationResult<GetDataResult<List<BranchResult>>>>> GetAllBranches([FromQuery]BranchParams branchParams)
        {
            var response = new OperationResult<GetDataResult<List<BranchResult>>>();

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> params {0}", JsonConvert.SerializeObject(new { Params = branchParams }));

            var results = await branchService.GetAllBranches(branchParams, this.BusinessId);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

			response.Data = results;

			return Ok(response);
        }


        [HttpGet("GetBranchByID")]
		[Authorize(Roles = "businessOwner")]
		public async Task<ActionResult<OperationResult<BranchResult>>> GetBranchByID(int branchId)
        {
            var response = new OperationResult<BranchResult>();

            this.logger.LogInformation("Branch Controller -> GetBranchByID()-> params {0}", JsonConvert.SerializeObject(new { Params = new { branchID = branchId } }));

            var results = await branchService.GetBranchByID(branchId);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        [HttpPost("AddOrUpdateBranch")]
		[Authorize(Roles = "businessOwner")]
		public async Task<ActionResult<OperationResult<int>>> AddBranch(BranchDto branchDto)
        {
            var response = new OperationResult<int>();

			this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));

            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;

            var id = await branchService.AddOrUpdateBranch(branchDto, UserName, this.BusinessId);
            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            response.Data = id;

			return Ok(response);
        }


        [HttpPost("MapBranchToPeople")]
		[Authorize(Roles = "businessOwner")]
		public async Task<ActionResult<OperationResult<int>>> MapBranchToPeople(BranchPeopleMapParams branchPeopleMap)
        {
			var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> MapBranchToPeople()-> params {0}", JsonConvert.SerializeObject(new { Params = branchPeopleMap }));
            
            var id = await branchService.MapBranchToPeople(branchPeopleMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToPeople()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            
            response.Data = id;

            return Ok(response);
        }

        [HttpPost("MapBranchToProduct")]
		[Authorize(Roles = "businessOwner")]
		public async Task<ActionResult<OperationResult<int>>> MapBranchToProduct(BranchProductMapParams branchProductMap)
        {

			var response = new OperationResult<int>();
			this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = branchProductMap }));

            var id = await branchService.MapBranchToProducts(branchProductMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

			response.Data = id;

			return Ok(response);
        }

    }
}
