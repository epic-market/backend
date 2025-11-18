using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using EpicMarket.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Dashboard analytics API. Provides onboarding progress and business performance metrics for outlets.
    /// </summary>
    /// <remarks>
    /// Route prefix: <c>api/dashboard</c>
    /// </remarks>
    [Route("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        private readonly ILogger<DashboardController> logger;
        private readonly IOnboardingService onboardingService;

        private readonly IDashboardService dashboardService;

        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IOnboardingService onboardingService, IDashboardService dashboardService) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.onboardingService = onboardingService;
            this.dashboardService = dashboardService;
        }


            /// <summary>
            /// Gets onboarding checklist steps for the currently authenticated user.
            /// </summary>
            /// <remarks>
            /// Route: <c>GET api/dashboard/OnboardingSteps</c>
            /// Auth: <c>Authorize</c>
            /// </remarks>
            [HttpGet("OnboardingSteps")]
            [Authorize]
            public async Task<IActionResult> GetAllOnBoardSteps()
            {

                var reponse = new OperationResult<List<OnboardingStepResult>>();

                this.logger.LogInformation("Dashboard Controller -> OnboardingSteps()");

                var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var steps = await this.onboardingService.GetAllOnboardingSteps(UserID);

                this.logger.LogInformation("Dashboard Controller-> OnboardingSteps()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = steps }));

                reponse.Data = steps;

                return Ok(reponse);

            }


            /// <summary>
            /// Marks an onboarding step as completed for the current user.
            /// </summary>
            /// <remarks>
            /// Route: <c>POST api/dashboard/completed</c>
            /// Auth: <c>Authorize</c>
            /// Query/Form: <c>stepId</c> identifier.
            /// </remarks>
            [HttpPost("completed")]
            public async Task<IActionResult> UpdateStatusOfOnBoardingStep(int stepId)
            {

                var reponse = new OperationResult<bool>();

                this.logger.LogInformation("Dashboard Controller -> UpdateStatusOfOnBoardingStep()");

                var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
                await this.onboardingService.CompleteStepForUserID(UserID,stepId);

                this.logger.LogInformation("Dashboard Controller-> UpdateStatusOfOnBoardingStep()");

                reponse.Data = true;

                return Ok(reponse);

            }
   

            /// <summary>
            /// Gets weekly and monthly active user counts for an outlet.
            /// </summary>
            /// <remarks>
            /// Route: <c>GET api/dashboard/active-users/{{outletId}}</c>
            /// Auth: <c>Authorize</c>
            /// </remarks>
            [HttpGet("active-users/{outletId}")]
            public async Task<IActionResult> GetActiveUsers(int outletId)
            {
                var response = new OperationResult<ActiveUserChartResponse>();
                
                this.logger.LogInformation($"Dashboard Controller -> GetActiveUsers({outletId})");

                var result = await this.dashboardService.GetActiveUsers(outletId);
                
                this.logger.LogInformation($"Dashboard Controller -> GetActiveUsers -> Returning data with {result.Monthly.Count} monthly and {result.Weekly.Count} weekly entries");
                
                response.Data = result;
                return Ok(response);
            }

            /// <summary>
            /// Retrieves gross merchandise value (GMV) trend data for an outlet.
            /// </summary>
            /// <remarks>
            /// Route: <c>GET api/dashboard/gmv/{{outletId}}</c>
            /// Auth: <c>Authorize</c>
            /// </remarks>
            [HttpGet("gmv/{outletId}")]
            public async Task<IActionResult> GetGrossMerchandiseValue(int outletId)
            {
                var response = new OperationResult<List<GMVChart>>();
                
                this.logger.LogInformation($"Dashboard Controller -> GetGrossMerchandiseValue({outletId})");

                var result = await this.dashboardService.GetGrossMerchandiseValue(outletId);
                
                this.logger.LogInformation($"Dashboard Controller -> GetGrossMerchandiseValue -> Returning {result.Count} GMV entries");
                
                response.Data = result;
                return Ok(response);
            }

            /// <summary>
            /// Gets the customer retention rate percentage for an outlet.
            /// </summary>
            /// <remarks>
            /// Route: <c>GET api/dashboard/retention-rate/{{outletId}}</c>
            /// Auth: <c>Authorize</c>
            /// </remarks>
            [HttpGet("retention-rate/{outletId}")]
            public async Task<IActionResult> GetCustomerRetentionRate(int outletId)
            {
                var response = new OperationResult<decimal>();
                
                this.logger.LogInformation($"Dashboard Controller -> GetCustomerRetentionRate({outletId})");

                var result = await this.dashboardService.GetCustomerRetentionRate(outletId);
                
                this.logger.LogInformation($"Dashboard Controller -> GetCustomerRetentionRate -> Returning rate: {result}%");
                
                response.Data = result;
                return Ok(response);
            }

            /// <summary>
            /// Retrieves the top selling products for an outlet.
            /// </summary>
            /// <remarks>
            /// Route: <c>GET api/dashboard/top-products/{{outletId}}</c>
            /// Auth: <c>Authorize</c>
            /// </remarks>
            [HttpGet("top-products/{outletId}")]
            public async Task<IActionResult> GetTopSellingProducts(int outletId)
            {
                var response = new OperationResult<List<PopularProductChart>>();
                
                this.logger.LogInformation($"Dashboard Controller -> GetTopSellingProducts({outletId})");

                var result = await this.dashboardService.GetTopSellingProducts(outletId);
                
                this.logger.LogInformation($"Dashboard Controller -> GetTopSellingProducts -> Returning {result.Count} products");
                
                response.Data = result;
                return Ok(response);
            }

            /// <summary>
            /// Gets order status distribution for the specified date and aggregation period.
            /// </summary>
            /// <remarks>
            /// Route: <c>GET api/dashboard/order-status/{{outletId}}</c>
            /// Auth: <c>Authorize</c>
            /// Query: <c>date</c>, optional <c>period</c> (day/week/month).
            /// </remarks>
            [HttpGet("order-status/{outletId}")]
            public async Task<IActionResult> GetOrderStatusDistribution(int outletId, [FromQuery] DateTime date, [FromQuery] string period = "day")
            {
                var response = new OperationResult<Dictionary<string, int>>();
                
                this.logger.LogInformation($"Dashboard Controller -> GetOrderStatusDistribution({outletId}, {date}, {period})");

                var result = await this.dashboardService.GetOrderStatusDistribution(date, outletId, period);
                
                this.logger.LogInformation($"Dashboard Controller -> GetOrderStatusDistribution -> Returning {result.Count} status entries");
                
                response.Data = result;
                return Ok(response);
            }


            
    }
}
