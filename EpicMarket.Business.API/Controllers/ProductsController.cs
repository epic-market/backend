using EpicMarket.Contracts;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

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
        public async Task<ActionResult<OperationResult<List<ProductsMapOptionResult>>>> GetAllProductForMap(int bussinessID,int outletID)
        {
            var response = new OperationResult<List<ProductsMapOptionResult>>();

			this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = bussinessID }));

            var results = await productService.GetAllProductForMap(bussinessID, outletID);

            this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

			response.Data = results;

			return Ok(response);
        }


        [HttpPost("AddProduct")]
        public async Task<ActionResult<OperationResult<int>>> AddProduct(ProductsDto productsDto)
        {

			var response = new OperationResult<int>();

			this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;

            response.Data  = productService.AddProduct(productsDto, UserName);

            this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

            return Ok(response);
        }
    }
}
