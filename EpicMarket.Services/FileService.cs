using Amazon.S3;
using Amazon.S3.Model;
using EpicMarket.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
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
		private readonly string _bucketName;

		public FileService(IAmazonS3 s3Client)
        {
			_s3Client = s3Client;
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

	}
}
