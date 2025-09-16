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
    /// <summary>
    /// Manages file operations including upload, download, and deletion from cloud storage
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FilesController : BaseApiController
	{

		private readonly IFileService fileService;
		private readonly IApplicationConfigurationService applicationConfigurationService;

		public FilesController(IFileService fileService, ApplicationDbContext dbContext , IApplicationConfigurationService applicationConfigurationService, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
			this.fileService = fileService;
			this.applicationConfigurationService = applicationConfigurationService;
		}

		/// <summary>
        /// Uploads a file to cloud storage (S3)
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="prefix">Optional folder prefix for organizing files</param>
        /// <returns>Success message with file location</returns>
        /// <response code="200">File successfully uploaded</response>
        /// <response code="400">Invalid file or upload failed</response>
        /// <remarks>
        /// Files are stored in S3 with automatic naming to prevent conflicts.
        /// Maximum file size and allowed formats are configured in application settings.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
		public async Task<IActionResult> UploadFileAsync(IFormFile file , string? prefix)
		{
			var key =  await this.SaveFileGlobalAsync(file, ApplicationConfigurationConstants.Products, fileService, applicationConfigurationService);
			return Ok($"File {prefix}/{key} uploaded to S3 successfully!");
		}

		/// <summary>
        /// Lists all files in storage with optional prefix filtering
        /// </summary>
        /// <param name="prefix">Optional folder prefix to filter files</param>
        /// <returns>List of files with metadata</returns>
        /// <response code="200">Returns list of files</response>
        /// <remarks>
        /// Returns file metadata including:
        /// - File name
        /// - Size
        /// - Last modified date
        /// - Storage path
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(200)]
		public async Task<IActionResult> GetAllFilesAsync(string? prefix)
		{
			var allfiles =  await this.fileService.GetAllFilesAsync(prefix);
			return Ok(allfiles);
		}

		/// <summary>
        /// Retrieves and previews a file by its storage key
        /// </summary>
        /// <param name="key">Unique storage key/path of the file</param>
        /// <returns>File stream with appropriate content type</returns>
        /// <response code="200">Returns the file for preview/download</response>
        /// <response code="404">File not found</response>
        /// <remarks>
        /// This endpoint:
        /// - Supports browser preview for images, PDFs, etc.
        /// - Sets appropriate content-type headers
        /// - Implements client-side caching for 1 hour
        /// - Can be used for both preview and download
        /// </remarks>
        [HttpGet("preview")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, NoStore = false)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFileByKeyAsync(string key)
		{

			var fileObject = await this.fileService.GetFileByKeyAsync(key);
			return File(fileObject.fileStream, fileObject.contentType);
		}

		/// <summary>
        /// Deletes a file from storage
        /// </summary>
        /// <param name="key">Unique storage key/path of the file to delete</param>
        /// <returns>No content on successful deletion</returns>
        /// <response code="204">File successfully deleted</response>
        /// <response code="404">File not found</response>
        /// <response code="403">Insufficient permissions to delete file</response>
        /// <remarks>
        /// Warning: This operation is permanent and cannot be undone.
        /// Ensure proper authorization before deleting files.
        /// </remarks>
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
		public async Task<IActionResult> DeleteFileAsync(string key)
		{
		    await this.fileService.DeleteFileAsync(key);
			return NoContent();
		}



	}
}
