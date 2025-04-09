using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Contracts
{    // Interface for the email service
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, List<EmailAttachmentModel> attachments = null);
    }

}