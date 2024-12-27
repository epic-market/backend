using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using NVelocity;
using NVelocity.App;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration; // Added for configuration access

namespace EpicMarket.Services
{
  
   public class EmailService
{
    private readonly string _templatePath;
    private readonly string _apiKey; // Added to store API key

    public EmailService(string templatePath, IConfiguration configuration) // Updated constructor to accept IConfiguration
    {
        _templatePath = templatePath;
        _apiKey = configuration["SendGrid:ApiKey"]; // Retrieve API key from appsettings
    }
    
    public async Task SendEmailAsync(string templateName, object model, string toEmail, string subject) {
        try {
            // Load and render the template
            var body = RenderTemplate(templateName, model);

            var client = new SendGridClient(_apiKey); // Use the API key from configuration

            var from = new EmailAddress("akhil@epicmarket.in", "Epic Market");
            var to = new EmailAddress(toEmail);

            // Use the rendered template as HTML content
            var msg = MailHelper.CreateSingleEmail(
                from,
                to, 
                subject,
                plainTextContent: body, // Fallback plain text version
                htmlContent: body // HTML version from template
            );

            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("Email sent successfully");
            }
            else {
                Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
                throw new Exception($"SendGrid API returned status code: {response.StatusCode}");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
    }

    private string RenderTemplate(string templateName, object model)
    {
        var velocityEngine = new VelocityEngine();
        velocityEngine.Init();

        var templatePath = Path.Combine(_templatePath, $"{templateName}.html");
        using (var reader = new StreamReader(templatePath))
        {
            var template = reader.ReadToEnd();
            var context = new VelocityContext();

            // Add each property from the model object to the context individually
            foreach (var prop in model.GetType().GetProperties())
            {
                context.Put(prop.Name, prop.GetValue(model));
            }

            // Add common template variables
            context.Put("Support_Email", "support@epicmarket.com");
            context.Put("Current_Year", DateTime.Now.Year.ToString());
            context.Put("Company_Address", "Epic Market, 123 Main St, City, Country");

            using (var writer = new StringWriter())
            {
                velocityEngine.Evaluate(context, writer, templateName, template);
                return writer.ToString();
            }
        }
    }
}
}

// Class to handle token response
public class TokenResponse
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
}