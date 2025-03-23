using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IApplicationConfigurationService _applicationConfigurationService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ICommunicationQueueService _communicationQueueService;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext context;
        private const int MAX_RETRY_COUNT = 3;

        public CommunicationService(
            IApplicationConfigurationService applicationConfigurationService,
            IEmailTemplateService emailTemplateService,
            ICommunicationQueueService communicationQueueService,
            IEmailService emailService,
            ApplicationDbContext context)
        {
            _applicationConfigurationService = applicationConfigurationService;
            _emailTemplateService = emailTemplateService;
            _communicationQueueService = communicationQueueService;
            _emailService = emailService;
            this.context = context;
        }

        public void SendEmailAsync(string email, string subject, string message)
        {
            var Frommail = "gadamsattiakhil@outlook.com";//this._applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.SMTPFromEmail);
            var pass = "Epicmarket@2024";//this._applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.SMTPPassword);
            var server = "smtp-mail.outlook.com";//this._applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.SMTPServer);
            var port = 587;//Convert.ToInt32(this._applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.SMTPPort));

            MailMessage mail = new MailMessage(from: Frommail, to: email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            var client = new SmtpClient()
            {
                Host = server,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(Frommail, pass),
            };

            try
            {
                client.Send(mail);
                Console.WriteLine("Mail sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending mail: {ex.Message}");
            }
        }

        public async Task SendTemplatedEmailAsync(string email, string subject, string templateName, object model, List<EmailAttachmentModel> attachments = null)
        {
            try
            {
                // Render the email template with the provided model
                string emailBody = await _emailTemplateService.RenderEmailTemplate(templateName, model);

                // Create and queue the communication
                var communicationQueue = new CommunicationQueueDTO
                {
                    ToEmail = email,
                    Subject = subject,
                    Body = emailBody,
                    TemplateName = templateName,
                    CommunicationStatusId = context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Queued).ID,
                    CreatedDate = DateTime.UtcNow,
                    RetryCount = 0,
                    // Add any other necessary properties
                };

                // Insert into communication queue
                await _communicationQueueService.InsertCommunicationQueue(communicationQueue);

                // Attempt to send the email
                await ProcessEmailSendingAsync(communicationQueue.ID, email, subject, emailBody, attachments);
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error in SendTemplatedEmailAsync: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessEmailSendingAsync(int communicationQueueId, string email, string subject, string body, List<EmailAttachmentModel> attachments = null)
        {
            try
            {
                // Send email using email service
                await _emailService.SendEmailAsync(email, subject, body, attachments);

                // Update status to sent
                await _communicationQueueService.UpdateCommunicationQueueStatus(communicationQueueId, context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Sent).ID);
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error sending email: {ex.Message}");

                // Get current retry count
                var retryCount = await _communicationQueueService.GetCommunicationQueueRetryCount(communicationQueueId);

                if (retryCount < MAX_RETRY_COUNT)
                {
                    // Update status to retrying and increment retry count
                    await _communicationQueueService.UpdateCommunicationQueueStatusAndRetry(communicationQueueId, context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Retrying).ID, retryCount + 1);

                    // Retry sending (possibly with delay in a real implementation)
                    await ProcessEmailSendingAsync(communicationQueueId, email, subject, body, attachments);
                }
                else
                {
                    // Max retries exceeded, update status to failed
                    await _communicationQueueService.UpdateCommunicationQueueStatus(communicationQueueId, context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Failed).ID);
                }
            }
        }

    }
}
