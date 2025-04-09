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

		[HttpGet("preview")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetFileByKeyAsync(string key)
		{
			var fileObject = await this.fileService.GetFileByKeyAsync(key);
			return File(fileObject.fileStream, fileObject.contentType);
		}



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
