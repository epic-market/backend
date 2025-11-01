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
    /// <summary>
    /// Product catalog category API. Provides category CRUD operations scoped to the authenticated business.
    /// </summary>
    /// <remarks>
    /// This partial controller shares the <c>api/products</c> route prefix defined in the other partial implementation of <see cref="ProductController"/>.
    /// </remarks>
    public partial class ProductController : BaseApiController
	{
	
		/// <summary>
		/// Gets all categories for the current business.
		/// </summary>
		/// <remarks>
		/// Route: <c>GET api/products/categories</c>
		/// Auth: <c>Authorize</c>. Business context inferred from the current user.
		/// </remarks>
		[HttpGet("categories")]
		public async Task<IActionResult> GetCategories()
		{

			var categories = await catalogCategoryService.GetCategories(this.BusinessId);
			return Ok(categories);
		}	

		/// <summary>
		/// Gets a single category by its identifier.
		/// </summary>
		/// <remarks>
		/// Route: <c>GET api/products/category/{{id}}</c>
		/// Auth: <c>Authorize</c>.
		/// </remarks>
		[HttpGet("category/{id}")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var category = await catalogCategoryService.GetCategory(id);
			return Ok(category);
		}
		
		/// <summary>
		/// Creates a new category for the current business.
		/// </summary>
		/// <remarks>
		/// Route: <c>POST api/products/category</c>
		/// Auth: <c>Authorize</c>.
		/// Body: JSON <see cref="CategoriesDto"/>.
		/// </remarks>
		[HttpPost("category")]
		public async Task<IActionResult> CreateCategory([FromBody]CategoriesDto categoryDto)
		{

		 	 categoryDto.BusinessID = this.BusinessId;
			var category = await catalogCategoryService.CreateCategory(categoryDto);
			return Ok(category);
		}	

		/// <summary>
		/// Updates an existing category.
		/// </summary>
		/// <remarks>
		/// Route: <c>PUT api/products/category/{{id}}</c>
		/// Auth: <c>Authorize</c>.
		/// Body: JSON <see cref="UpdateCategoryDto"/>.
		/// </remarks>
		[HttpPut("category/{id}")]
		public async Task<IActionResult> UpdateCategory(int id,UpdateCategoryDto categoryDto)
		{
			var category = await catalogCategoryService.UpdateCategory(id,categoryDto);
			return Ok(category);
		}			

		/// <summary>
		/// Deletes a category from the catalog.
		/// </summary>
		/// <remarks>
		/// Route: <c>DELETE api/products/category/{{id}}</c>
		/// Auth: <c>Authorize</c>.
		/// </remarks>
		[HttpDelete("category/{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await catalogCategoryService.DeleteCategory(id);
			return Ok(category);
		}

    }
}
