using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;
		private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

		public AttachmentService(ApplicationDbContext context,IUnitOfWork unitOfWork, IEntityService entityService)
        {
            _context = context;
			this.unitOfWork = unitOfWork;
            this.entityService = entityService;
		}

        public async Task<int> GetAttachmentId(string key)
        {
            var attachment = await _context.Attachments.FirstOrDefaultAsync(a => a.DocumentFile == key) ;
            if(attachment != null)
            {
                return attachment.ID;
            }
            return 0;
        }

        public async  Task<int> InsertAttachment(AttachmentDTO attachmentDTO)
        {
           var attachment = new Data.Models.Attachment
            {
                Name = attachmentDTO.Name,
                Comment = attachmentDTO.Comment,
                DocumentType = attachmentDTO.DocumentType,//File
                DocumentFileType = attachmentDTO.DocumentFileType,
                DocumentFolderPath = attachmentDTO.DocumentFolderPath,
                DocumentFile = attachmentDTO.DocumentFile,
                EntityId = attachmentDTO.EntityId,
                RecordId = attachmentDTO.RecordId
            };

            await _context.Attachments.AddAsync(attachment);
        
            await unitOfWork.Complete();

			return attachment.ID;
        }


        public async Task InsertAttachmentLink(AttachmentLinkDTO attachmentLinkDTO, int currentBusinessId)
        {
            // First get the attachment to validate business ID
            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(a => a.ID == attachmentLinkDTO.AttachmentID);
  
            var enityID = await this.entityService.GetEntityId(EntityConstants.Business);

            if (attachment == null)
            {
                throw new Exception("Attachment not found");
            }

            // Validate that the attachment belongs to the current business
            if (attachment.EntityId != enityID || attachment.RecordId != currentBusinessId)
            {
                throw new Exception("Attachment is not found for this business");
            }

            var attachmentTypeID = await _context.AttachmentTypes
                .Where(a => a.Name == attachmentLinkDTO.AttachmentTypeName)
                .Select(a => a.ID)
                .FirstOrDefaultAsync();
            
            var entityID = await _context.Entity
                .Where(a => a.Name == attachmentLinkDTO.Entity)
                .Select(a => a.ID)
                .FirstOrDefaultAsync();

            var attachmentLink = new AttachmentLink
            {
                AttachmentID = attachmentLinkDTO.AttachmentID,
                EntityID = entityID,
                RecordID = attachmentLinkDTO.RecordID,
                AttachmentTypeID = attachmentTypeID,
            };

            await _context.AttachmentLinks.AddAsync(attachmentLink);
            await unitOfWork.Complete();
        }
    }
}
