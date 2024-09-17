using Amazon.S3;
using Amazon.S3.Model;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
	public class FileService : IFileService
	{
		private readonly IAmazonS3 _s3Client;
        private readonly ApplicationDbContext dbContext;
        private readonly string _bucketName;

		public FileService(IAmazonS3 s3Client, ApplicationDbContext dbContext)
        {
			_s3Client = s3Client;
            this.dbContext = dbContext;
            _bucketName = "epic-market";
		}

        public async Task<bool> DeleteFileAsync( string key)
		{
			await _s3Client.DeleteObjectAsync(_bucketName, key);
			return true;
		}

		public async Task<List<S3ObjectDto>> GetAllFilesAsync(string prefix)
		{
			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
			var request = new ListObjectsV2Request()
			{
				BucketName = _bucketName,
				Prefix = prefix
			};
			var result = await _s3Client.ListObjectsV2Async(request);
			var s3Objects = result.S3Objects.Select(s =>
			{
				var urlRequest = new GetPreSignedUrlRequest()
				{
					BucketName = _bucketName,
					Key = s.Key,
					Expires = DateTime.UtcNow.AddMinutes(1)
				};
				return new S3ObjectDto()
				{
					Name = s.Key.ToString(),
					PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
				};
			}).ToList();
			return s3Objects;
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

		public async Task<string> UploadFileAsync(IFormFile file, string prefix , string fileNameKey)
		{
			
			var request = new PutObjectRequest()
			{
				BucketName = _bucketName,
				Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{fileNameKey}",
				InputStream = file.OpenReadStream()
			};
			request.Metadata.Add("Content-Type", file.ContentType);
			 await _s3Client.PutObjectAsync(request);
			return fileNameKey;
		}


        public async Task<bool> DeleteImage(ListOfImages keys, string UserName)
        {

            var count = 1;
            foreach (var key in keys.ImageKeys)
            {

                int lastSlashIndex = key.LastIndexOf('/');
                string fileName = key.Substring(lastSlashIndex + 1);

                var attachment = await dbContext.Attachments.Where(c => c.DocumentFile == fileName).FirstOrDefaultAsync();

                if (attachment != null)
                {
                    dbContext.Attachments.Remove(attachment);
                    var status = await DeleteFileAsync(key);
					await dbContext.SaveChangesAsync();
					count = status ? count + 1 : count;
                }
                else
                {
                    throw new Exception("File Not Found");
                }
            }
            return keys.ImageKeys.Count + 1 == count;
        }

    }
}
