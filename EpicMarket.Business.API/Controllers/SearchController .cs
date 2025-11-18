using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using EpicMarket.Services;


namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Provides search and discovery endpoints for outlets and businesses.
    /// Includes keyword search and trending store lookups.
    /// </summary>
    [Route("api/search")]
    public class SearchController : BaseApiController
    {
        private readonly ILogger<SearchController> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly IBusinessService businessService;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileStoreService;
        private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly ISearchService searchService;


        public SearchController(
                                    ILogger<SearchController> logger,
                                    UserManager<AppUser> _userManager,
                                    IBusinessService businessService,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IAttachmentService attachmentService,
                                    IFileService fileStoreService,
                                    ISearchService searchService,

                                    IApplicationConfigurationService applicationConfigurationService
                                  ) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            userManager = _userManager;
            this.businessService = businessService;
            this.attachmentService = attachmentService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
            this.searchService = searchService;
        }

        /// <summary>
        /// Performs a keyword search over businesses and outlets with optional location filtering.
        /// Route: GET api/search
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="searchTerm">Keyword to search for in business and outlet records.</param>
        /// <param name="latitude">Optional latitude to constrain search results.</param>
        /// <param name="longitude">Optional longitude to constrain search results.</param>
        /// <param name="radiusKm">Radius in kilometers for location-based filtering.</param>
        /// <returns>Structured search results containing outlets, products, and metadata.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<SearchResult>> Search(
        [FromQuery] string searchTerm,
        [FromQuery] double? latitude = null,
        [FromQuery] double? longitude = null,
        [FromQuery] double radiusKm = 10)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("Search term cannot be empty");

            var request = new SearchRequest
            {
                SearchTerm = searchTerm,
                Latitude = latitude,
                Longitude = longitude,
                RadiusKm = radiusKm
            };

            var result = await this.searchService.SearchAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the top three trending stores near a given location.
        /// Route: GET api/search/top3/nearby
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="latitude">Latitude of the search origin.</param>
        /// <param name="longitude">Longitude of the search origin.</param>
        /// <param name="radiusKm">Radius in kilometers to evaluate trending stores.</param>
        /// <returns>Collection of trending outlet summaries.</returns>
        [HttpGet("top3/nearby")]
        public async Task<ActionResult<List<TrendingOutletDto>>> GetTop3TrendingStoresNearby(
                    [FromQuery] double latitude,
                    [FromQuery] double longitude,
                    [FromQuery] double radiusKm = 10)
        {
            var trendingStores = await searchService.GetTop3TrendingStoresNearbyAsync(latitude, longitude, radiusKm);
            return Ok(trendingStores);
        }

    }
}
