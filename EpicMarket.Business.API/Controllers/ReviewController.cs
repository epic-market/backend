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
    /// Manages customer reviews and ratings for outlets and products.
    /// Provides endpoints to fetch reviews and add new ratings.
    /// </summary>
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


        /// <summary>
        /// Gets all reviews for the specified outlet.
        /// Route: GET api/review/outlet/{outletId}
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="outletId">Identifier of the outlet whose reviews will be returned.</param>
        /// <returns>List of outlet reviews.</returns>
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

        /// <summary>
        /// Gets all reviews for the specified product.
        /// Route: GET api/review/product/{productId}
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="productId">Identifier of the product whose reviews will be returned.</param>
        /// <returns>List of product reviews.</returns>
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

        /// <summary>
        /// Adds a rating and optional review for the specified outlet.
        /// Route: POST api/review/outlet
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="request">Rating details for the outlet.</param>
        /// <returns>Status of the rating operation.</returns>
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
        
        /// <summary>
        /// Adds a rating and optional review for the specified product.
        /// Route: POST api/review/product
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="request">Rating details for the product.</param>
        /// <returns>Status of the rating operation.</returns>
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
