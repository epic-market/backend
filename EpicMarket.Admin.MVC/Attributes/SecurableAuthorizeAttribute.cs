using EpicMarket.Admin.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EpicMarket.Admin.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SecurableAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Securable { get; set; }

        public SecurableAuthorizeAttribute(string securable)
        {
            Securable = securable ?? throw new ArgumentNullException(nameof(securable));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action is decorated with [AllowAnonymous]
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            // Check if user is authenticated
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "Identity", returnUrl = context.HttpContext.Request.Path });
                return;
            }

            // Get the securable service from DI
            var securableService = context.HttpContext.RequestServices.GetRequiredService<ISecurableService>();
            
            // Check if the user has access to the securable
            if (!securableService.HasAccess(Securable))
            {
                // Set ViewData for the access denied view
                context.HttpContext.Items["AccessDeniedSecurable"] = Securable;
                
                // Redirect to access denied view
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/_AccessDeniedPartial.cshtml",
                    ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                        new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                        new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                    {
                        Model = Securable,
                        ["AccessDeniedMessage"] = $"You do not have permission to access this feature ({Securable})."
                    }
                };
            }
        }
    }
} 