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
    [Route("api/review")]
    public class ReviewController : BaseApiController
    {
        private readonly ILogger<ReviewController> logger;

        private readonly IRatingService ratingService;
        public ReviewController(ILogger<ReviewController> logger, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IRatingService ratingService) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.ratingService = ratingService;
        }


            [HttpGet("outlet/{outletId}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetAllReviewsOutlet(int outletId)
            {

                var reponse = new OperationResult<List<ReviewResult>>();

                this.logger.LogInformation("Review Controller -> GetAllReviewsOutlet()");

                var steps = await this.ratingService.GetAllReviewsOutlet(outletId);

                this.logger.LogInformation("Review Controller-> GetAllReviewsOutlet()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = steps }));

                reponse.Data = steps;

                return Ok(reponse);

            }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllReviewsProduct(int productId)
        {
            var response = new OperationResult<List<ReviewResult>>();

            this.logger.LogInformation("Review Controller -> GetAllReviewsProduct()");

            var steps = await this.ratingService.GetAllReviewsProduct(productId);

            this.logger.LogInformation("Review Controller-> GetAllReviewsProduct()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = steps }));

            response.Data = steps;

            return Ok(response);
        }

        [HttpPost("outlet")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddRatingToOutlet([FromBody] AddOutletRatingRequest request)
        {
            var response = new OperationResult<bool>();
            this.logger.LogInformation("Products Controller -> GetAllProducts()-> params {0}", JsonConvert.SerializeObject(new { Params = request }));
            await ratingService.AddOutletRatingAsync(request, "test");
            response.Data = true;
            return Ok(response);
        }
            
        [HttpPost("product")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddRatingToProduct([FromBody] AddProductRatingRequest request)
        {
            var response = new OperationResult<bool>();
            this.logger.LogInformation("Review Controller -> AddRatingToProduct()-> params {0}", JsonConvert.SerializeObject(new { Params = request }));
            await ratingService.AddProductRatingAsync(request, "test");
            response.Data = true;
            return Ok(response);
        }
    }
}
