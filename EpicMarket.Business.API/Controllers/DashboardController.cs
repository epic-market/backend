using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using EpicMarket.Contracts;

namespace EpicMarket.Business.API.Controllers
{

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


            [HttpGet("OnboardingSteps")]
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
   

            [HttpGet("active-users/{outletId}")]
            public async Task<IActionResult> GetActiveUsers(int outletId)
            {
                var response = new OperationResult<(List<ActiveUserChart> Monthly, List<ActiveUserChart> Weekly)>();
                
                this.logger.LogInformation($"Dashboard Controller -> GetActiveUsers({outletId})");

                var result = await this.dashboardService.GetActiveUsers(outletId);
                
                this.logger.LogInformation($"Dashboard Controller -> GetActiveUsers -> Returning data with {result.Monthly.Count} monthly and {result.Weekly.Count} weekly entries");
                
                response.Data = result;
                return Ok(response);
            }

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
