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

    public partial class CatalogController : BaseApiController
	{
	
		[HttpGet("categories")]
		public async Task<IActionResult> GetCategories()
		{

			var categories = await catalogCategoryService.GetCategories(this.BusinessId);
			return Ok(categories);
		}	

		[HttpGet("category/{id}")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var category = await catalogCategoryService.GetCategory(id);
			return Ok(category);
		}
		
		[HttpPost("category")]
		public async Task<IActionResult> CreateCategory([FromBody]CategoriesDto categoryDto)
		{

		 	 categoryDto.BusinessID = this.BusinessId;
			var category = await catalogCategoryService.CreateCategory(categoryDto);
			return Ok(category);
		}	

		[HttpPut("category/{id}")]
		public async Task<IActionResult> UpdateCategory(int id,UpdateCategoryDto categoryDto)
		{
			var category = await catalogCategoryService.UpdateCategory(id,categoryDto);
			return Ok(category);
		}			

		[HttpDelete("category/{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await catalogCategoryService.DeleteCategory(id);
			return Ok(category);
		}

    }
}
