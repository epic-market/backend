using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Contracts
{
	public interface IFileService
	{
		Task<string> UploadFileAsync(IFormFile file, string prefix , string fileNameKey, string EntityName, int RecordId);


		Task<List<S3ObjectDto>> GetAllFilesAsync(string prefix);

		Task<FileDto> GetFileByKeyAsync(string key);

		Task<bool> DeleteFileAsync(string key);

		Task<bool> DeleteImage(List<string> ImageKeys, string UserName);


    }
}
