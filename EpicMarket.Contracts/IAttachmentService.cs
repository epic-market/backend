using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IAttachmentService
    {
        int InsertOrUpdateAttachment(AttachmentDTO attachment);
        void InsertAttachmentLink(AttachmentLinkDTO attachmentLinkDTO);
    }
}
