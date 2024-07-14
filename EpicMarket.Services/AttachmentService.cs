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

        public void InsertOrUpdateAttachment(AttachmentDTO attachmentDTO)
        {
            var existingAttachment = _context.Attachments
                .FirstOrDefault(a => a.ID == attachmentDTO.ID);

            if (existingAttachment != null)
            {
                existingAttachment.AttachmentTypeID = attachmentDTO.AttachmentTypeID;
                existingAttachment.Name = attachmentDTO.Name;
                existingAttachment.Comment = attachmentDTO.Comment;
                existingAttachment.DocumentType = attachmentDTO.DocumentType;
                existingAttachment.DocumentFileType = attachmentDTO.DocumentFileType;
                existingAttachment.DocumentFolderPath = attachmentDTO.DocumentFolderPath;
                existingAttachment.DocumentFile = attachmentDTO.DocumentFile;

                _context.Attachments.Update(existingAttachment);
            }
            else
            {
                var newAttachment = new Attachment
                {
                    AttachmentTypeID = attachmentDTO.AttachmentTypeID,
                    Name = attachmentDTO.Name,
                    Comment = attachmentDTO.Comment,
                    DocumentType = attachmentDTO.DocumentType,
                    DocumentFileType = attachmentDTO.DocumentFileType,
                    DocumentFolderPath = attachmentDTO.DocumentFolderPath,
                    DocumentFile = attachmentDTO.DocumentFile
                };

                _context.Attachments.Add(newAttachment);
            }

            _context.SaveChanges();
        }


        public void InsertAttachmentLink(AttachmentLinkDTO attachmentLinkDTO)
        {
            var attachmentLink = new AttachmentLink
            {
                AttachmentID = attachmentLinkDTO.AttachmentID,
                EntityID = attachmentLinkDTO.EntityID,
                RecordID = attachmentLinkDTO.RecordID
            };

            _context.AttachmentLinks.Add(attachmentLink);

            _context.SaveChanges();
        }
    }
}
