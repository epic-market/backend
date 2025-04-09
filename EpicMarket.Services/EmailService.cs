using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Contracts;

namespace EpicMarket.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _fromEmail = configuration["SendGrid:FromEmail"] ?? "akhil@epicmarket.in";
            _fromName = configuration["SendGrid:FromName"] ?? "Epic Market";
        }
        
        public async Task SendEmailAsync(string toEmail, string subject, string body, List<EmailAttachmentModel> attachments = null)
        {
            try {
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress(_fromEmail, _fromName);
                var to = new EmailAddress(toEmail);

                var msg = MailHelper.CreateSingleEmail(
                    from,
                    to, 
                    subject,
                    plainTextContent: body, // Plain text version
                    htmlContent: body // HTML version
                );
                
                // Add attachments if any
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        msg.AddAttachment(attachment.Filename, attachment.Content, 
                            attachment.Type, attachment.Disposition, attachment.ContentId);
                    }
                }

                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"SendGrid API returned status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
    
    // Attachment class to simplify adding attachments
}

// Class to handle token response
public class TokenResponse
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
}