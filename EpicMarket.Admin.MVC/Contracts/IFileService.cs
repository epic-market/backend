using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
	public interface IFileService
	{
		Task<string> UploadFileAsync(IFormFile file, string prefix , string fileNameKey);

		Task<List<S3ObjectDto>> GetAllFilesAsync(string prefix);

		Task<FileDto> GetFileByKeyAsync(string key);

		Task<bool> DeleteFileAsync(string key);

		Task<bool> DeleteImage(ListOfImages keys, string UserName);


    }
}
