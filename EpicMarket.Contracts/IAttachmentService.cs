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
        Task<int> InsertAttachment(AttachmentDTO attachment);
		Task InsertAttachmentLink(AttachmentLinkDTO attachmentLinkDTO, int currentBusinessId);
        Task<int> GetAttachmentId(string key);
    }
}
