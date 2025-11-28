using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomerAPI;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EpicMarket.Business.API.Controllers.CustomerAPI
{
    /// <summary>
    /// Consumer-facing Business API.
    /// Provides endpoints for categories, businesses, outlets, listings, and search.
    /// </summary>
    [Route("api/business")]
    [ApiController]
    public class ConsumerBusinessController : ControllerBase
    {
        private readonly ILogger<ConsumerBusinessController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IBusinessService _businessService;
        private readonly ISearchService _searchService;
        private readonly IAttachmentService _attachmentService;

        public ConsumerBusinessController(
            ILogger<ConsumerBusinessController> logger,
            ApplicationDbContext dbContext,
            IBusinessService businessService,
            ISearchService searchService,
            IAttachmentService attachmentService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _businessService = businessService;
            _searchService = searchService;
            _attachmentService = attachmentService;
        }

        /// <summary>
        /// Get all business categories.
        /// </summary>
        /// <remarks>
        /// Route: GET api/business/categories
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <returns>List of business categories</returns>
        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<CategoryResponse>>>> GetCategories()
        {
            var response = new OperationResult<List<CategoryResponse>>();

            try
            {
                _logger.LogInformation("ConsumerBusinessController -> GetCategories()");

                var categories = await _dbContext.BusinessCategories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ID)
                    .Select(c => new CategoryResponse
                    {
                        Id = c.ID,
                        Name = c.Name,
                        Description = c.Description ?? $"Discover {c.Name}",
                        Image = null, // Image not available in current model
                        Icon = GetCategoryIcon(c.Type ?? c.Name),
                        DisplayOrder = c.ID,
                        BusinessCount = _dbContext.Businesses.Count(b => b.BusinessCategoryID == c.ID && b.IsActive),
                        IsActive = c.IsActive
                    })
                    .ToListAsync();

                response.Message = "Success";
                response.Data = categories;

                _logger.LogInformation("ConsumerBusinessController -> GetCategories() -> Found {count} categories", categories.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred while retrieving categories";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get a specific business category by ID.
        /// </summary>
        /// <remarks>
        /// Route: GET api/business/categories/{id}
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet("categories/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<CategoryResponse>>> GetCategoryById(int id)
        {
            var response = new OperationResult<CategoryResponse>();

            try
            {
                _logger.LogInformation("ConsumerBusinessController -> GetCategoryById({id})", id);

                var category = await _dbContext.BusinessCategories
                    .Where(c => c.ID == id && c.IsActive)
                    .Select(c => new CategoryResponse
                    {
                        Id = c.ID,
                        Name = c.Name,
                        Description = c.Description ?? $"Discover {c.Name}",
                        Image = null,
                        Icon = GetCategoryIcon(c.Type ?? c.Name),
                        DisplayOrder = c.ID,
                        BusinessCount = _dbContext.Businesses.Count(b => b.BusinessCategoryID == c.ID && b.IsActive),
                        IsActive = c.IsActive
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Category not found";
                    response.ErrorDetail = $"No category found with ID: {id}";
                    return NotFound(response);
                }

                response.Message = "Success";
                response.Data = category;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID: {id}", id);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get all business outlets with optional filtering.
        /// </summary>
        /// <remarks>
        /// Route: GET api/business/businesses
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="search">Search term for name/description</param>
        /// <param name="category">Filter by category name</param>
        /// <param name="sortBy">Sort field: rating, reviewCount, name, newest</param>
        /// <param name="verifiedOnly">Filter for verified businesses only</param>
        /// <param name="newOnly">Filter for new businesses only</param>
        /// <param name="onlineBooking">Filter for businesses with online booking</param>
        /// <param name="instantBooking">Filter for businesses with instant booking</param>
        /// <returns>List of businesses</returns>
        [HttpGet("businesses")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<BusinessResponse>>>> GetBusinesses(
            [FromQuery] string search = null,
            [FromQuery] string category = null,
            [FromQuery] string sortBy = "rating",
            [FromQuery] bool verifiedOnly = false,
            [FromQuery] bool newOnly = false,
            [FromQuery] bool onlineBooking = false,
            [FromQuery] bool instantBooking = false)
        {
            var response = new OperationResult<List<BusinessResponse>>();

            try
            {
                _logger.LogInformation("ConsumerBusinessController -> GetBusinesses() -> search: {search}, category: {category}", search, category);

                var query = _dbContext.Outlets
                    .Include(o => o.Bussiness)
                        .ThenInclude(b => b.BusinessCategory)
                    .Include(o => o.Address)
                    .Where(o => o.IsActive && o.Bussiness.IsActive);

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(o =>
                        o.Name.Contains(search) ||
                        o.Description.Contains(search) ||
                        o.Bussiness.Name.Contains(search));
                }

                // Apply category filter
                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(o => o.Bussiness.BusinessCategory.CategoryName == category);
                }

                // Apply new only filter (created in last 30 days)
                if (newOnly)
                {
                    var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                    query = query.Where(o => o.CreateDate >= thirtyDaysAgo);
                }

                // Apply sorting
                query = sortBy?.ToLower() switch
                {
                    "name" => query.OrderBy(o => o.Name),
                    "newest" => query.OrderByDescending(o => o.CreateDate),
                    "reviewcount" => query.OrderByDescending(o => o.Ratings.Count),
                    _ => query.OrderByDescending(o => o.Ratings.Any() ? o.Ratings.Average(r => r.RatingValue) : 0)
                };

                var outlets = await query.ToListAsync();

                var businesses = outlets.Select(o => MapToBusinessResponse(o)).ToList();

                response.Message = "Success";
                response.Data = businesses;

                _logger.LogInformation("ConsumerBusinessController -> GetBusinesses() -> Found {count} businesses", businesses.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting businesses");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get detailed information about a specific outlet.
        /// </summary>
        /// <remarks>
        /// Route: GET api/business/outlets/{outletId}
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="outletId">Outlet ID</param>
        /// <returns>Outlet details</returns>
        [HttpGet("outlets/{outletId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<OutletDetailResponse>>> GetOutletDetails(int outletId)
        {
            var response = new OperationResult<OutletDetailResponse>();

            try
            {
                _logger.LogInformation("ConsumerBusinessController -> GetOutletDetails({outletId})", outletId);

                var outlet = await _dbContext.Outlets
                    .Include(o => o.Bussiness)
                        .ThenInclude(b => b.BusinessCategory)
                    .Include(o => o.Address)
                    .Include(o => o.Ratings)
                    .FirstOrDefaultAsync(o => o.ID == outletId && o.IsActive);

                if (outlet == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Outlet not found";
                    response.ErrorDetail = $"No outlet found with ID: {outletId}";
                    return NotFound(response);
                }

                // Get outlet photos from attachments
                var photos = await GetOutletPhotos(outletId);

                var outletDetail = new OutletDetailResponse
                {
                    OutletId = outlet.ID,
                    Name = outlet.Name,
                    Description = outlet.Description,
                    Address = outlet.Address?.Address1 ?? "",
                    City = outlet.Address?.City ?? "",
                    State = outlet.Address?.State ?? "",
                    Pincode = outlet.Address?.Pincode ?? 0,
                    Latitude = outlet.Address?.Latitude ?? 0,
                    Longitude = outlet.Address?.Longitude ?? 0,
                    ContactEmail = outlet.ContactEmail,
                    Thumbnail = null, // Can be populated from attachments
                    Photos = photos,
                    IsOpen = outlet.IsOpen,
                    TimingList = ParseTimings(outlet.TimingList),
                    SocialMediaLinkFacebook = outlet.SocialMediaLinkFacebook,
                    SocialMediaLinkInstagram = outlet.SocialMediaLinkInstagram,
                    SocialMediaLinkTwitter = outlet.SocialMediaLinkTwitter,
                    SocialMediaLinkYoutube = outlet.SocialMediaLinkYoutube,
                    SpecialNoteOfTheDay = outlet.SpecialNoteOfTheDay,
                    Rating = outlet.Rating ?? (outlet.Ratings.Any() ? Math.Round(outlet.Ratings.Average(r => r.Stars), 1) : 0),
                    ReviewCount = outlet.ReviewCount ?? outlet.Ratings.Count,
                    BusinessCategory = outlet.Bussiness?.BusinessCategory?.Name ?? "General"
                };

                response.Message = "Success";
                response.Data = outletDetail;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting outlet details for ID: {outletId}", outletId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get business listings grouped by categories.
        /// </summary>
        /// <remarks>
        /// Route: GET api/business/listings
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="category">Optional category filter</param>
        /// <returns>Grouped business listings</returns>
        [HttpGet("listings")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<BusinessListingsResponse>>> GetBusinessListings([FromQuery] string category = null)
        {
            var response = new OperationResult<BusinessListingsResponse>();

            try
            {
                _logger.LogInformation("ConsumerBusinessController -> GetBusinessListings() -> category: {category}", category);

                var categories = await _dbContext.BusinessCategories
                    .Where(c => c.IsActive)
                    .Where(c => string.IsNullOrWhiteSpace(category) || c.Name == category)
                    .OrderBy(c => c.ID)
                    .ToListAsync();

                var businessGroups = new List<BusinessGroupItem>();

                foreach (var cat in categories)
                {
                    var outlets = await _dbContext.Outlets
                        .Include(o => o.Bussiness)
                            .ThenInclude(b => b.BusinessCategory)
                        .Include(o => o.Address)
                        .Include(o => o.Ratings)
                        .Where(o => o.IsActive && o.Bussiness.IsActive && o.Bussiness.BusinessCategoryID == cat.ID)
                        .OrderByDescending(o => o.Rating ?? 0)
                        .Take(10)
                        .ToListAsync();

                    var groupTitle = cat.Name switch
                    {
                        "Restaurants" => "Trending Restaurants",
                        "Groceries" => "Grocery Essentials",
                        "Pharmacy" => "Health & Pharmacy",
                        _ => $"Popular {cat.Name}"
                    };

                    businessGroups.Add(new BusinessGroupItem
                    {
                        Title = groupTitle,
                        Description = cat.Description ?? $"Discover {cat.Name}",
                        Category = cat.Name,
                        Businesses = outlets.Select(o => MapToBusinessResponse(o)).ToList()
                    });
                }

                response.Message = "Success";
                response.Data = new BusinessListingsResponse
                {
                    BusinessGroups = businessGroups
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting business listings");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Search for businesses and products.
        /// </summary>
        /// <remarks>
        /// Route: GET api/business/search
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="searchTerm">Search query</param>
        /// <param name="latitude">User's latitude for distance calculation</param>
        /// <param name="longitude">User's longitude for distance calculation</param>
        /// <param name="radiusKm">Search radius in kilometers</param>
        /// <returns>Search results</returns>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<SearchResponse>>> Search(
            [FromQuery] string searchTerm,
            [FromQuery] double? latitude = null,
            [FromQuery] double? longitude = null,
            [FromQuery] double radiusKm = 10)
        {
            var response = new OperationResult<SearchResponse>();

            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Search term is required";
                    return BadRequest(response);
                }

                _logger.LogInformation("ConsumerBusinessController -> Search() -> searchTerm: {searchTerm}", searchTerm);

                // Search outlets
                var outletsQuery = _dbContext.Outlets
                    .Include(o => o.Address)
                    .Where(o => o.IsActive &&
                        (o.Name.Contains(searchTerm) ||
                         o.Description.Contains(searchTerm)));

                var outlets = await outletsQuery.Take(20).ToListAsync();

                var branchResults = outlets.Select(o => new SearchBranchResult
                {
                    Id = o.ID,
                    Name = o.Name,
                    Description = o.Description,
                    Thumbnail = o.Thumbnail,
                    Latitude = o.Address?.Latitude ?? 0,
                    Longitude = o.Address?.Longitude ?? 0,
                    Distance = latitude.HasValue && longitude.HasValue && o.Address != null
                        ? CalculateDistance(latitude.Value, longitude.Value, o.Address.Latitude, o.Address.Longitude)
                        : null
                }).ToList();

                // Search products
                var productsQuery = _dbContext.Products
                    .Include(p => p.ProductVariants)
                    .Include(p => p.Outlet)
                    .Where(p => p.IsActive &&
                        (p.Name.Contains(searchTerm) ||
                         p.Description.Contains(searchTerm)));

                var products = await productsQuery.Take(20).ToListAsync();

                var productResults = products.Select(p => new SearchProductResult
                {
                    Id = p.ID,
                    Name = p.Name,
                    Price = p.ProductVariants.FirstOrDefault()?.SalePrice ?? 0,
                    Thumbnail = p.Thumbnail,
                    BranchName = p.Outlet?.Name ?? "",
                    BranchId = p.OutletID ?? 0,
                    Distance = null // Can be calculated if product has outlet location
                }).ToList();

                response.Message = "Success";
                response.Data = new SearchResponse
                {
                    Branches = branchResults,
                    Products = productResults
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for: {searchTerm}", searchTerm);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        #region Private Helper Methods

        private static string GetCategoryIcon(string categoryName)
        {
            return categoryName?.ToLower() switch
            {
                "restaurants" or "restaurant" or "food" => "🍽️",
                "groceries" or "grocery" => "🛒",
                "pharmacy" or "medicine" or "health" => "💊",
                "electronics" => "📱",
                "fashion" or "clothing" => "👕",
                "beauty" or "cosmetics" => "💄",
                "sports" => "⚽",
                "books" => "📚",
                "toys" => "🎮",
                "home" or "furniture" => "🏠",
                _ => "📦"
            };
        }

        private BusinessResponse MapToBusinessResponse(Outlet outlet)
        {
            var rating = outlet.Rating ?? (outlet.Ratings?.Any() == true
                ? Math.Round(outlet.Ratings.Average(r => r.Stars), 1)
                : 0);

            return new BusinessResponse
            {
                Id = outlet.ID,
                OutletId = outlet.ID,
                BusinessId = outlet.BussinessID,
                Name = outlet.Name,
                Description = outlet.Description,
                AddressLine1 = outlet.Address?.Address1 ?? "",
                AddressLine2 = outlet.Address?.Address2 ?? "",
                Address = $"{outlet.Address?.Address1 ?? ""}, {outlet.Address?.City ?? ""}",
                City = outlet.Address?.City ?? "",
                State = outlet.Address?.State ?? "",
                Pincode = outlet.Address?.Pincode ?? 0,
                Country = "India",
                Latitude = outlet.Address?.Latitude ?? 0,
                Longitude = outlet.Address?.Longitude ?? 0,
                ContactNumber = outlet.ContactNumber.ToString(),
                ContactEmail = outlet.ContactEmail,
                Thumbnail = null, // Can be populated from attachments if needed
                Image = null,
                Photos = new List<string>(),
                IsOpen = outlet.IsOpen,
                TimingList = ParseTimings(outlet.TimingList),
                SocialMediaLinkFacebook = outlet.SocialMediaLinkFacebook,
                SocialMediaLinkInstagram = outlet.SocialMediaLinkInstagram,
                SpecialNoteOfTheDay = outlet.SpecialNoteOfTheDay,
                Rating = rating,
                ReviewCount = outlet.ReviewCount ?? outlet.Ratings?.Count ?? 0,
                WaitTime = "15-25 min",
                PriceRange = "₹₹",
                Features = new List<string> { "Dine-In", "Takeaway" },
                Type = rating >= 4.5 ? "Featured" : "Standard",
                Badge = rating >= 4.5 ? "Top Rated" : null,
                Category = outlet.Bussiness?.BusinessCategory?.Name ?? "General",
                CategoryId = outlet.Bussiness?.BusinessCategoryID ?? 0,
                IsActive = outlet.IsActive
            };
        }

        private List<TimingItem> ParseTimings(string timingListJson)
        {
            if (string.IsNullOrWhiteSpace(timingListJson))
            {
                return new List<TimingItem>
                {
                    new TimingItem { Day = "Monday - Sunday", OpenTime = "9:00 AM", CloseTime = "9:00 PM" }
                };
            }

            try
            {
                // Try to parse as JSON array
                var timings = System.Text.Json.JsonSerializer.Deserialize<List<TimingItem>>(timingListJson);
                return timings ?? new List<TimingItem>
                {
                    new TimingItem { Day = "Monday - Sunday", OpenTime = "9:00 AM", CloseTime = "9:00 PM" }
                };
            }
            catch
            {
                return new List<TimingItem>
                {
                    new TimingItem { Day = "Monday - Sunday", OpenTime = "9:00 AM", CloseTime = "9:00 PM" }
                };
            }
        }

        private async Task<List<string>> GetOutletPhotos(int outletId)
        {
            try
            {
                var attachmentLinks = await _dbContext.AttachmentLinks
                    .Include(al => al.Attachments)
                    .Where(al => al.Entity == "Branch" && al.RecordID == outletId)
                    .Select(al => al.Attachments.FileKey)
                    .ToListAsync();

                return attachmentLinks ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return Math.Round(R * c, 1);
        }

        private double ToRadians(double degrees) => degrees * Math.PI / 180;

        #endregion
    }
}
