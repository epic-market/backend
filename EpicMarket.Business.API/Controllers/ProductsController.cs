using EpicMarket.Contracts;
using EpicMarket.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EpicMarket.Business.API.Controllers
{

    public class ProductsController : BaseApiController
    {

        private readonly ILogger<ProductsController> logger;
        private readonly IBranchService branchService;

        public ProductsController(ILogger<ProductsController> logger, IBranchService branchService)
        {
            this.logger = logger;
            this.branchService = branchService;
        }

        [HttpPost("GetAllProductForMap")]
        public async Task<ActionResult<List<ProductsMapOptionResult>>> GetAllProductForMap(int BussinessID)
        {
            this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = BussinessID }));

            var branchParams = new BranchParams();

            var results = await branchService.GetAllBranches(branchParams);

            this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            return Ok(results);
        }
    }
}
