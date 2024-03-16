using EpicMarket.Contracts;
using EpicMarket.Entities;
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

        public BranchController(ILogger<BranchController> logger, IBranchService branchService)
        {
            this.logger = logger;
            this.branchService = branchService;
       
        }

        [HttpGet("GetAllBranches")]
        public async Task<ActionResult<List<BranchResult>>> GetAllBranches(BranchParams branchParams)
        {
            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> params {0}", JsonConvert.SerializeObject(new { Params = branchParams }));

            var results = await branchService.GetAllBranches(branchParams);

            this.logger.LogInformation("Branch Controller -> GetAllBranches()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            return Ok(results);
        }

        [HttpPost("AddBranch")]
        public async Task<ActionResult<int>> AddBranch(BranchDto branchDto)
        {
            this.logger.LogInformation("Branch Controller -> AddBranch()-> params {0}", JsonConvert.SerializeObject(new { Params = branchDto }));

            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;

            var id = await branchService.AddBranch(branchDto, UserName);
            this.logger.LogInformation("Branch Controller -> AddBranch()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }


        [HttpPost("MapBranchToPeople")]
        public async Task<ActionResult<int>> MapBranchToPeople(BranchPeopleMapParams branchPeopleMap)
        {
            this.logger.LogInformation("Branch Controller -> MapBranchToPeople()-> params {0}", JsonConvert.SerializeObject(new { Params = branchPeopleMap }));
            
            var id = await branchService.MapBranchToPeople(branchPeopleMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToPeople()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }

        [HttpPost("MapBranchToProduct")]
        public async Task<ActionResult<int>> MapBranchToProduct(BranchProductMapParams branchProductMap)
        {
            this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = branchProductMap }));

            var id = await branchService.MapBranchToProducts(branchProductMap);

            this.logger.LogInformation("Branch Controller -> MapBranchToProduct()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));

            return Ok(id);
        }

    }
}
