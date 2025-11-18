using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Contracts;

namespace EpicMarket.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiToken;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiToken = configuration["Mailtrap:ApiToken"] ?? throw new ArgumentNullException("Mailtrap:ApiToken");
            _fromEmail = configuration["Mailtrap:FromEmail"] ?? "hello@epicmarket.in";
            _fromName = configuration["Mailtrap:FromName"] ?? "Epic Market";
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
            // Configure HttpClient base address for Mailtrap API
            _httpClient.BaseAddress = new Uri("https://send.api.mailtrap.io");
        }
        
        public async Task SendEmailAsync(string toEmail, string subject, string body, List<EmailAttachmentModel> attachments = null)
        {
            try
            {
                // Determine if body is HTML or plain text
                bool isHtml = body.Contains("<html", StringComparison.OrdinalIgnoreCase) || 
                             body.Contains("<!DOCTYPE", StringComparison.OrdinalIgnoreCase) ||
                             body.Contains("<body", StringComparison.OrdinalIgnoreCase);

                // Build Mailtrap API request payload
                var payload = new
                {
                    from = new
                    {
                        email = _fromEmail,
                        name = _fromName
                    },
                    to = new[]
                    {
                        new { email = toEmail }
                    },
                    subject = subject,
                    text = isHtml ? null : body,
                    html = isHtml ? body : null,
                    category = "EpicMarket Application",
                    attachments = attachments != null && attachments.Count > 0
                        ? attachments.Select(a => new
                        {
                            content = a.Content, // Should be base64 encoded
                            filename = a.Filename,
                            type = a.Type ?? "application/octet-stream",
                            disposition = a.Disposition ?? "attachment"
                        }).ToArray()
                        : null
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Create request with proper headers
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/send")
                {
                    Content = content
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Mailtrap API returned status code: {response.StatusCode}. Response: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}