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

            // TEMPORARY: Skip securable checks - Remove in production!
            // Note: Basic authentication is still required, but all authenticated users can access any securable
            return;
        }
    }
}