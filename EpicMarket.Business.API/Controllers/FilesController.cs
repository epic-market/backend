using Amazon.S3;
using Amazon.S3.Model;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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

		[HttpPost]
		public async Task<IActionResult> UploadFileAsync(IFormFile file , string? prefix)
		{
			var key =  await this.SaveFileGlobalAsync(file, ApplicationConfigurationConstants.Products, fileService, applicationConfigurationService);
			return Ok($"File {prefix}/{key} uploaded to S3 successfully!");
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFilesAsync(string? prefix)
		{
			var allfiles =  await this.fileService.GetAllFilesAsync(prefix);
			return Ok(allfiles);
		}

		[HttpGet("preview")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetFileByKeyAsync(string key)
		{

			var fileObject = await this.fileService.GetFileByKeyAsync(key);
			return File(fileObject.fileStream, fileObject.contentType);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteFileAsync(string key)
		{
		    await this.fileService.DeleteFileAsync(key);
			return NoContent();
		}


        [HttpDelete("images")]
        [Authorize(Roles = ROLES.BUSINESS_OWNER)]
        public async Task<ActionResult<OperationResult<bool>>> DeleteImage(ListOfImages Keys)
        {
            var response = new OperationResult<bool>();
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;
            var status = await this.fileService.DeleteImage(Keys, UserName);
            response.Data = status;
            return Ok(response);
        }





    }
}
