using Azure;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace EpicMarket.Admin.MVC.Services
{
	public class AttachmentService : IAttachmentService
	{
		private readonly ApplicationDbContext _context;
		private readonly IFileService fileService;
		private readonly IApplicationConfigurationService applicationConfigurationService;

		public AttachmentService(ApplicationDbContext applicationDbContext, IFileService fileService, IApplicationConfigurationService applicationConfigurationService) {
			_context = applicationDbContext;
			this.fileService = fileService;
			this.applicationConfigurationService = applicationConfigurationService;
		}


		public async Task UploadAttachment(AttachmentModel attachmentModel)
		{
			if (attachmentModel.Files?.Length > 0)
			{
				foreach (var product in attachmentModel.Files)
				{
					var insertedKey = await this.SaveFileBusinessAsync(product, attachmentModel.BusinessID);
					var attachmentId = await this.GetAttachmentId(insertedKey.Key);
					await InsertAttachmentLink(new AttachmentLinkDTO()
					{
						AttachmentTypeName = attachmentModel.AttachmentType,
						AttachmentID = attachmentId,
						Entity = attachmentModel.Entity,
						RecordID = attachmentModel.RecordID
					});

				}
			}
		}

	public async Task<int> GetAttachmentId(string key)
	{
		var attachment = await _context.Attachments.FirstOrDefaultAsync(a => a.DocumentFile == key);
		return attachment.ID;
	}
	



		// private async Task<SaveFileDTO> SaveFileGlobalAsync(IFormFile file, int RecordID = 0)
		// {
		// 	string filePath = " ";
		// 	if (file != null && file.Length > 0)
		// 	{
		// 		string fullPathLocation = null;
		// 		string subPathLocation = null;

		// 		if (!string.IsNullOrWhiteSpace(FolderPathConstant))
		// 		{
		// 			fullPathLocation = applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.BasePath);
		// 			subPathLocation = _context.ApplicationConfigurations.Where(c => c.Name == FolderPathConstant).FirstOrDefault().Value;
		// 			if (RecordID != 0)
		// 			{
		// 				fullPathLocation = fullPathLocation + "/" + RecordID + "/" + subPathLocation;
		// 			}
		// 			if (!fullPathLocation.EndsWith("/"))
		// 			{
		// 				fullPathLocation = fullPathLocation + "/";
		// 			}
		// 		}
		// 		// Made fitting based on AWS
		// 		var uploadedFile = file;

		// 		var fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
		// 		string type = Path.GetExtension(uploadedFile.FileName);
		// 		fileName += "_" + DateTime.Now.Ticks.ToString() + type;

		// 		fileName = fileName.Replace('&', '_').Replace('<', '_').Replace('>', '_');
		// 		filePath = await fileService.UploadFileAsync(uploadedFile, fullPathLocation, fileName);

		// 		return new SaveFileDTO()
		// 		{
		// 			//FileName = fileName,
		// 			//FullPathLocation = fullPathLocation.ToString()
		// 		};
		// 	}

		// 	return null;
		// }


 		private async Task<SaveFileDTO> SaveFileBusinessAsync(IFormFile file,int BusinessId)
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
				var key =  await fileService.UploadFileAsync(uploadedFile, fullPathLocation, fileName, EntityConstants.Business, BusinessId);

				return new SaveFileDTO()
				{
					Key = key
				};
			}

			return null;
		}




	
		public async Task InsertAttachmentLink(AttachmentLinkDTO attachmentLinkDTO)
		{
			var attachmentTypeID = _context.AttachmentTypes.FirstOrDefault(a => a.Name == attachmentLinkDTO.AttachmentTypeName).ID;
			var entityID = _context.Entity.FirstOrDefault(a => a.Name == attachmentLinkDTO.Entity).ID;
			var attachmentLink = new AttachmentLink
			{
				AttachmentID = attachmentLinkDTO.AttachmentID,
				EntityID = entityID,
				RecordID = attachmentLinkDTO.RecordID,
				AttachmentTypeID = attachmentTypeID,
			};

			await _context.AttachmentLinks.AddAsync(attachmentLink);
			await _context.SaveChangesAsync(); ;
		}


		public async Task<int> InsertOrUpdateAttachment(AttachmentDTO attachmentDTO)
		{
			var attachment = _context.Attachments
				.FirstOrDefault(a => a.ID == attachmentDTO.ID);
			if (attachment != null)
			{
				attachment.Name = attachmentDTO.Name;
				attachment.Comment = attachmentDTO.Comment;
				attachment.DocumentType = attachmentDTO.DocumentType;
				attachment.DocumentFileType = attachmentDTO.DocumentFileType;
				attachment.DocumentFolderPath = attachmentDTO.DocumentFolderPath;
				attachment.DocumentFile = attachmentDTO.DocumentFile;

				_context.Attachments.Update(attachment);
			}
			else
			{
				attachment = new Attachment
				{
					Name = attachmentDTO.Name,
					Comment = attachmentDTO.Comment,
					DocumentType = attachmentDTO.DocumentType,//File
					DocumentFileType = attachmentDTO.DocumentFileType,
					DocumentFolderPath = attachmentDTO.DocumentFolderPath,
					DocumentFile = attachmentDTO.DocumentFile
				};

				await _context.Attachments.AddAsync(attachment);
			}

			await _context.SaveChangesAsync();
			return attachment.ID;
		}

        public async  Task<List<string>> GetAttachmentLinks(GetAttachmentLink getAttachmentLink) 
        {

            var attachmentTypeID = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == getAttachmentLink.AttachmentType);


            var attachments = from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            where entity.Name == getAttachmentLink.Entity && link.RecordID == getAttachmentLink.RecordID && link.AttachmentTypeID == attachmentTypeID.ID
                            orderby attachment.CreateDate descending
                            select new
                            {
                                ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                            };

			return attachments.Select(a => a.ImagePath).ToList();
        }

        public Task DeleteAttachment(AttachmentDTO attachmentDTO)
        {
            throw new NotImplementedException();
        }
    }
}
