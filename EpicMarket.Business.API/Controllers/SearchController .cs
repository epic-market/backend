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
