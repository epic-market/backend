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
    /// <summary>
    /// Product catalog API. Provides business owners and employees with CRUD operations,
    /// inventory management, quick actions, and customer-facing catalog queries.
    /// </summary>
    /// <remarks>
    /// Route prefix: <c>api/products</c>
    /// </remarks>
    [Route("api/products")]
    public partial class ProductController : BaseApiController
	{
		private readonly ILogger<ProductController> logger;
		private readonly IProductService productService;
		private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly IRatingService ratingService;
        private readonly IAttachmentService attachmentService;
		private readonly IFileService fileStoreService;
        private readonly IProductCategoryService catalogCategoryService;
        private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IOutletService branchService;

		public ProductController(ILogger<ProductController> logger, IProductService productService, IApplicationConfigurationService applicationConfigurationService,IRatingService ratingService,
			IAttachmentService attachmentService, IFileService fileStoreService,IProductCategoryService catalogCategoryService , ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
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


        /// <summary>
        /// Creates a new product for the current business.
        /// </summary>
        /// <remarks>
        /// Route: <c>POST api/products</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// Body: JSON <see cref="AddProductsParams"/>.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> AddProduct([FromBody] AddProductsParams productsDto)
        {

            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            response.Data = await productService.AddProduct(productsDto, UserName, this.BusinessId, this.PageSource);
            this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

            return Ok(response);
        }


        /// <summary>
        /// Adds or updates inventory details for a product variant.
        /// </summary>
        /// <remarks>
        /// Route: <c>POST api/products/Inventory</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// Body: JSON <see cref="InventoryResult"/>.
        /// </remarks>
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


        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <remarks>
        /// Route: <c>PUT api/products/{{id}}</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// Body: JSON <see cref="AddProductsParams"/>.
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> UpdateProduct([FromRoute] int id, [FromBody] AddProductsParams productsDto)
        {

            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            response.Data = await productService.UpdateProducts(productsDto, id, UserName, this.BusinessId, this.PageSource);
            this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

            return Ok(response);
        }


        /// <summary>
        /// Performs bulk quick actions such as publish/unpublish or discount updates on products.
        /// </summary>
        /// <remarks>
        /// Route: <c>POST api/products/QuickActions</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// Body: JSON <see cref="QuickActionsParams"/>.
        /// </remarks>
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


        /// <summary>
        /// Adds an internal rating to a product from business users.
        /// </summary>
        /// <remarks>
        /// Route: <c>POST api/products/Rating</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// Body: JSON <see cref="AddProductRatingRequest"/>.
        /// </remarks>
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


        /// <summary>
        /// Lists products for mapping to a specific outlet (POS catalog mapping).
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/Map/{{outletID}}</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// </remarks>
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


        /// <summary>
        /// Retrieves inventory details for a product variant at a specific branch.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/Inventory</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// Query: <c>productVariantId</c>, <c>branchId</c>.
        /// </remarks>
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

        /// <summary>
        /// Retrieves a paged list of products for the current business.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// Query: <see cref="ProductListParams"/> filters.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<GetDataResult<List<ProductResult>>>>> GetAllProducts([FromQuery] ProductListParams productResult)
		{
			var response = new OperationResult<GetDataResult<List<ProductResult>>>();
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
			var results = await productService.GetAllProducts(productResult, this.BusinessId);
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
			response.Data = results;

			return Ok(response);
		}


        /// <summary>
        /// Gets detailed information for a product.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/{{id}}</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<ProductDetailsResult>>> GetProductDetails(int id)
        {
            var response = new OperationResult<ProductDetailsResult>();
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));
            var results = await productService.GetProductDetails(id);
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Gets public product details for customer storefront usage.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/customer/{{id}}</c>
        /// Auth: <c>AllowAnonymous</c>
        /// </remarks>
        [HttpGet("customer/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<ProductDetailsV2Result>>> GetProductDetailsV2(int id)
        {
            var response = new OperationResult<ProductDetailsV2Result>();
            this.logger.LogInformation("Products Controller -> GetProductDetailsV2()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));
            var results = await productService.GetProductDetailsV2(id);
            this.logger.LogInformation("Products Controller -> GetProductDetailsV2()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;

            return Ok(response);
        }



        /// <summary>
        /// Marks a product as verified for publication.
        /// </summary>
        /// <remarks>
        /// Route: <c>POST api/products/verify</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// Body: JSON <see cref="VerifyProductDto"/>.
        /// </remarks>
        [HttpPost("verify")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<int>>> VerifyProduct(VerifyProductDto verifyProductDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> VerifyProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = verifyProductDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await productService.VerifyProduct(verifyProductDto, UserName, this.AdminPersonID, this.PageSource);
            this.logger.LogInformation("Products Controller -> VerifyProduct()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }



        /// <summary>
        /// Deletes a product from the catalog.
        /// </summary>
        /// <remarks>
        /// Route: <c>DELETE api/products/{{id}}</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER)</c>
        /// </remarks>
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



        /// <summary>
        /// Retrieves products configured for the POS experience for a specific outlet.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/POS/{{outletID}}</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// Query: <see cref="ProductPOSParams"/> filters.
        /// </remarks>
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

 


        /// <summary>
        /// Retrieves products optimized for the business mobile application.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/customer-mobile</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// Query: <see cref="ProductMobileParams"/> filters.
        /// </remarks>
        [HttpGet("customer-mobile")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<GetDataResult<List<CustomerProductResult>>>>> GetAllProductsForMobile([FromQuery] ProductMobileParams productResult)
		{
			var response = new OperationResult<GetDataResult<List<CustomerProductResult>>>();
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
			var results = await productService.GetAllProductsForMobile(productResult);
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
			response.Data = results;
			return Ok(response);
		}

        /// <summary>
        /// Get all categories for a specific outlet (customer endpoint)
        /// </summary>
        /// <param name="outletId">ID of the outlet</param>
        /// <returns>List of categories</returns>
        /// <summary>
        /// Gets public categories for a specific outlet for customer browsing.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/customer/categories/{{outletId}}</c>
        /// Auth: <c>AllowAnonymous</c>
        /// </remarks>
        [HttpGet("customer/categories/{outletId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<CategoriesDto>>>> GetCategoriesByOutlet(int outletId)
        {
            var response = new OperationResult<List<CategoriesDto>>();
            this.logger.LogInformation("Products Controller -> GetCategoriesByOutlet()-> outletId: {0}", outletId);
            var categories = await catalogCategoryService.GetCategoriesByOutletId(outletId);
            response.Data = categories;
            return Ok(response);
        }

        /// <summary>
        /// Get products filtered by outlet, category, and search term (customer endpoint)
        /// </summary>
        /// <param name="parameters">Filter parameters including outlet ID, category, and search term</param>
        /// <returns>List of products grouped by category</returns>
        /// <summary>
        /// Gets public products filtered by outlet, category, and search term for customers.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/customer/products</c>
        /// Auth: <c>AllowAnonymous</c>
        /// Query: <see cref="ProductMobileParams"/> filters (OutletId required).
        /// </remarks>
        [HttpGet("customer/products")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<GetDataResult<List<CustomerProductResult>>>>> GetCustomerProducts([FromQuery] ProductMobileParams parameters)
        {
            if (parameters.OutletId <= 0)
            {
                return BadRequest("Valid outlet ID is required");
            }

            var response = new OperationResult<GetDataResult<List<CustomerProductResult>>>();
            this.logger.LogInformation("Products Controller -> GetCustomerProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = parameters }));
            var results = await productService.GetAllProductsForMobile(parameters);
            this.logger.LogInformation("Products Controller -> GetCustomerProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;
            return Ok(response);
        }

        /// <summary>
        /// Get detailed product information for customers (customer endpoint)
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>Detailed product information with available variants</returns>
        /// <summary>
        /// Gets detailed product information for customers, including variants and pricing.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/customer/products/{{productId}}</c>
        /// Auth: <c>AllowAnonymous</c>
        /// </remarks>
        [HttpGet("customer/products/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<CustomerProductDetailsResult>>> GetCustomerProductDetails(int productId)
        {
            var response = new OperationResult<CustomerProductDetailsResult>();
            this.logger.LogInformation("Products Controller -> GetCustomerProductDetails()-> productId: {0}", productId);
            
            try
            {
                var result = await productService.GetCustomerProductDetails(productId);
                response.Data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error getting customer product details for product {0}", productId);
                return NotFound(new { message = ex.Message });
            }
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
        
        /// <summary>
        /// Retrieves all variants for a given product.
        /// </summary>
        /// <remarks>
        /// Route: <c>GET api/products/{{productId}}/variants</c>
        /// Auth: <c>Authorize(Roles = BUSINESS_OWNER,BUSINESS_EMPLOYEE)</c>
        /// </remarks>
        [HttpGet("{productId}/variants")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<List<SingleProductVariantsResult>>>> GetProductVariants(
            [FromRoute] int productId)
        {
            var response = new OperationResult<List<SingleProductVariantsResult>>();
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
