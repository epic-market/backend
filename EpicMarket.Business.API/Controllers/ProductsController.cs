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


		[HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<int>>> AddProduct([FromForm] AddProductsDto productsDto)
		{

			var response = new OperationResult<int>();
			this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
			var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
			response.Data = await productService.AddProduct(productsDto, UserName, this.BusinessId, this.PageSource);

			if (productsDto.Products?.Length > 0)
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


		[HttpPut("{id}")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
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
        public async Task<ActionResult<OperationResult<GetDataResult<List<ProductResult>>>>> GetAllProductsForPOS([FromQuery] ProductPOSParams productResult , int outletID)
        {
            var response = new OperationResult<GetDataResult<List<ProductResult>>>();
            this.logger.LogInformation("Products Controller -> GetAllProductsForPOS()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));
            var results = await productService.GetAllProductsForPOS(productResult, outletID);
            this.logger.LogInformation("Products Controller -> GetAllProductsForPOS()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            response.Data = results;

            return Ok(response);
        }
    }
}
