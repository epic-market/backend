using Amazon.S3;
using Amazon.S3.Model;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// File management API for business owners, including upload, preview, and bulk delete actions.
    /// Route prefix: api/files
    /// Auth: Mixed (business owner only for mutating operations).
    /// </summary>
    [Route("api/files")]
	public class FilesController : BaseApiController
    {

		private readonly IFileService fileService;
        private readonly ApplicationDbContext dbContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly IApplicationConfigurationService applicationConfigurationService;

		public FilesController(IFileService fileService, ApplicationDbContext dbContext ,IUnitOfWork unitOfWork, IApplicationConfigurationService applicationConfigurationService, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
			this.fileService = fileService;
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
            this.applicationConfigurationService = applicationConfigurationService;
		}

		/// <summary>
		/// Uploads a single file to the configured file store for the current business.
		/// Route: POST api/files
		/// Auth: Business owner.
		/// </summary>
		/// <param name="file">The file payload supplied via multipart form data.</param>
		/// <returns>Uploaded file metadata including the generated storage key.</returns>
		[HttpPost]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<OperationResult<SaveFileDTO>> UploadFileAsync([FromForm]IFormFile file)
		{
			var key =  await this.SaveFileBusinessAsync(file, fileService, applicationConfigurationService, BusinessId);
			return new OperationResult<SaveFileDTO>()
			{
				Data = key
			};
		}

		/// <summary>
		/// Streams a file from storage using its key, typically for inline previews.
		/// Route: GET api/files/preview?key=VALUE
		/// Auth: AllowAnonymous (relies on key secrecy).
		/// </summary>
		/// <param name="key">The unique storage key generated during upload.</param>
		/// <returns>Binary file stream with the original content type.</returns>
		[HttpGet("preview")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetFileByKeyAsync(string key)
		{
			var fileObject = await this.fileService.GetFileByKeyAsync(key);
			return File(fileObject.fileStream, fileObject.contentType);
		}



		/// <summary>
		/// Deletes multiple files that belong to the current business.
		/// Route: DELETE api/files/multiple
		/// Auth: Business owner.
		/// </summary>
		/// <param name="Keys">Collection of storage keys to remove.</param>
		/// <returns>True when all supplied files are deleted successfully.</returns>
		[HttpDelete("multiple")]
		[Authorize(Roles = ROLES.BUSINESS_OWNER)]
		public async Task<ActionResult<OperationResult<bool>>> DeleteFile(ListOfImages Keys)
		{
            var response = new OperationResult<bool>();
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var status = await this.fileService.DeleteImage(Keys, UserName);
            response.Data = status;
            return Ok(response);
        }





    }
}
