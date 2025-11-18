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
	/// Public-facing content APIs such as FAQs and blogs for the marketing site.
	/// Route prefix: api/home
	/// Auth: Anonymous access.
	/// </summary>
	[AllowAnonymous]
	[Route("api/[controller]")]
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
		/// Retrieves all FAQ categories for filtering customer support content.
		/// Route: GET api/home/faq/categories
		/// Auth: AllowAnonymous.
		/// </summary>
		/// <returns>Collection of FAQ categories.</returns>
		[HttpGet("faq/categories")]
		public async Task<ActionResult<OperationResult<List<FaqCategoryDto>>>> GetAllFaqCategory()
		{
            var reponse = new OperationResult<List<FaqCategoryDto>>();

            this.logger.LogInformation("Home Controller -> GetAllFaqCategory()");

            var list = await homeService.GetAllFaqCategoryAsync();

            this.logger.LogInformation("Home Controller-> GetAllFaqCategory()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }

		/// <summary>
		/// Retrieves FAQs optionally filtered by category and search term.
		/// Route: GET api/home/faq
		/// Auth: AllowAnonymous.
		/// </summary>
		/// <param name="category">Category identifier or "all" for every category.</param>
		/// <param name="search">Optional keyword filter applied to FAQ content.</param>
		/// <returns>Collection of FAQs matching the criteria.</returns>
		[HttpGet("faq")]
		public async Task<ActionResult<OperationResult<List<FaqDto>>>> GetFaqs([FromQuery] string category, [FromQuery] string search)
		{
            var response = new OperationResult<List<FaqDto>>();
            this.logger.LogInformation("Home Controller -> GetFaqs()");

            List<FaqDto> list;
            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                // Try parse category to int since our existing method expects an integer
                if (int.TryParse(category, out int categoryId))
                {
                    list = await homeService.GetAllFaqByCategoryAsync(categoryId, search);
                }
                else
                {
                    return BadRequest("Invalid category format");
                }
            }
            else
            {
                // Handle the "all" category case or when category is not specified
                // Assuming we need to get FAQs from all categories
                var categories = await homeService.GetAllFaqCategoryAsync();
                list = new List<FaqDto>();
                
                foreach (var cat in categories)
                {
                    var faqs = await homeService.GetAllFaqByCategoryAsync(cat.Id, search);
                    list.AddRange(faqs);
                }
            }

            this.logger.LogInformation("Home Controller-> GetFaqs()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            response.Data = list;
            return Ok(response);
        }

		/// <summary>
		/// Returns paginated and filterable blog posts for the marketing site.
		/// Route: GET api/home/blogs
		/// Auth: AllowAnonymous.
		/// </summary>
		/// <param name="blogs">Pagination and filtering parameters for blogs.</param>
		/// <returns>List of blog posts along with pagination metadata.</returns>
		[HttpGet("blogs")]
		public async Task<ActionResult<OperationResult<List<BlogDto>>>> GetAllBlogs([FromQuery]BlogParams blogs)
		{
            var response = new OperationResult<List<BlogDto>>();

            this.logger.LogInformation("Home Controller -> GetAllBlogs()");

            var list = await homeService.GetAllBlogs(blogs);

            this.logger.LogInformation("Home Controller-> GetAllBlogs()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            response.Data = list;

            return Ok(response);
        }

		/// <summary>
		/// Lists all blog categories that can be used to filter content.
		/// Route: GET api/home/blogs/categories
		/// Auth: AllowAnonymous.
		/// </summary>
		/// <returns>Collection of blog categories.</returns>
		[HttpGet("blogs/categories")]
		public async Task<ActionResult<OperationResult<List<BlogCategoryDto>>>> GetAllBlogCategories()
		{
            var response = new OperationResult<List<BlogCategoryDto>>();

            this.logger.LogInformation("Home Controller -> GetAllBlogCategories()");

            var categories = await homeService.GetAllBlogCategories();

            this.logger.LogInformation("Home Controller-> GetAllBlogCategories()-> return {0}", JsonConvert.SerializeObject(new { Categories = categories }));

            response.Data = categories;

            return Ok(response);
        }

		/// <summary>
		/// Retrieves the details of a specific blog post.
		/// Route: GET api/home/blogs/{blogId}
		/// Auth: AllowAnonymous.
		/// </summary>
		/// <param name="blogId">The numeric identifier of the blog post.</param>
		/// <returns>Blog details including content and metadata.</returns>
		[HttpGet("blogs/{blogId}")]
		public async Task<ActionResult<OperationResult<BlogDto>>> GetBlogDetails(int blogId)
		{
            var response = new OperationResult<BlogDto>();

            this.logger.LogInformation("Home Controller -> GetBlogDetails({0})", blogId);

            var blog = await homeService.GetBlogDetails(blogId);

            if (blog == null)
            {
                response.Message = "Blog not found";
                return NotFound(response);
            }

            this.logger.LogInformation("Home Controller-> GetBlogDetails()-> return {0}", JsonConvert.SerializeObject(new { Blog = blog }));

            response.Data = blog;

            return Ok(response);
        }

        //create an endpoint to get all the categories here we need to take the latitude and longitude and return the categories
        [HttpGet("get-categories")]
        [Authorize]
        public async Task<ActionResult<OperationResult<List<CategoryDto>>>> GetAllCategories([FromQuery] CategoryParams categoryParams)
        {
            var reponse = new OperationResult<List<CategoryDto>>();

            this.logger.LogInformation("Home Controller -> GetAllCategories()");

            var list = await homeService.GetAllCategories(categoryParams);

            this.logger.LogInformation("Home Controller-> GetAllCategories()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }

    

        [HttpGet("get-trending-businesses")]
        public async Task<ActionResult<OperationResult<List<TrendingBusinessDto>>>> GetTrendingBusinesses([FromQuery] TrendingBusinessParams trendingBusinessParams)
        {
            var reponse = new OperationResult<List<TrendingBusinessDto>>();

            this.logger.LogInformation("Home Controller -> GetTrendingBusinesses()");

            var list = await homeService.GetTrendingBusinesses(trendingBusinessParams);

            this.logger.LogInformation("Home Controller-> GetTrendingBusinesses()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }



    }
}
