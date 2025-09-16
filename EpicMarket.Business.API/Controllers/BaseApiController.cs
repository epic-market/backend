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
    /// <summary>
    /// Base controller class providing common functionality for all API controllers
    /// </summary>
    /// <remarks>
    /// This base controller provides:
    /// - Business context access
    /// - User authentication information
    /// - File management utilities
    /// - Common properties for derived controllers
    /// </remarks>
    //[ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
		private readonly IHttpContextAccessor httpContextAccessor;
		protected readonly ApplicationDbContext dbContext;

		public BaseApiController(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor
			)
        {
			this.httpContextAccessor = httpContextAccessor;
			this.dbContext = dbContext;
        }

		/// <summary>
        /// Gets the business ID associated with the current authenticated user
        /// </summary>
        /// <value>Business ID for business owner or employee</value>
        /// <exception cref="Exception">Thrown when no business is found for the user</exception>
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
		/// <summary>
        /// Gets the username of the currently logged-in user
        /// </summary>
        /// <value>The username from the authentication claims</value>
        public string LoggedInUserName
        {
			get
			{
				return this.User.FindFirst(ClaimTypes.Name).Value;
			}
		}
		/// <summary>
        /// Gets the complete URL of the current request
        /// </summary>
        /// <value>Full URL including scheme, host, path, and query string</value>
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

        /// <summary>
        /// Gets the ID of the system administrator
        /// </summary>
        /// <value>The person ID of the admin user</value>
        protected int AdminPersonID
        {
            get
            {
               return dbContext.Users.FirstOrDefault(u => u.UserName == Constants.ADMIN_USERID).Id;
            }
        }



        /// <summary>
        /// Saves a file to the configured storage location
        /// </summary>
        /// <param name="file">The file to be uploaded</param>
        /// <param name="entityName">Entity name to determine storage path</param>
        /// <param name="fileStoreService">File storage service instance</param>
        /// <param name="applicationConfigurationService">Configuration service for path resolution</param>
        /// <param name="RecordID">Optional record ID for organizing files</param>
        /// <returns>SaveFileDTO containing file name and storage location, or null if file is empty</returns>
        /// <remarks>
        /// Files are saved with timestamp-based naming to prevent conflicts.
        /// Special characters are sanitized from filenames.
        /// </remarks>
        protected async Task<SaveFileDTO> SaveFileGlobalAsync(IFormFile file, string entityName, IFileService fileStoreService, IApplicationConfigurationService applicationConfigurationService,int RecordID = 0)
		{
			string filePath = " ";
			if (file != null && file.Length > 0)
			{
				string fullPathLocation = null;
				string subPathLocation = null;

                if (!string.IsNullOrWhiteSpace(entityName))
				{
					fullPathLocation = applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.BasePath);
                    subPathLocation = this.GetFolderPathFromConfiguration(entityName, applicationConfigurationService);
                    if (RecordID != 0) {
                        fullPathLocation = fullPathLocation + "/" + RecordID +"/"+ subPathLocation;
                    }
					if (!fullPathLocation.EndsWith("/"))
					{
						fullPathLocation = fullPathLocation + "/";
					}
				}
				// Made fitting based on AWS
				var uploadedFile = file;

				var fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
				string type = Path.GetExtension(uploadedFile.FileName);
				fileName += "_" + DateTime.Now.Ticks.ToString() + type;

				fileName = fileName.Replace('&', '_').Replace('<', '_').Replace('>', '_');
				fileName = fileName.SanitizeFile();
				filePath =  await fileStoreService.UploadFileAsync(uploadedFile, fullPathLocation, fileName);

				return new SaveFileDTO()
				{
					FileName = fileName,
					FullPathLocation = fullPathLocation.ToString()
				};
			}

			return null;
		}


		/// <summary>
        /// Retrieves a file from storage by its key
        /// </summary>
        /// <param name="filekey">Unique identifier of the file</param>
        /// <param name="entityName">Entity name to determine storage path</param>
        /// <param name="fileStoreService">File storage service instance</param>
        /// <param name="applicationConfigurationService">Configuration service for path resolution</param>
        /// <returns>FileDto containing file data and metadata, or null if not found</returns>
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


		/// <summary>
        /// Deletes a file from storage
        /// </summary>
        /// <param name="filekey">Unique identifier of the file to delete</param>
        /// <param name="entityName">Entity name to determine storage path</param>
        /// <param name="fileStoreService">File storage service instance</param>
        /// <param name="applicationConfigurationService">Configuration service for path resolution</param>
        /// <returns>The filekey if successful, null if file doesn't exist</returns>
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



		/// <summary>
        /// Retrieves the storage folder path based on entity type or configuration key
        /// </summary>
        /// <param name="entityNameOrAppConfig">Entity name or application configuration constant</param>
        /// <param name="applicationConfigurationService">Configuration service for path resolution</param>
        /// <returns>Configured storage path for the specified entity type</returns>
        /// <remarks>
        /// Supports paths for: Logo, Proof, Business, Products, and Thumbnail files
        /// </remarks>
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

			return path;
		}



	}
}
