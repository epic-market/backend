using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface ICommunicationService
    {
        public void SendEmailAsync(string email, string subject, string message);
        
        public Task SendTemplatedEmailAsync(string email, string subject, string templateName, object model, List<EmailAttachmentModel> attachments = null);
    }
}
