using EpicMarket.Contracts;
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

        public CommunicationService(IApplicationConfigurationService applicationConfigurationService)
        {
            this._applicationConfigurationService = applicationConfigurationService;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var Frommail = "gadamsattiakhil@outlook.com";//this._applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.SMTPFromEmail);
            var pass = "Demo@1234";//this._applicationConfigurationService.GetApplicationConfigurationValue(ApplicationConfigurationConstants.SMTPPassword);
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
                await client.SendMailAsync(mail);
                Console.WriteLine("Mail sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending mail: {ex.Message}");
            }
        }
    }
}
