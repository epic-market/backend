using EpicMarket.Admin.MVC.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;

namespace EpicMarket.Admin.MVC.TagHelpers
{
    [HtmlTargetElement(Attributes = "securable")]
    public class SecurableTagHelper : TagHelper
    {
        private readonly ISecurableService _securableService;
        private readonly ILogger<SecurableTagHelper> _logger;

        public SecurableTagHelper(ISecurableService securableService, ILogger<SecurableTagHelper> logger)
        {
            _securableService = securableService ?? throw new ArgumentNullException(nameof(securableService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HtmlAttributeName("securable")]
        public string Securable { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Securable))
            {
                _logger.LogWarning("Empty securable attribute found");
                output.SuppressOutput();
                return;
            }

            try
            {
                if (!_securableService.HasAccess(Securable))
                {
                    output.SuppressOutput();
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't crash the application
                _logger.LogError(ex, "Error checking securable permission '{Securable}'", Securable);
                
                // Default to hiding the element if there's an error
                output.SuppressOutput();
            }
        }
    }
} 