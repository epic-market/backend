using EpicMarket.Admin.MVC.Models;
using EpicMarket.Entities;
using Microsoft.NET.StringTools;

namespace EpicMarket.Admin.MVC.Contracts
{
	public interface IAttachmentService
	{
		Task UploadAttachment(AttachmentModel attachmentModel);
		Task DeleteAttachment(AttachmentDTO attachmentDTO);

		Task<List<string>> GetAttachmentLinks(GetAttachmentLink getAttachmentLink);

    }
}
