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

    [AllowAnonymous]
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

        [HttpGet("GetAllFaqCategory/{typeOfCategory}")]
        public async Task<ActionResult<OperationResult<List<FaqCategoryDto>>>> GetAllFaqCategory(string typeOfCategory)
        {
            var reponse = new OperationResult<List<FaqCategoryDto>>();

            this.logger.LogInformation("Home Controller -> GetAllFaqCategory()");

            var list = await homeService.GetAllFaqCategoryAsync(typeOfCategory);

            this.logger.LogInformation("Home Controller-> GetAllFaqCategory()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }


        [HttpGet("GetAllFAQ/{CateoryID}")]
        public async Task<ActionResult<OperationResult<List<FaqDto>>>> GetAllFaqByCategoryID(int CateoryID)
        {
            var reponse = new OperationResult<List<FaqDto>>();

            this.logger.LogInformation("Home Controller -> GetAllFaqByCategoryID()");

            var list = await homeService.GetAllFaqByCategoryAsync(CateoryID);

            this.logger.LogInformation("Home Controller-> GetAllFaqByCategoryID()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }


        [HttpGet("GetAllBlogs")]
        public async Task<ActionResult<OperationResult<List<BlogDto>>>> GetAllBlogs([FromQuery]BlogParams blogs)
        {
            var reponse = new OperationResult<List<BlogDto>>();

            this.logger.LogInformation("Home Controller -> GetAllBlogs()");

            var list = await homeService.GetAllBlogs(blogs);

            this.logger.LogInformation("Home Controller-> GetAllBlogs()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }

        [HttpGet("GetAllBlogsByCategory")]
        public async Task<ActionResult<OperationResult<List<BlogDto>>>> GetAllBlogsByCategory([FromQuery] BlogsByCategoryParams blogs)
        {
            var reponse = new OperationResult<List<BlogDto>>();

            this.logger.LogInformation("Home Controller -> GetAllBlogsByCategory()");

            var list = await homeService.GetAllBlogsByCategory(blogs);

            this.logger.LogInformation("Home Controller-> GetAllBlogsByCategory()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = list }));

            reponse.Data = list;

            return Ok(list);
        }

        [HttpGet("GetBlogDetails/{blogId}")]
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
