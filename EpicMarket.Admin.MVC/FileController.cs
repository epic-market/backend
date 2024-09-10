using Amazon.S3;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicMarket.Admin.MVC
{
    public class FileController: Controller
    {

        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public FileController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = "epic-market";
        }

        public async Task<ActionResult> DisplayImage(string key)
        {
            try
            {
                var fileDto = await GetFileByKeyAsync(key);
                return File(fileDto.fileStream, fileDto.contentType);
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found
                return Content("Error: " + ex.Message);
            }
        }


        public async Task<FileDto> GetFileByKeyAsync(string key)
        {
            var s3Object = await _s3Client.GetObjectAsync(_bucketName, key);
            return new FileDto()
            {
                fileStream = s3Object.ResponseStream,
                contentType = s3Object.Headers.ContentType
            };

        }
        }
}
