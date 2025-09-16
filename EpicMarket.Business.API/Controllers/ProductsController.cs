using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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
    /// Manages product catalog operations including CRUD, image management, and verification
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
	public class ProductsController : BaseApiController
	{
		private readonly ILogger<ProductsController> logger;
		private readonly IProductService productService;
		private readonly IApplicationConfigurationService applicationConfigurationService;
		private readonly IAttachmentService attachmentService;
		private readonly IFileService fileStoreService;
		private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IBranchService branchService;

		public ProductsController(ILogger<ProductsController> logger, IProductService productService, IApplicationConfigurationService applicationConfigurationService,
			IAttachmentService attachmentService, IFileService fileStoreService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
			this.logger = logger;
			this.productService = productService;
			this.applicationConfigurationService = applicationConfigurationService;
			this.attachmentService = attachmentService;
			this.fileStoreService = fileStoreService;
			this.dbContext = dbContext;
			this.httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
        /// Retrieves products available for outlet mapping
        /// </summary>
        /// <param name="outletID">The outlet ID to check existing product mappings</param>
        /// <returns>List of products with their mapping status</returns>
        /// <response code="200">Returns list of products with mapping options</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <remarks>
        /// Returns all business products with indicators showing:
        /// - Already mapped to the specified outlet
        /// - Available for mapping
        /// - Stock availability status
        /// </remarks>
        [HttpGet("Map/{outletID}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<List<ProductsMapOptionResult>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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
        /// Creates a new product with images
        /// </summary>
        /// <param name="productsDto">Product information including details, pricing, and images</param>
        /// <returns>The ID of the created product</returns>
        /// <response code="200">Product successfully created</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid product data or image upload failed</response>
        /// <remarks>
        /// Sample form data request:
        /// 
        ///     POST /api/products
        ///     Content-Type: multipart/form-data
        ///     
        ///     productName: "Premium Widget"
        ///     description: "High-quality widget for professional use"
        ///     sku: "WDG-001"
        ///     category: "Electronics"
        ///     price: 49.99
        ///     quantity: 100
        ///     unit: "piece"
        ///     thumbnail: [image file]
        ///     products: [array of product images]
        /// 
        /// Supports multiple product images and a separate thumbnail image.
        /// Images are automatically resized and optimized for web display.
        /// </remarks>
        [HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
		public async Task<ActionResult<OperationResult<int>>> AddProduct([FromForm] AddProductsDto productsDto)
		{

			var response = new OperationResult<int>();
			this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
			var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
			response.Data = await productService.AddProduct(productsDto, UserName, this.BusinessId, this.PageSource);

			if (productsDto.Products.Length > 0)
			{
				foreach (var product in productsDto.Products) {
					var filinsertOutput = await this.SaveFileGlobalAsync(product, ApplicationConfigurationConstants.Products, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
					var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
					{
						Name = EntityConstants.Catelog + AttachmentTypeConstants.PRODUCTIMAGES,
						Comment = null,
						DocumentType = DocumentTypeConstants.FILE,
						DocumentFileType = product.ContentType,
						DocumentFolderPath = filinsertOutput.FullPathLocation,
						DocumentFile = filinsertOutput.FileName,
					});
					await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
					{
						AttachmentTypeName = AttachmentTypeConstants.PRODUCTIMAGES,
						AttachmentID = attachmentId,
						Entity = EntityConstants.Catelog,
						RecordID = response.Data
					});

				}

				if (productsDto.Thumbnail.Length > 0)
				{
					var filinsertOutput = await this.SaveFileGlobalAsync(productsDto.Thumbnail, ApplicationConfigurationConstants.THUMBNAIL, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
					var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
					{
						Name = EntityConstants.Catelog + AttachmentTypeConstants.THUMBNAIL,
						Comment = null,
						DocumentType = DocumentTypeConstants.FILE,
						DocumentFileType = productsDto.Thumbnail.ContentType,
						DocumentFolderPath = filinsertOutput.FullPathLocation,
						DocumentFile = filinsertOutput.FileName,
					});
					await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
					{
						AttachmentTypeName = AttachmentTypeConstants.THUMBNAIL,
						AttachmentID = attachmentId,
						Entity = EntityConstants.Catelog,
						RecordID = response.Data
					});

				}

			}
			this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

			return Ok(response);
		}


		/// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The product ID to update</param>
        /// <param name="productsDto">Updated product information</param>
        /// <returns>The ID of the updated product</returns>
        /// <response code="200">Product successfully updated</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="404">Product not found</response>
        /// <response code="400">Invalid update data</response>
        /// <remarks>
        /// Updates product details including:
        /// - Basic information (name, description, SKU)
        /// - Pricing and inventory
        /// - Categories and tags
        /// - Images (new images can be added)
        /// 
        /// Existing images are preserved unless explicitly deleted.
        /// </remarks>
        [HttpPut("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
		public async Task<ActionResult<OperationResult<int>>> UpdateProduct([FromRoute] int id, [FromForm] AddProductsDto productsDto)
		{

			var response = new OperationResult<int>();
			this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
			var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
			response.Data = await productService.UpdateProducts(productsDto,id, UserName, this.BusinessId, this.PageSource);

			if (productsDto.Products?.Length > 0)
			{
				foreach (var product in productsDto.Products)
				{
					var filinsertOutput = await this.SaveFileGlobalAsync(product, ApplicationConfigurationConstants.Products, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
					var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
					{
						Name = EntityConstants.Catelog + AttachmentTypeConstants.PRODUCTIMAGES,
						Comment = null,
						DocumentType = DocumentTypeConstants.FILE,
						DocumentFileType = product.ContentType,
						DocumentFolderPath = filinsertOutput.FullPathLocation,
						DocumentFile = filinsertOutput.FileName,
					});
					await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
					{
						AttachmentTypeName = AttachmentTypeConstants.PRODUCTIMAGES,
						AttachmentID = attachmentId,
						Entity = EntityConstants.Catelog,
						RecordID = response.Data
					});

				}

				if (productsDto.Thumbnail != null)
				{
					var filinsertOutput = await this.SaveFileGlobalAsync(productsDto.Thumbnail, ApplicationConfigurationConstants.THUMBNAIL, this.fileStoreService, this.applicationConfigurationService, this.BusinessId);
					var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
					{
						Name = EntityConstants.Catelog + AttachmentTypeConstants.THUMBNAIL,
						Comment = null,
						DocumentType = DocumentTypeConstants.FILE,
						DocumentFileType = productsDto.Thumbnail.ContentType,
						DocumentFolderPath = filinsertOutput.FullPathLocation,
						DocumentFile = filinsertOutput.FileName,
					});
					await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
					{
						AttachmentTypeName = AttachmentTypeConstants.THUMBNAIL,
						AttachmentID = attachmentId,
						Entity = EntityConstants.Catelog,
						RecordID = response.Data
					});

				}

			}
			this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

			return Ok(response);
		}

        /// <summary>
        /// Retrieves all products with filtering and pagination
        /// </summary>
        /// <param name="productResult">Filter parameters including search, category, and pagination</param>
        /// <returns>Paginated list of products</returns>
        /// <response code="200">Returns paginated product list</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/products?pageNumber=1&amp;pageSize=20&amp;searchTerm=widget&amp;category=electronics&amp;inStock=true
        /// 
        /// Returns product information including:
        /// - Basic details and pricing
        /// - Stock levels
        /// - Categories and tags
        /// - Thumbnail images
        /// - Verification status
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<GetDataResult<List<ProductResult>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<OperationResult<GetDataResult<List<ProductResult>>>>> GetAllProducts([FromQuery] ProductParams productResult)
		{
			var response = new OperationResult<GetDataResult<List<ProductResult>>>();
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
			var results = await productService.GetAllProducts(productResult, this.BusinessId);
			this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
			response.Data = results;

			return Ok(response);
		}


		/// <summary>
        /// Retrieves detailed information for a specific product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>Complete product details</returns>
        /// <response code="200">Returns product details</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        /// <response code="404">Product not found</response>
        /// <remarks>
        /// Returns comprehensive product information including:
        /// - All product attributes
        /// - Full image gallery
        /// - Pricing tiers
        /// - Stock information
        /// - Associated outlets
        /// - Verification status and history
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        [ProducesResponseType(typeof(OperationResult<ProductsDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<ProductsDto>>> GetProductDetails(int id)
        {
            var response = new OperationResult<ProductsDto>();
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = id }));
            var results = await productService.GetProductDetails(id);
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Submits products for admin verification
        /// </summary>
        /// <param name="verifyBranchDto">Verification request with product IDs</param>
        /// <returns>Number of products submitted for verification</returns>
        /// <response code="200">Products successfully submitted for verification</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="400">Invalid verification request</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/products/verify
        ///     {
        ///        "productIds": [1, 2, 3, 4, 5],
        ///        "notes": "All product information complete and accurate"
        ///     }
        /// 
        /// Products must have:
        /// - Complete descriptions
        /// - At least one image
        /// - Valid pricing
        /// - Proper categorization
        /// </remarks>
        [HttpPost("verify")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<OperationResult<int>>> VerifyCatalog(VerifyDto verifyBranchDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> VerifyCatalog()-> params {0}", JsonConvert.SerializeObject(new { Params = verifyBranchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = await productService.VerifyCatalog(verifyBranchDto, UserName, this.AdminPersonID, this.PageSource);
            this.logger.LogInformation("Products Controller -> VerifyCatalog()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }


		/// <summary>
        /// Deletes product images
        /// </summary>
        /// <param name="Keys">List of image keys/URLs to delete</param>
        /// <returns>Success status</returns>
        /// <response code="200">Images successfully deleted</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="404">One or more images not found</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/products/images
        ///     {
        ///        "imageKeys": [
        ///            "products/123/image1.jpg",
        ///            "products/123/image2.jpg"
        ///        ]
        ///     }
        /// 
        /// Warning: This operation is permanent and cannot be undone.
        /// </remarks>
        [HttpDelete("images")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<bool>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
		public async Task<ActionResult<OperationResult<bool>>> DeleteImage(ListOfImages Keys)
		{
			var response = new OperationResult<bool>();
			this.logger.LogInformation("Products Controller -> deleteImage()-> params {0}", JsonConvert.SerializeObject(new { Params = Keys }));
			var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
			var status = await productService.deleteImage(Keys, UserName);
			response.Data = status;
			this.logger.LogInformation("Products Controller -> deleteImage()-> return {0}", status);
			return Ok(response);
		}


		/// <summary>
        /// Deletes a product from the catalog
        /// </summary>
        /// <param name="id">The product ID to delete</param>
        /// <returns>Success status</returns>
        /// <response code="200">Product successfully deleted</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not a business owner</response>
        /// <response code="404">Product not found</response>
        /// <response code="409">Product cannot be deleted due to existing orders</response>
        /// <remarks>
        /// This operation will:
        /// - Mark the product as inactive
        /// - Remove from all outlet mappings
        /// - Preserve historical data for reporting
        /// - Delete associated images from storage
        /// 
        /// Products with active orders cannot be deleted.
        /// </remarks>
        [HttpDelete("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
        [ProducesResponseType(typeof(OperationResult<bool>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
		public async Task<ActionResult<OperationResult<bool>>> Delete(int id)
		{
			var response = new OperationResult<bool>();
			this.logger.LogInformation("Products Controller -> deleteProducts()-> params {0}", id);
			var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
			response.Data = true;
			 await productService.deleteCatelog(id, UserName);
			return Ok(response);
		}


	}
}
