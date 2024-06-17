using Amazon.S3;
using Amazon.S3.Model;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EpicMarket.Business.API.Controllers
{

	public class FilesController : BaseApiController
	{

		private readonly IFileService fileService;
		private readonly IApplicationConfigurationService applicationConfigurationService;

		public FilesController(IFileService fileService, ApplicationDbContext dbContext , IApplicationConfigurationService applicationConfigurationService) : base(dbContext)
		{
			this.fileService = fileService;
			this.applicationConfigurationService = applicationConfigurationService;
		}

		[HttpPost]
		public async Task<IActionResult> UploadFileAsync(IFormFile file , string? prefix)
		{
			var key = await this.SaveFileGlobalAsync(file, ApplicationConfigurationConstants.Products, fileService, applicationConfigurationService);
			return Ok($"File {prefix}/{key} uploaded to S3 successfully!");
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFilesAsync(string? prefix)
		{
			var allfiles =  await this.fileService.GetAllFilesAsync(prefix);
			return Ok(allfiles);
		}

		[HttpGet("preview")]
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



	}
}
