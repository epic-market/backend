using EpicMarket.Contracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EpicMarket.Business.API.Extension
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Gets or sets the securable.
        /// </summary>
        /// <value>
        /// The securable.
        /// </value>
        public string Securable { get; set; }

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="context">Result action.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var username = userService.LoggedInUsername;
            if ((string.IsNullOrWhiteSpace(username) || !string.IsNullOrWhiteSpace(this.Securable)) && !userService.HasPermission(username, this.Securable))
            {
                throw new AccessViolationException("Unauthorized to access");
            }
        }
    }
}
