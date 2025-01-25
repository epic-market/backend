using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace EpicMarket.Business.API.Controllers
{

    [Route("api/category")]
    public class CategoryController : BaseApiController
	{
		private readonly ILogger<CategoryController> logger;
		private readonly ICategoryService categoryService;

		public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, IApplicationConfigurationService applicationConfigurationService,IRatingService ratingService,
			IAttachmentService attachmentService, IFileService fileStoreService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
			this.logger = logger;
			this.categoryService= categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{

			var categories = await categoryService.GetCategories(this.BusinessId);
			return Ok(categories);
		}	

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var category = await categoryService.GetCategory(id);
			return Ok(category);
		}
		
		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody]CategoriesDto categoryDto)
		{

		 	 categoryDto.BusinessID = this.BusinessId;
			var category = await categoryService.CreateCategory(categoryDto);
			return Ok(category);
		}	

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory(int id,UpdateCategoryDto categoryDto)
		{
			var category = await categoryService.UpdateCategory(id,categoryDto);
			return Ok(category);
		}			

		[HttpDelete]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await categoryService.DeleteCategory(id);
			return Ok(category);
		}

    }
}
