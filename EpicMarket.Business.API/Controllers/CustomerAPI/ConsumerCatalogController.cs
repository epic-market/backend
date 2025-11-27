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
    /// Consumer-facing Catalog API.
    /// Provides endpoints for product categories and products.
    /// </summary>
    [Route("api/catalog")]
    [ApiController]
    public class ConsumerCatalogController : ControllerBase
    {
        private readonly ILogger<ConsumerCatalogController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductService _productService;

        public ConsumerCatalogController(
            ILogger<ConsumerCatalogController> logger,
            ApplicationDbContext dbContext,
            IProductService productService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _productService = productService;
        }

        /// <summary>
        /// Get product categories for a specific outlet.
        /// </summary>
        /// <remarks>
        /// Route: GET api/catalog/categories/{outletId}
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="outletId">Outlet ID</param>
        /// <returns>List of product categories</returns>
        [HttpGet("categories/{outletId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<ProductCategoryResponse>>>> GetProductCategories(int outletId)
        {
            var response = new OperationResult<List<ProductCategoryResponse>>();

            try
            {
                _logger.LogInformation("ConsumerCatalogController -> GetProductCategories({outletId})", outletId);

                // Get the business ID for this outlet
                var outlet = await _dbContext.Outlets.FindAsync(outletId);
                if (outlet == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Outlet not found";
                    return NotFound(response);
                }

                // Get categories for this business
                var categories = await _dbContext.ProductCategories
                    .Where(c => c.IsActive && c.BusinessID == outlet.BussinessID)
                    .OrderBy(c => c.ID)
                    .Select(c => new ProductCategoryResponse
                    {
                        Id = c.ID,
                        Name = c.Name,
                        Description = c.Description,
                        OutletId = outletId,
                        DisplayOrder = c.ID,
                        IsActive = c.IsActive
                    })
                    .ToListAsync();

                response.Message = "Success";
                response.Data = categories;

                _logger.LogInformation("ConsumerCatalogController -> GetProductCategories() -> Found {count} categories", categories.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product categories for outlet: {outletId}", outletId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get products for a specific outlet with filtering and pagination.
        /// </summary>
        /// <remarks>
        /// Route: GET api/catalog/products
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="outletId">Outlet ID (required)</param>
        /// <param name="categoryId">Filter by category ID</param>
        /// <param name="searchTerm">Search in name/description/tags</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <param name="sortBy">Sort field: name, price, rating</param>
        /// <param name="sortOrder">Sort direction: asc, desc</param>
        /// <returns>Paginated list of products</returns>
        [HttpGet("products")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<ProductListResponse>>> GetProducts(
            [FromQuery] int outletId,
            [FromQuery] int? categoryId = null,
            [FromQuery] string searchTerm = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "name",
            [FromQuery] string sortOrder = "asc")
        {
            var response = new OperationResult<ProductListResponse>();

            try
            {
                if (outletId <= 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Valid outlet ID is required";
                    return BadRequest(response);
                }

                _logger.LogInformation("ConsumerCatalogController -> GetProducts() -> outletId: {outletId}, categoryId: {categoryId}", outletId, categoryId);

                // Get the business ID for this outlet
                var outlet = await _dbContext.Outlets.FindAsync(outletId);
                if (outlet == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Outlet not found";
                    return NotFound(response);
                }

                var query = _dbContext.Products
                    .Include(p => p.ProductVariants)
                    .Include(p => p.Category)
                    .Include(p => p.Ratings)
                    .Where(p => p.IsActive && p.BusinessID == outlet.BussinessID);

                // Apply category filter
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query = query.Where(p => p.CategoryID == categoryId.Value);
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p =>
                        p.Name.Contains(searchTerm) ||
                        p.Description.Contains(searchTerm));
                }

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply sorting
                query = (sortBy?.ToLower(), sortOrder?.ToLower()) switch
                {
                    ("price", "desc") => query.OrderByDescending(p => p.ProductVariants.FirstOrDefault().SalePrice),
                    ("price", _) => query.OrderBy(p => p.ProductVariants.FirstOrDefault().SalePrice),
                    ("rating", "desc") => query.OrderByDescending(p => p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue) : 0),
                    ("rating", _) => query.OrderBy(p => p.Ratings.Any() ? p.Ratings.Average(r => r.RatingValue) : 0),
                    ("name", "desc") => query.OrderByDescending(p => p.Name),
                    _ => query.OrderBy(p => p.Name)
                };

                // Apply pagination
                var products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var productResponses = products.Select(p => MapToProductResponse(p)).ToList();

                response.Message = "Success";
                response.Data = new ProductListResponse
                {
                    Items = productResponses,
                    Count = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                _logger.LogInformation("ConsumerCatalogController -> GetProducts() -> Found {count} products", productResponses.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products for outlet: {outletId}", outletId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get detailed information about a specific product.
        /// </summary>
        /// <remarks>
        /// Route: GET api/catalog/products/{productId}
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="productId">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("products/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<ProductResponse>>> GetProductById(int productId)
        {
            var response = new OperationResult<ProductResponse>();

            try
            {
                _logger.LogInformation("ConsumerCatalogController -> GetProductById({productId})", productId);

                var product = await _dbContext.Products
                    .Include(p => p.ProductVariants)
                    .Include(p => p.Category)
                    .Include(p => p.Ratings)
                    .FirstOrDefaultAsync(p => p.ID == productId && p.IsActive);

                if (product == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Product not found";
                    response.ErrorDetail = $"No product found with ID: {productId}";
                    return NotFound(response);
                }

                response.Message = "Success";
                response.Data = MapToProductResponse(product);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by ID: {productId}", productId);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        #region Private Helper Methods

        private ProductResponse MapToProductResponse(Catalog product)
        {
            var rating = product.Rating ?? (product.Ratings?.Any() == true
                ? Math.Round(product.Ratings.Average(r => r.Stars), 1)
                : 0);

            // Parse variant options from the VariantOptions field
            var variantOptions = new List<VariantOption>();
            var variants = product.ProductVariants?.Where(v => v.IsActive).ToList() ?? new List<CatalogVariants>();
            
            // Group variants by attribute name
            var attributeGroups = new Dictionary<string, HashSet<string>>();
            foreach (var variant in variants)
            {
                if (!string.IsNullOrWhiteSpace(variant.Attributes))
                {
                    if (!attributeGroups.ContainsKey("Option"))
                    {
                        attributeGroups["Option"] = new HashSet<string>();
                    }
                    attributeGroups["Option"].Add(variant.Attributes);
                }
            }

            foreach (var group in attributeGroups)
            {
                variantOptions.Add(new VariantOption
                {
                    Name = group.Key,
                    Values = group.Value.ToList()
                });
            }

            // Parse base highlights
            var baseHighlights = new List<ProductHighlight>();
            if (!string.IsNullOrWhiteSpace(product.BaseHightlights))
            {
                // Assuming base highlights are stored as "key:value,key:value"
                var highlights = product.BaseHightlights.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var highlight in highlights)
                {
                    var parts = highlight.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        baseHighlights.Add(new ProductHighlight
                        {
                            Key = parts[0].Trim(),
                            Value = parts[1].Trim()
                        });
                    }
                }
            }

            // Thumbnail would need to be fetched from attachments if needed
            string thumbnail = null;

            return new ProductResponse
            {
                Id = product.ID,
                ProductId = product.ID,
                Name = product.Name,
                Description = product.Description,
                OutletId = 0, // Not directly available - products belong to Business
                CategoryId = product.CategoryID ?? 0,
                Category = product.Category != null ? new ProductCategoryInfo
                {
                    CategoryId = product.Category.ID,
                    CategoryName = product.Category.Name
                } : null,
                RequiresRefrigeration = product.RequiresRefrigeration,
                IsRecommended = product.IsRecommended,
                IsFeatured = false, // Not available in model
                Rating = rating,
                RatingCount = product.ReviewCount ?? product.Ratings?.Count ?? 0,
                Image = thumbnail,
                Tags = new List<string>(), // Not available in model
                VariantOptions = variantOptions,
                BaseHighlights = baseHighlights,
                Variants = variants.Select(v => MapToVariantResponse(v)).ToList(),
                IsActive = product.IsActive,
                CreatedDate = product.CreateDate,
                ModifiedDate = product.ModifiedDate
            };
        }

        private ProductVariantResponse MapToVariantResponse(CatalogVariants variant)
        {
            var attributes = new List<VariantAttribute>();
            if (!string.IsNullOrWhiteSpace(variant.Attributes))
            {
                attributes.Add(new VariantAttribute
                {
                    Name = "Option",
                    Value = variant.Attributes
                });
            }

            return new ProductVariantResponse
            {
                VariantId = variant.ID,
                Attributes = attributes,
                Sku = variant.SKU,
                SalePrice = (decimal)variant.SalePrice,
                CompareAtPrice = variant.CompareAtPrice.HasValue ? (decimal?)variant.CompareAtPrice.Value : null,
                MaximumOrderQuantity = variant.MaximumOrderQuantity ?? 10,
                MinimumOrderQuantity = variant.MinimumOrderQuantity ?? 1,
                Images = new List<string>(), // Can be populated from attachments if needed
                Thumbnail = null, // Can be populated from attachments if needed
                IsDefaultVariant = variant.IsDefaultVariant,
                StockQuantity = null, // Can be populated from inventory if needed
                TrackInventory = false // Not available in model
            };
        }

        #endregion
    }
}
