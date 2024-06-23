using EpicMarket.Business.API.Helpers;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace EpicMarket.Business.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
		private readonly IHttpContextAccessor httpContextAccessor;
		protected readonly ApplicationDbContext dbContext;

		public BaseApiController(IHttpContextAccessor httpContextAccessor,
			ApplicationDbContext dbContext)
        {
			this.httpContextAccessor = httpContextAccessor;
			this.dbContext = dbContext;
        }

		public BaseApiController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public int BusinessId
        {
            get {
                var usernameid = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (this.User.IsInRole(ROLES.BUSINESS_OWNER))
                {
                   return dbContext.Businesses.Where(c => c.PersonID == usernameid).Select(c => c.ID).FirstOrDefault(); ;
                }
                else if (this.User.IsInRole(ROLES.BUSINESS_EMPLOYEE))
                {
                    return dbContext.BusinessEmployeeMaps.Where(c => c.EmployeeID == usernameid).Select(c => c.BussinessID).FirstOrDefault();
                }
                else {
                    throw new("No Business is found");
                }
			}
		}

		protected string PageSource
		{
			get
			{

				var request = this.httpContextAccessor.HttpContext.Request;
				if (request == null) return null;
				var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1);
				uriBuilder.Path = request.Path.ToString();
				uriBuilder.Query = request.QueryString.ToString();
				if (uriBuilder.Uri.IsDefaultPort)
				{
					uriBuilder.Port = -1;
				}

				return uriBuilder.Uri.AbsoluteUri;
			}
		}

        protected int AdminPersonID
        {
            get
            {
               return dbContext.Users.FirstOrDefault(u => u.UserName == Constants.ADMIN_USERID).Id;
            }
        }



        protected async Task<string> SaveFileGlobalAsync(IFormFile file, string entityName, IFileService fileStoreService, IApplicationConfigurationService applicationConfigurationService)
		{
			string filePath = " ";
			if (file != null && file.Length > 0)
			{
				string fullPathLocation = null;
				if (!string.IsNullOrWhiteSpace(entityName))
				{
					fullPathLocation = this.GetFolderPathFromConfiguration(entityName, applicationConfigurationService);
					if (!fullPathLocation.EndsWith("\\"))
					{
						fullPathLocation = fullPathLocation + "\\";
					}
				}

				var uploadedFile = file;

				var fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
				string type = Path.GetExtension(uploadedFile.FileName);
				fileName += "_" + DateTime.Now.Ticks.ToString() + type;

				fileName = fileName.Replace('&', '_').Replace('<', '_').Replace('>', '_');
				fileName = fileName.SanitizeFile();
				filePath = await fileStoreService.UploadFileAsync(uploadedFile, fullPathLocation, fileName);

				return fileName;
			}

			return null;
		}


		protected async Task<FileDto> GetFile(string filekey, string entityName, IFileService fileStoreService, IApplicationConfigurationService applicationConfigurationService)
		{
			string filePath = " ";
			if (!string.IsNullOrEmpty(filekey))
			{
				string fullPathLocation = null;
				if (!string.IsNullOrWhiteSpace(entityName))
				{
					fullPathLocation = this.GetFolderPathFromConfiguration(entityName, applicationConfigurationService);
					if (!fullPathLocation.EndsWith("\\"))
					{
						fullPathLocation = fullPathLocation + "\\";
					}
				}


				var fullFilePath = Path.Combine(fullPathLocation, filePath);

				var file = await fileStoreService.GetFileByKeyAsync(fullFilePath);

				return file;
			}

			return null;
		}


		protected async Task<string> DeleteFile(string filekey, string entityName, IFileService fileStoreService, IApplicationConfigurationService applicationConfigurationService)
		{

			if (!string.IsNullOrWhiteSpace(filekey))
			{
				string fullPathLocation = null;
				if (!string.IsNullOrWhiteSpace(entityName))
				{
					fullPathLocation = this.GetFolderPathFromConfiguration(entityName, applicationConfigurationService);
					if (!fullPathLocation.EndsWith("\\"))
					{
						fullPathLocation = fullPathLocation + "\\";
					}
				}
				var fullPathToFile = Path.Combine(fullPathLocation, filekey);
				await fileStoreService.DeleteFileAsync(fullPathToFile);
				return filekey;
			}

			return null;
		}



		protected string GetFolderPathFromConfiguration(string entityNameOrAppConfig, IApplicationConfigurationService applicationConfigurationService = null)
		{
			var path = string.Empty;
			if (ApplicationConfigurationConstants.Products.Equals(entityNameOrAppConfig))
			{
				path = applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.Products);
			}
			else if (ApplicationConfigurationConstants.LOGO.Equals(entityNameOrAppConfig))
			{
				path = applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.LOGO);
			}
			
			return path;
		}



	}
}
