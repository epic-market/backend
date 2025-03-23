using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace EpicMarket.Business.API.Controllers
{

    [Route("api/catalog")]
    public partial class CatalogController : BaseApiController
	{
		private readonly ILogger<CatalogController> logger;
		private readonly ICatalogService productService;
		private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly IRatingService ratingService;
        private readonly IAttachmentService attachmentService;
		private readonly IFileService fileStoreService;
        private readonly ICatalogCategoryService catalogCategoryService;
        private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IOutletService branchService;

		public CatalogController(ILogger<CatalogController> logger, ICatalogService productService, IApplicationConfigurationService applicationConfigurationService,IRatingService ratingService,
			IAttachmentService attachmentService, IFileService fileStoreService,ICatalogCategoryService catalogCategoryService , ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
			this.logger = logger;
			this.productService = productService;
			this.applicationConfigurationService = applicationConfigurationService;
            this.ratingService = ratingService;
            this.attachmentService = attachmentService;
			this.fileStoreService = fileStoreService;
            this.catalogCategoryService = catalogCategoryService;
            this.dbContext = dbContext;
			this.httpContextAccessor = httpContextAccessor;

		}


        [HttpPost]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> AddProduct([FromBody] AddProductsDto productsDto)
        {

            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            response.Data = await productService.AddProduct(productsDto, UserName, this.BusinessId, this.PageSource);
            this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

            return Ok(response);
        }


        [HttpPost("Inventory")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> AddOrUpdateProductInventoryDetails([FromBody] InventoryResult productAdvanced)
        {

            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> UpdateAdvanceSettings()-> params {0}", JsonConvert.SerializeObject(new { Params = productAdvanced }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            await productService.AddOrUpdateProductInventoryDetails(productAdvanced);
            this.logger.LogInformation("Products Controller -> UpdateAdvanceSettings()-> updated Successfully");
            return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> UpdateProduct([FromRoute] int id, [FromBody] AddProductsDto productsDto)
        {

            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            response.Data = await productService.UpdateProducts(productsDto, id, UserName, this.BusinessId, this.PageSource);
            this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

            return Ok(response);
        }


        [HttpPost("QuickActions")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> QuickActions(QuickActionsParams quickActionsParams)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> QuickActions()-> params {0}", JsonConvert.SerializeObject(new { Params = quickActionsParams }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await productService.QuickActions(quickActionsParams, UserName);
            this.logger.LogInformation("Products Controller -> QuickActions()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }


        [HttpPost("Rating")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<bool>> AddRatingToProduct([FromBody] AddProductRatingRequest request)
        {
            var response = new OperationResult<bool>();
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = request }));
            await ratingService.AddProductRatingAsync(request, this.LoggedInUserName);
            response.Data = true;
            return Ok(response);
        }




















        [HttpGet("Map/{outletID}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<List<ProductsMapOptionResult>>>> GetAllProductForMap(int outletID)
		{
			var response = new OperationResult<List<ProductsMapOptionResult>>();
			this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> params {0}", JsonConvert.SerializeObject(new { Params = outletID }));
			var results = await productService.GetAllProductForMap(this.BusinessId, outletID);
			this.logger.LogInformation("Products Controller -> GetAllProductForMap()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
			response.Data = results;
			return Ok(response);
		}





        [HttpGet("Inventory")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<InventoryResult>>> GetProductInventoryDetails(int productVariantId,int branchId)
        {
            var response = new OperationResult<InventoryResult>();
            this.logger.LogInformation("Products Controller -> GetProductInventoryDetails()-> params {0}", JsonConvert.SerializeObject(new { Params = productVariantId, branchId }));
            var results = await productService.GetProductInventoryDetails(productVariantId, branchId);
            this.logger.LogInformation("Products Controller -> GetProductInventoryDetails()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;
            return Ok(response);
        }





        [HttpGet]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<GetDataResult<List<ProductResult>>>>> GetAllProducts([FromQuery] ProductParams productResult)
		{
			var response = new OperationResult<GetDataResult<List<ProductResult>>>();
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
			var results = await productService.GetAllProducts(productResult, this.BusinessId);
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
			response.Data = results;

			return Ok(response);
		}


		[HttpGet("{id}")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<ProductsDto>>> GetProductDetails(int id)
        {
            var response = new OperationResult<ProductsDto>();
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));
            var results = await productService.GetProductDetails(id);
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;

            return Ok(response);
        }



        [HttpPost("verify")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]	
        public async Task<ActionResult<OperationResult<int>>> VerifyCatalog(VerifyCatalogDto verifyCatalogDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> VerifyCatalog()-> params {0}", JsonConvert.SerializeObject(new { Params = verifyCatalogDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await productService.VerifyCatalog(verifyCatalogDto, UserName, this.AdminPersonID, this.PageSource);
            this.logger.LogInformation("Products Controller -> VerifyCatalog()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }



		[HttpDelete("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<bool>>> Delete(int id)
		{
			var response = new OperationResult<bool>();
			this.logger.LogInformation("Products Controller -> deleteProducts()-> params {0}", id);
			var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
			response.Data = true;
			 await productService.deleteCatelog(id, UserName);
			return Ok(response);
		}



        [HttpGet("POS/{outletID}")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<GetDataResult<List<ProductForPOSResult>>>>> GetAllProductsForPOS([FromQuery] ProductPOSParams productResult , int outletID)
        {
            var response = new OperationResult<GetDataResult<List<ProductForPOSResult>>>();
            this.logger.LogInformation("Products Controller -> GetAllProductsForPOS()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
            var results = await productService.GetAllProductsForPOS(productResult, outletID);
            this.logger.LogInformation("Products Controller -> GetAllProductsForPOS()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;

            return Ok(response);
        }

 


		[HttpGet("customer-mobile")]
		[Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<GetDataResult<List<CustomerResultBaseOnCategory>>>>> GetAllProductsForMobile([FromQuery] ProductMobileParams productResult)
		{
			var response = new OperationResult<GetDataResult<List<CustomerResultBaseOnCategory>>> ();
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
			var results = await productService.GetAllProductsForMobile(productResult);
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
			response.Data = results;
			return Ok(response);
		}


        // [HttpPost("{id}/variants")]
        // [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        // public async Task<ActionResult<OperationResult<int>>> AddProductVariant(
        //     [FromRoute] int id,
        //     [FromBody] ProductVariantDto variantDto)
        // {
        //     var response = new OperationResult<int>();
        //     this.logger.LogInformation("Products Controller -> AddProductVariant()-> params {0}", 
        //         JsonConvert.SerializeObject(new { ProductId = id, Variant = variantDto }));

        //     var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
        //     response.Data = await productService.AddProductVariant(id, variantDto, UserName);
		// 	if (variantDto.ProductImages?.Length > 0)
		// 	{
		// 		foreach (var product in variantDto.ProductImages)
		// 		{
		// 			var attachmentId = await this.attachmentService.GetAttachmentId(product);
		// 			await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
		// 			{
		// 				AttachmentTypeName = AttachmentTypeConstants.PRODUCTIMAGES,
		// 				AttachmentID = attachmentId,
		// 				Entity = EntityConstants.CatelogVariant,
		// 				RecordID = response.Data
		// 			}, this.BusinessId);

		// 		}

		// 		if (variantDto.Thumbnail != null)
		// 		{
		// 			var attachmentId = await this.attachmentService.GetAttachmentId(variantDto.Thumbnail);
		// 			await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
		// 			{
		// 				AttachmentTypeName = AttachmentTypeConstants.THUMBNAIL,
		// 				AttachmentID = attachmentId,
		// 				Entity = EntityConstants.CatelogVariant,
		// 				RecordID = response.Data
		// 			}, this.BusinessId);

		// 		}

		// 	}
			
            
        //     return Ok(response);
        // }
        
        [HttpGet("{productId}/variants")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<List<ProductVariantResponse>>>> GetProductVariants(
            [FromRoute] int productId)
        {
            var response = new OperationResult<List<ProductVariantResponse>>();
            this.logger.LogInformation("Products Controller -> GetProductVariants()-> productId: {0}", productId);
            
            response.Data = await productService.GetProductVariants(productId);
            return Ok(response);
        }



        // [HttpPut("variants/{variantId}")]
        // [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        // public async Task<ActionResult<OperationResult<bool>>> UpdateProductVariant(
        //     [FromRoute] int variantId,
        //     [FromBody] ProductVariantDto variantDto)
        // {
        //     var response = new OperationResult<bool>();
        //     this.logger.LogInformation("Products Controller -> UpdateProductVariant()-> params {0}", 
        //         JsonConvert.SerializeObject(new { VariantId = variantId, Variant = variantDto }));
            
        //     var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
        //     await productService.UpdateProductVariant(variantId, variantDto, UserName);
        //     response.Data = true;
            
        //     return Ok(response);
        // }

    }
}
