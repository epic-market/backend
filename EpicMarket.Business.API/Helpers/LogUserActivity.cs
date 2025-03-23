using Amazon.Runtime.Internal.Util;
using EpicMarket.Business.API.Extension;
using EpicMarket.Contracts;
using EpicMarket.Entities.Constants;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EpicMarket.Business.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var IsBusinessRole = resultContext.HttpContext.User.IsInRole(ROLES.BUSINESS_OWNER);
         
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            if (IsBusinessRole)
            {
                if (!uow.UserRepository.IsBusinessVerified(userId)) 
                {
                    throw new Exception("Your business Is Still not Verified");
                };
            }
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}
