using EpicMarket.Contracts;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EpicMarket.Data.Models;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Provides public content including FAQs, blogs, and general information
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HomeController : BaseApiController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IBusinessService businessService;
        private readonly IHomeService homeService;

        public HomeController(ILogger<HomeController> logger, IBusinessService businessService, IHomeService homeService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
            this.logger = logger;
            this.businessService = businessService;
            this.homeService = homeService;
        }

        /// <summary>
        /// Retrieves all FAQ categories by type
        /// </summary>
        /// <param name="typeOfCategory">The type of FAQ category (e.g., "business", "customer", "general")</param>
        /// <returns>List of FAQ categories</returns>
        /// <response code="200">Returns list of FAQ categories</response>
        /// <remarks>
        /// Available category types:
        /// - "business": Business-related FAQs
        /// - "customer": Customer support FAQs
        /// - "general": General information FAQs
        /// </remarks>
        [HttpGet("GetAllFaqCategory/{typeOfCategory}")]
        [ProducesResponseType(typeof(OperationResult<List<FaqCategoryDto>>), 200)]
        public async Task<ActionResult<OperationResult<List<FaqCategoryDto>>>> GetAllFaqCategory(string typeOfCategory)
        {
            var reponse = new OperationResult<List<FaqCategoryDto>>();

            this.logger.LogInformation("Home Controller -> GetAllFaqCategory()");

            var list = await homeService.GetAllFaqCategoryAsync(typeOfCategory);

            this.logger.LogInformation("Home Controller-> GetAllFaqCategory()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }


        /// <summary>
        /// Retrieves all FAQs for a specific category
        /// </summary>
        /// <param name="CateoryID">The category ID to filter FAQs</param>
        /// <returns>List of FAQs in the specified category</returns>
        /// <response code="200">Returns list of FAQs</response>
        /// <response code="404">Category not found</response>
        /// <remarks>
        /// Returns FAQs with:
        /// - Question text
        /// - Answer content
        /// - Display order
        /// - Last updated date
        /// </remarks>
        [HttpGet("GetAllFAQ/{CateoryID}")]
        [ProducesResponseType(typeof(OperationResult<List<FaqDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<List<FaqDto>>>> GetAllFaqByCategoryID(int CateoryID)
        {
            var reponse = new OperationResult<List<FaqDto>>();

            this.logger.LogInformation("Home Controller -> GetAllFaqByCategoryID()");

            var list = await homeService.GetAllFaqByCategoryAsync(CateoryID);

            this.logger.LogInformation("Home Controller-> GetAllFaqByCategoryID()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }


        /// <summary>
        /// Retrieves all blog posts with pagination and filtering
        /// </summary>
        /// <param name="blogs">Filter parameters including pagination, search, and date range</param>
        /// <returns>Paginated list of blog posts</returns>
        /// <response code="200">Returns list of blog posts</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/home/GetAllBlogs?pageNumber=1&amp;pageSize=10&amp;searchTerm=market&amp;featured=true
        /// 
        /// Returns blog posts with:
        /// - Title and summary
        /// - Author information
        /// - Publication date
        /// - Featured image
        /// - Read time estimate
        /// - Tags and categories
        /// </remarks>
        [HttpGet("GetAllBlogs")]
        [ProducesResponseType(typeof(OperationResult<List<BlogDto>>), 200)]
        public async Task<ActionResult<OperationResult<List<BlogDto>>>> GetAllBlogs([FromQuery]BlogParams blogs)
        {
            var reponse = new OperationResult<List<BlogDto>>();

            this.logger.LogInformation("Home Controller -> GetAllBlogs()");

            var list = await homeService.GetAllBlogs(blogs);

            this.logger.LogInformation("Home Controller-> GetAllBlogs()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }

        /// <summary>
        /// Retrieves blog posts filtered by category
        /// </summary>
        /// <param name="blogs">Category filter parameters with pagination</param>
        /// <returns>List of blog posts in the specified category</returns>
        /// <response code="200">Returns filtered blog posts</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/home/GetAllBlogsByCategory?categoryId=5&amp;pageNumber=1&amp;pageSize=10
        /// </remarks>
        [HttpGet("GetAllBlogsByCategory")]
        [ProducesResponseType(typeof(OperationResult<List<BlogDto>>), 200)]
        public async Task<ActionResult<OperationResult<List<BlogDto>>>> GetAllBlogsByCategory([FromQuery] BlogsByCategoryParams blogs)
        {
            var reponse = new OperationResult<List<BlogDto>>();

            this.logger.LogInformation("Home Controller -> GetAllBlogsByCategory()");

            var list = await homeService.GetAllBlogsByCategory(blogs);

            this.logger.LogInformation("Home Controller-> GetAllBlogsByCategory()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }

        /// <summary>
        /// Retrieves detailed information for a specific blog post
        /// </summary>
        /// <param name="blogId">The unique identifier of the blog post</param>
        /// <returns>Complete blog post details</returns>
        /// <response code="200">Returns blog post details</response>
        /// <response code="404">Blog post not found</response>
        /// <remarks>
        /// Returns complete blog content including:
        /// - Full article text
        /// - All images and media
        /// - Author biography
        /// - Related posts
        /// - Comments (if enabled)
        /// - Social sharing statistics
        /// </remarks>
        [HttpGet("GetBlogDetails/{blogId}")]
        [ProducesResponseType(typeof(OperationResult<BlogDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<BlogDto>>> GetBlogDetails(int blogId)
        {
            var reponse = new OperationResult<BlogDto>();

            this.logger.LogInformation("Home Controller -> GetAllBlogs()");

            var list = await homeService.GetBlogDetails(blogId);

            this.logger.LogInformation("Home Controller-> GetAllBlogs()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }
    }
}
