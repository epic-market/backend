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
        private readonly ApplicationDbContext dbContext;
        private readonly IOnboardingService onboardingService;

        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IOnboardingService onboardingService) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.onboardingService = onboardingService;
        }


            [HttpGet("OnboardingSteps")]
            public async Task<IActionResult> GetAllOnBoardSteps()
            {

                var reponse = new OperationResult<List<OnboardingStepResult>>();

                this.logger.LogInformation("Dashboard Controller -> GetStatusOptions()");

                var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var steps = await this.onboardingService.GetAllOnboardingSteps(UserID);

                this.logger.LogInformation("Dashboard Controller-> GetStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = steps }));

                reponse.Data = steps;

                return Ok(reponse);

            }
        }
}
