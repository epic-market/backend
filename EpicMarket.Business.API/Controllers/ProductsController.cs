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
        private readonly IProductService productService;
        private readonly IBranchService branchService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            this.logger = logger;
            this.productService = productService;
        }

        [HttpGet("GetAllProductForMap")]
        public async Task<ActionResult<List<ProductsMapOptionResult>>> GetAllProductForMap(int bussinessID,int outletID)
        {
            this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = bussinessID }));

            var results = await productService.GetAllProductForMap(bussinessID, outletID);

            this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            return Ok(results);
        }
    }
}
