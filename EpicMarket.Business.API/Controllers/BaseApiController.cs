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
	//[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
		private readonly IHttpContextAccessor httpContextAccessor;
		protected readonly ApplicationDbContext dbContext;

		public BaseApiController(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor)
        {
			this.httpContextAccessor = httpContextAccessor;
			this.dbContext = dbContext;
        }

		public int BusinessId //we get the business id from the user role which is the last business id that the user is associated with
        {
            get {
                var usernameid = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return this.User.IsInRole(ROLES.BUSINESS_OWNER) ? 
                    dbContext.Businesses.Where(c => c.PersonID == usernameid).Select(c => c.ID).OrderByDescending(c => c).FirstOrDefault() : 
                    this.User.IsInRole(ROLES.BUSINESS_EMPLOYEE) ? 
                        dbContext.BusinessEmployeeMaps.Where(c => c.EmployeeID == usernameid).Select(c => c.BussinessID).OrderByDescending(c => c).FirstOrDefault() : 
                        throw new("No Business is found");
			}
		}
		public string LoggedInUserName
        {
			get
			{
				return this.User.FindFirst(ClaimTypes.Name).Value;
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
				uriBuilder.Port = uriBuilder.Uri.IsDefaultPort ? -1 : uriBuilder.Port;

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



        protected async Task<SaveFileDTO> SaveFileBusinessAsync(IFormFile file, IFileService fileStoreService, IApplicationConfigurationService applicationConfigurationService,int BusinessId)
		{
			if (file != null && file.Length > 0)
			{
				string fullPathLocation = applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.BasePath);
				if (BusinessId != 0) {
					fullPathLocation = fullPathLocation + "/" + BusinessId +"/Images/";
				}
				else {
					throw new Exception("BusinessId is required");
				}
		
				// Made fitting based on AWS
				var uploadedFile = file;

				var fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
				string type = Path.GetExtension(uploadedFile.FileName);
				fileName += "_" + DateTime.Now.Ticks.ToString() + type;

				fileName = fileName.Replace('&', '_').Replace('<', '_').Replace('>', '_');
				fileName = fileName.SanitizeFile();
				var key =  await fileStoreService.UploadFileAsync(uploadedFile, fullPathLocation, fileName, EntityConstants.Business, BusinessId);

				return new SaveFileDTO()
				{
					Key = key
				};
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
            if (FilePathConstants.LOGOPATH.Equals(entityNameOrAppConfig))
            {
                path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.LOGOPATH);
            }
            else if (FilePathConstants.ProofPATH.Equals(entityNameOrAppConfig))
            {
                path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.ProofPATH);
            }
            else if (ApplicationConfigurationConstants.Business.Equals(entityNameOrAppConfig))
            {
                path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.Business);
            }
			else if (ApplicationConfigurationConstants.Products.Equals(entityNameOrAppConfig))
			{
				path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.PRODUCTPATH);
			}
			else if (ApplicationConfigurationConstants.THUMBNAIL.Equals(entityNameOrAppConfig))
			{
				path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.THUMBNAILPATH);
			}
            else if (ApplicationConfigurationConstants.BranchesPhotos.Equals(entityNameOrAppConfig))
            {
                path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.BranchesPhotos);
            }
            else if (ApplicationConfigurationConstants.BranchThumbnail.Equals(entityNameOrAppConfig))
            {
                path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.BranchThumbnail);
            }
			else if (ApplicationConfigurationConstants.TASKPATH.Equals(entityNameOrAppConfig))
            {
                path = applicationConfigurationService.GetApplicationConfigurationValue(FilePathConstants.TASKPATH);
            }



            return path;
		}



	}
}
