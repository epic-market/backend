using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;

        public AttachmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int InsertOrUpdateAttachment(AttachmentDTO attachmentDTO)
        {
            var attachment = _context.Attachments
                .FirstOrDefault(a => a.ID == attachmentDTO.ID);
            var attachmentTypeID=_context.AttachmentTypes.FirstOrDefault(a => a.Name == attachmentDTO.AttachmentTypeName).ID;
            if (attachment != null)
            {
                attachment.AttachmentTypeID = attachmentTypeID;
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
                    AttachmentTypeID = attachmentTypeID,
                    Name = attachmentDTO.Name,
                    Comment = attachmentDTO.Comment,
                    DocumentType = attachmentDTO.DocumentType,//File
                    DocumentFileType = attachmentDTO.DocumentFileType,
                    DocumentFolderPath = attachmentDTO.DocumentFolderPath,
                    DocumentFile = attachmentDTO.DocumentFile
                };

                _context.Attachments.Add(attachment);
            }

            _context.SaveChanges();
            return attachment.ID;
        }


        public void InsertAttachmentLink(AttachmentLinkDTO attachmentLinkDTO)
        {
            var entityID = _context.Entity.FirstOrDefault(a => a.Name == attachmentLinkDTO.Entity).ID;
            var attachmentLink = new AttachmentLink
            {
                AttachmentID = attachmentLinkDTO.AttachmentID,
                EntityID = entityID,
                RecordID = attachmentLinkDTO.RecordID
            };

            _context.AttachmentLinks.Add(attachmentLink);

            _context.SaveChanges();
        }
    }
}
