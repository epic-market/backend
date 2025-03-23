using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicMarket.Contracts;
using Microsoft.Extensions.Configuration;
using NVelocity;
using NVelocity.App;

namespace EpicMarket.Services.EmailTemplateServices
{
    public class EmailTemplateService : IEmailTemplateService   
    {
        private readonly IConfiguration _configuration;
        public EmailTemplateService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
         public  async Task<string> RenderEmailTemplate(string templateName, object model)
        {

            var EmailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EmailTemplates");
            var velocityEngine = new VelocityEngine();
            velocityEngine.Init();

            var templatePath = Path.Combine(EmailTemplatePath, $"{templateName}.html");
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