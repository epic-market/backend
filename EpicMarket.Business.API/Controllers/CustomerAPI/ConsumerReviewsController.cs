using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomerAPI;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers.CustomerAPI
{
    /// <summary>
    /// Consumer-facing Reviews API.
    /// Provides endpoints for getting and submitting reviews for outlets and products.
    /// </summary>
    [Route("api/reviews")]
    [ApiController]
    public class ConsumerReviewsController : ControllerBase
    {
        private readonly ILogger<ConsumerReviewsController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRatingService _ratingService;

        public ConsumerReviewsController(
            ILogger<ConsumerReviewsController> logger,
            ApplicationDbContext dbContext,
            UserManager<AppUser> userManager,
            IRatingService ratingService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
            _ratingService = ratingService;
        }

        /// <summary>
        /// Get all reviews for a specific outlet.
        /// </summary>
        /// <remarks>
        /// Route: GET api/reviews/outlet/{outletId}
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="outletId">Outlet ID</param>
        /// <returns>List of outlet reviews</returns>
        [HttpGet("outlet/{outletId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<ReviewResponse>>>> GetOutletReviews(int outletId)
        {
            var response = new OperationResult<List<ReviewResponse>>();

            try
            {
                _logger.LogInformation("ConsumerReviewsController -> GetOutletReviews({outletId})", outletId);

                var reviews = await _dbContext.Ratings
                    .Include(r => r.Customer)
                    .Where(r => r.OutletId == outletId)
                    .OrderByDescending(r => r.CreatedDate)
                    .Select(r => new ReviewResponse
                    {
                        Id = r.Id,
                        RatingId = r.Id,
                        UserId = r.CustomerId,
                        RatingValue = r.Stars,
                        Review = r.Review,
                        RatingType = "Outlet",
                        EntityId = outletId,
                        CustomerName = $"{r.Customer.FirstName} {r.Customer.LastName}",
                        Rating = r.Stars.ToString(),
                        Date = r.CreatedDate.ToString("yyyy-MM-dd"),
                        OutletId = r.OutletId,
                        ProductId = null,
                        IsActive = r.IsVerified,
                        CreatedDate = r.CreatedDate
                    })
                    .ToListAsync();

                response.Message = "Success";
                response.Data = reviews;

                _logger.LogInformation("ConsumerReviewsController -> GetOutletReviews() -> Found {count} reviews", reviews.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for outlet: {outletId}", outletId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get all reviews for a specific product.
        /// </summary>
        /// <remarks>
        /// Route: GET api/reviews/product/{productId}
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="productId">Product ID</param>
        /// <returns>List of product reviews</returns>
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<ReviewResponse>>>> GetProductReviews(int productId)
        {
            var response = new OperationResult<List<ReviewResponse>>();

            try
            {
                _logger.LogInformation("ConsumerReviewsController -> GetProductReviews({productId})", productId);

                var reviews = await _dbContext.Ratings
                    .Include(r => r.Customer)
                    .Where(r => r.ProductId == productId)
                    .OrderByDescending(r => r.CreatedDate)
                    .Select(r => new ReviewResponse
                    {
                        Id = r.Id,
                        RatingId = r.Id,
                        UserId = r.CustomerId,
                        RatingValue = r.Stars,
                        Review = r.Review,
                        RatingType = "Product",
                        EntityId = productId,
                        CustomerName = $"{r.Customer.FirstName} {r.Customer.LastName}",
                        Rating = r.Stars.ToString(),
                        Date = r.CreatedDate.ToString("yyyy-MM-dd"),
                        OutletId = null,
                        ProductId = r.ProductId,
                        IsActive = r.IsVerified,
                        CreatedDate = r.CreatedDate
                    })
                    .ToListAsync();

                response.Message = "Success";
                response.Data = reviews;

                _logger.LogInformation("ConsumerReviewsController -> GetProductReviews() -> Found {count} reviews", reviews.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for product: {productId}", productId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Submit a new review for a product or outlet.
        /// </summary>
        /// <remarks>
        /// Route: POST api/reviews
        /// Auth: Authorize
        /// </remarks>
        /// <param name="request">Review submission details</param>
        /// <returns>Submitted review details</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OperationResult<ReviewResponse>>> SubmitReview([FromBody] SubmitReviewRequest request)
        {
            var response = new OperationResult<ReviewResponse>();

            try
            {
                _logger.LogInformation("ConsumerReviewsController -> SubmitReview() -> reviewType: {reviewType}, recordId: {recordId}", 
                    request.ReviewType, request.RecordId);

                // Validate review type
                var reviewType = request.ReviewType?.ToLower();
                if (reviewType != "product" && reviewType != "outlet")
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid review type";
                    response.ErrorDetail = "Review type must be 'product' or 'outlet'";
                    return BadRequest(response);
                }

                // Parse record ID
                if (!int.TryParse(request.RecordId, out int entityId))
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid record ID";
                    response.ErrorDetail = "Record ID must be a valid number";
                    return BadRequest(response);
                }

                // Get current user
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    return Unauthorized(response);
                }

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "User not found";
                    return NotFound(response);
                }

                // Check if entity exists
                var entityType = reviewType == "product" ? "Product" : "Outlet";
                bool entityExists;
                
                if (reviewType == "product")
                {
                    entityExists = await _dbContext.Products.AnyAsync(p => p.ID == entityId && p.IsActive);
                }
                else
                {
                    entityExists = await _dbContext.Outlets.AnyAsync(o => o.ID == entityId && o.IsActive);
                }

                if (!entityExists)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = $"{entityType} not found";
                    response.ErrorDetail = $"No {entityType.ToLower()} found with ID: {entityId}";
                    return NotFound(response);
                }

                // Check for existing review from this user
                Rating existingReview;
                if (reviewType == "product")
                {
                    existingReview = await _dbContext.Ratings
                        .FirstOrDefaultAsync(r => r.CustomerId == userId && r.ProductId == entityId);
                }
                else
                {
                    existingReview = await _dbContext.Ratings
                        .FirstOrDefaultAsync(r => r.CustomerId == userId && r.OutletId == entityId);
                }

                Rating rating;
                
                if (existingReview != null)
                {
                    // Update existing review
                    existingReview.Stars = request.Rating;
                    existingReview.Review = request.Comment;
                    existingReview.ModifiedDate = DateTime.UtcNow;
                    
                    _dbContext.Ratings.Update(existingReview);
                    rating = existingReview;
                }
                else
                {
                    // Create new review
                    rating = new Rating
                    {
                        CustomerId = userId,
                        ProductId = reviewType == "product" ? entityId : null,
                        OutletId = reviewType == "outlet" ? entityId : null,
                        Stars = request.Rating,
                        Review = request.Comment,
                        IsVerified = false,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    await _dbContext.Ratings.AddAsync(rating);
                }

                await _dbContext.SaveChangesAsync();

                response.Message = "Review submitted successfully";
                response.Data = new ReviewResponse
                {
                    Id = rating.Id,
                    RatingId = rating.Id,
                    UserId = rating.CustomerId,
                    RatingValue = rating.Stars,
                    Review = rating.Review,
                    RatingType = entityType,
                    EntityId = entityId,
                    CustomerName = $"{user.FirstName} {user.LastName}",
                    Rating = rating.Stars.ToString(),
                    Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    OutletId = rating.OutletId,
                    ProductId = rating.ProductId,
                    IsActive = rating.IsVerified,
                    CreatedDate = rating.CreatedDate
                };

                _logger.LogInformation("ConsumerReviewsController -> SubmitReview() -> Review submitted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting review");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred while submitting review";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
