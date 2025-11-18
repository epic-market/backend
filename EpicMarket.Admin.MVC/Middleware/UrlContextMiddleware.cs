using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Middleware
{
    public class UrlContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UrlContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UrlContextService urlContextService)
        {
            var request = context.Request;
            var uriBuilder = new UriBuilder(
                request.Scheme, 
                request.Host.Host, 
                request.Host.Port ?? -1)
            {
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            
            uriBuilder.Port = uriBuilder.Uri.IsDefaultPort ? -1 : uriBuilder.Port;
            urlContextService.CurrentPageUrl = uriBuilder.Uri.AbsoluteUri;

            await _next(context);
        }
    }

    // Extension method to make registration cleaner
    public static class UrlContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseUrlContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UrlContextMiddleware>();
        }
    }
} 