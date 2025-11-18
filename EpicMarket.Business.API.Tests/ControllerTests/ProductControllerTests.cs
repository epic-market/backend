using EpicMarket.Business.API.Controllers;
using EpicMarket.Business.API.Tests.TestHelpers;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EpicMarket.Business.API.Tests.ControllerTests
{
    public class ProductControllerTests : BaseControllerTest<ProductController>
    {
        [Fact]
        public void Setup_ProductController()
        {
            // Arrange & Act
            Setup();
            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Assert
            Controller.Should().NotBeNull();
        }

        [Fact]
        public async Task AddProduct_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productDto = TestDataHelper.GetValidAddProductsParams();

            MockProductService.Setup(x => x.AddProduct(
                It.IsAny<AddProductsParams>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(1);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.AddProduct(productDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(1);
        }

        [Fact]
        public async Task UpdateProduct_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productDto = TestDataHelper.GetValidAddProductsParams();
            var productId = 1;

            MockProductService.Setup(x => x.UpdateProducts(
                It.IsAny<AddProductsParams>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(productId);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.UpdateProduct(productId, productDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(productId);
        }

        [Fact]
        public async Task GetAllProducts_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productParams = new ProductListParams
            {
                Page = 1,
                PageSize = 10,
                SearchTerm = "test",
                CategoryId = 1
            };

            var productResults = TestDataHelper.GetTestProductResults();

            MockProductService.Setup(x => x.GetAllProducts(It.IsAny<ProductListParams>(), It.IsAny<int>()))
                .ReturnsAsync(productResults);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetAllProducts(productParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<ProductResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].ProductName.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetProductDetails_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productId = 1;
            var productDetails = new ProductDetailsResult
            {
                ProductId = productId,
                ProductName = "Test Product",
                Description = "Test product description",
                BasePrice = 29.99m,
                CategoryName = "Electronics",
                IsActive = true
            };

            MockProductService.Setup(x => x.GetProductDetails(productId))
                .ReturnsAsync(productDetails);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetProductDetails(productId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<ProductDetailsResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.ProductId.Should().Be(productId);
            response.Data.ProductName.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetProductDetailsV2_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productId = 1;
            var productDetails = new ProductDetailsV2Result
            {
                ProductId = productId,
                ProductName = "Test Product",
                Description = "Test product description",
                BasePrice = 29.99m,
                CategoryName = "Electronics",
                IsActive = true
            };

            MockProductService.Setup(x => x.GetProductDetailsV2(productId))
                .ReturnsAsync(productDetails);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Act
            var result = await Controller.GetProductDetailsV2(productId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<ProductDetailsV2Result>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.ProductId.Should().Be(productId);
            response.Data.ProductName.Should().Be("Test Product");
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productId = 1;

            MockProductService.Setup(x => x.deleteCatelog(productId, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.Delete(productId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task QuickActions_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var quickActionsParams = new QuickActionsParams
            {
                Action = "ACTIVATE",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            MockProductService.Setup(x => x.QuickActions(quickActionsParams, It.IsAny<string>()))
                .ReturnsAsync(1);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.QuickActions(quickActionsParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(1);
        }

        [Fact]
        public async Task AddRatingToProduct_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var ratingRequest = new AddProductRatingRequest
            {
                ProductId = 1,
                Rating = 5,
                Comment = "Excellent product!"
            };

            MockRatingService.Setup(x => x.AddProductRatingAsync(ratingRequest, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.AddRatingToProduct(ratingRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllProductForMap_WithValidOutletId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;
            var productMapResults = new List<ProductsMapOptionResult>
            {
                new ProductsMapOptionResult
                {
                    ProductId = 1,
                    ProductName = "Test Product",
                    IsSelected = true
                }
            };

            MockProductService.Setup(x => x.GetAllProductForMap(It.IsAny<int>(), outletId))
                .ReturnsAsync(productMapResults);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetAllProductForMap(outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<ProductsMapOptionResult>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(1);
            response.Data[0].ProductName.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetProductInventoryDetails_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productVariantId = 1;
            var branchId = 1;
            var inventoryResult = new InventoryResult
            {
                ProductVariantId = productVariantId,
                BranchId = branchId,
                Quantity = 100,
                MinStock = 10,
                MaxStock = 200
            };

            MockProductService.Setup(x => x.GetProductInventoryDetails(productVariantId, branchId))
                .ReturnsAsync(inventoryResult);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetProductInventoryDetails(productVariantId, branchId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<InventoryResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.ProductVariantId.Should().Be(productVariantId);
            response.Data.BranchId.Should().Be(branchId);
            response.Data.Quantity.Should().Be(100);
        }

        [Fact]
        public async Task AddOrUpdateProductInventoryDetails_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var inventoryData = new InventoryResult
            {
                ProductVariantId = 1,
                BranchId = 1,
                Quantity = 100,
                MinStock = 10,
                MaxStock = 200
            };

            MockProductService.Setup(x => x.AddOrUpdateProductInventoryDetails(inventoryData))
                .Returns(Task.CompletedTask);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.AddOrUpdateProductInventoryDetails(inventoryData);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllProductsForPOS_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;
            var productPOSParams = new ProductPOSParams
            {
                Page = 1,
                PageSize = 20,
                SearchTerm = "test"
            };

            var posResults = new GetDataResult<List<ProductForPOSResult>>
            {
                Data = new List<ProductForPOSResult>
                {
                    new ProductForPOSResult
                    {
                        ProductId = 1,
                        ProductName = "Test Product",
                        Price = 29.99m,
                        Stock = 100
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 20
            };

            MockProductService.Setup(x => x.GetAllProductsForPOS(productPOSParams, outletId))
                .ReturnsAsync(posResults);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetAllProductsForPOS(productPOSParams, outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<ProductForPOSResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].ProductName.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetCategoriesByOutlet_WithValidOutletId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;
            var categories = new List<CategoriesDto>
            {
                new CategoriesDto
                {
                    Id = 1,
                    Name = "Electronics",
                    Description = "Electronic items"
                }
            };

            MockProductCategoryService.Setup(x => x.GetCategoriesByOutletId(outletId))
                .ReturnsAsync(categories);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Act
            var result = await Controller.GetCategoriesByOutlet(outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<CategoriesDto>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(1);
            response.Data[0].Name.Should().Be("Electronics");
        }

        [Fact]
        public async Task GetCustomerProducts_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var parameters = new ProductMobileParams
            {
                OutletId = 1,
                CategoryId = 1,
                SearchTerm = "test",
                Page = 1,
                PageSize = 10
            };

            var customerProducts = new GetDataResult<List<CustomerProductResult>>
            {
                Data = new List<CustomerProductResult>
                {
                    new CustomerProductResult
                    {
                        ProductId = 1,
                        ProductName = "Customer Product",
                        Price = 29.99m,
                        DiscountedPrice = 24.99m,
                        Rating = 4.5,
                        IsAvailable = true
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            MockProductService.Setup(x => x.GetAllProductsForMobile(parameters))
                .ReturnsAsync(customerProducts);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Act
            var result = await Controller.GetCustomerProducts(parameters);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<CustomerProductResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].ProductName.Should().Be("Customer Product");
        }

        [Fact]
        public async Task GetCustomerProducts_WithInvalidOutletId_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            var parameters = new ProductMobileParams
            {
                OutletId = 0, // Invalid outlet ID
                CategoryId = 1,
                SearchTerm = "test",
                Page = 1,
                PageSize = 10
            };

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Act
            var result = await Controller.GetCustomerProducts(parameters);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Valid outlet ID is required");
        }

        [Fact]
        public async Task GetCustomerProductDetails_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productId = 1;
            var customerProductDetails = new CustomerProductDetailsResult
            {
                ProductId = productId,
                ProductName = "Customer Product",
                Description = "Product for customers",
                Price = 29.99m,
                DiscountedPrice = 24.99m,
                Rating = 4.5,
                IsAvailable = true
            };

            MockProductService.Setup(x => x.GetCustomerProductDetails(productId))
                .ReturnsAsync(customerProductDetails);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Act
            var result = await Controller.GetCustomerProductDetails(productId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<CustomerProductDetailsResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.ProductId.Should().Be(productId);
            response.Data.ProductName.Should().Be("Customer Product");
        }

        [Fact]
        public async Task GetCustomerProductDetails_WithException_ReturnsNotFound()
        {
            // Arrange
            Setup();
            var productId = 1;

            MockProductService.Setup(x => x.GetCustomerProductDetails(productId))
                .ThrowsAsync(new Exception("Product not found"));

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            // Act
            var result = await Controller.GetCustomerProductDetails(productId);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProductVariants_WithValidProductId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var productId = 1;
            var variants = new List<SingleProductVariantsResult>
            {
                new SingleProductVariantsResult
                {
                    VariantId = 1,
                    Name = "Red",
                    Price = 29.99m,
                    Stock = 100,
                    SKU = "PROD-RED-001"
                }
            };

            MockProductService.Setup(x => x.GetProductVariants(productId))
                .ReturnsAsync(variants);

            Controller = new ProductController(
                MockLogger.Object,
                MockProductService.Object,
                MockApplicationConfigurationService.Object,
                MockRatingService.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockProductCategoryService.Object,
                DbContext,
                MockHttpContextAccessor.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetProductVariants(productId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<SingleProductVariantsResult>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(1);
            response.Data[0].Name.Should().Be("Red");
        }

        protected override void Cleanup()
        {
            base.Cleanup();
        }
    }
}