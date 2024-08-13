using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
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
		private readonly IApplicationConfigurationService applicationConfigurationService;
		private readonly IAttachmentService attachmentService;
		private readonly IFileService fileStoreService;
		private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IBranchService branchService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService,IApplicationConfigurationService applicationConfigurationService,

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

        [HttpGet("GetAllProductForMap")]
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


        [HttpPost("AddOrUpdateProduct")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async  Task<ActionResult<OperationResult<int>>> AddOrUpdateProduct([FromForm]ProductsDto productsDto)
        {

			var response = new OperationResult<int>();

			this.logger.LogInformation("Products Controller -> AddProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = productsDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;

            response.Data  =  await productService.AddOrUpdateProduct(productsDto, UserName,this.BusinessId,this.PageSource);

			if (productsDto.Products.Length > 0)
			{


                foreach (var product in productsDto.Products) {

					var filinsertOutput = await this.SaveFileGlobalAsync(product, ApplicationConfigurationConstants.Products, this.fileStoreService, this.applicationConfigurationService,this.BusinessId);
					var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
					{
						AttachmentTypeName = AttachmentTypeConstants.LOGO,
						Name = EntityConstants.Business + AttachmentTypeConstants.LOGO,
						Comment = null,
						DocumentType = DocumentTypeConstants.FILE,
						DocumentFileType = product.ContentType,
						DocumentFolderPath = filinsertOutput.FullPathLocation,
						DocumentFile = filinsertOutput.FileName,
					});
					this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
					{
						AttachmentID = attachmentId,
						Entity = EntityConstants.Business,
						RecordID = response.Data
					});

				}

				
			}

			this.logger.LogInformation("Products Controller -> AddProduct()-> return {0}", JsonConvert.SerializeObject(new { Results = response }));

            return Ok(response);
        }


        [HttpGet("GetAllProducts")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
		public async Task<ActionResult<OperationResult<GetDataResult<List<ProductResult>>>>> GetAllProducts([FromQuery] ProductParams productResult)
        {
            var response = new OperationResult<GetDataResult<List<ProductResult>>>();

            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productResult }));

            var results = await productService.GetAllProducts(productResult,this.BusinessId);

            this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }


        [HttpGet("GetProductDetails")]
        [Authorize(Roles = $"{ROLES.BUSINESS_OWNER},{ROLES.BUSINESS_EMPLOYEE}")]
        public async Task<ActionResult<OperationResult<ProductsDto>>> GetProductDetails(int productId)
        {
            var response = new OperationResult<ProductsDto>();

            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = productId }));

            var results = await productService.GetProductDetails(productId);

            this.logger.LogInformation("Products Controller -> GetAllProducts()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        [HttpPost("verifyCatalog")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public ActionResult<OperationResult<int>> VerifyCatalog(VerifyDto verifyBranchDto)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Products Controller -> VerifyCatalog()-> params {0}", JsonConvert.SerializeObject(new { Params = verifyBranchDto }));
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var id = productService.VerifyCatalog(verifyBranchDto, UserName, this.AdminPersonID, this.PageSource);
            this.logger.LogInformation("Products Controller -> VerifyCatalog()-> return {0}", JsonConvert.SerializeObject(new { Value = id }));
            response.Data = id;
            return Ok(response);
        }



    }
}
