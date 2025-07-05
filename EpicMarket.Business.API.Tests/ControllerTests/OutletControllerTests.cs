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
    public class OutletControllerTests : BaseControllerTest<OutletController>
    {
        [Fact]
        public void Setup_OutletController()
        {
            // Arrange & Act
            Setup();
            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            // Assert
            Controller.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllOutlets_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletParams = new OutletListParams
            {
                Page = 1,
                PageSize = 10,
                SearchTerm = "branch"
            };

            var outletResults = TestDataHelper.GetTestOutletResults();

            MockOutletService.Setup(x => x.GetAllOutlets(It.IsAny<OutletListParams>(), It.IsAny<int>()))
                .ReturnsAsync(outletResults);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetAllOutlets(outletParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<OutletResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].Name.Should().Be("Test Outlet");
        }

        [Fact]
        public async Task GetOutletById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;
            var outletDetails = new OutletDetailsResult
            {
                BranchId = outletId,
                Name = "Test Outlet",
                Address = "123 Test Street",
                City = "Test City",
                State = "Test State",
                ContactNumber = "+1234567890",
                ContactEmail = "outlet@example.com",
                IsOpen = true,
                Rating = 4.5
            };

            MockOutletService.Setup(x => x.GetOutletById(outletId))
                .ReturnsAsync(outletDetails);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetOutletById(outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<OutletDetailsResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.BranchId.Should().Be(outletId);
            response.Data.Name.Should().Be("Test Outlet");
        }

        [Fact]
        public async Task CreateOutlet_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletDto = TestDataHelper.GetValidOutletRegisterDto();

            MockOutletService.Setup(x => x.CreateOutlet(
                It.IsAny<OutletRegisterDto>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(1);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.CreateOutlet(outletDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(1);
        }

        [Fact]
        public async Task UpdateOutlet_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;
            var updateDto = new UpdateOutletDto
            {
                Name = "Updated Outlet",
                Description = "Updated description",
                Address = "789 Updated Street",
                City = "Updated City",
                State = "Updated State",
                ContactNumber = "+1234567890",
                ContactEmail = "updated@example.com"
            };

            MockOutletService.Setup(x => x.UpdateOutlet(
                outletId,
                It.IsAny<UpdateOutletDto>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(outletId);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.UpdateOutlet(outletId, updateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(outletId);
        }

        [Fact]
        public async Task DeleteOutlet_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;

            MockOutletService.Setup(x => x.DeleteOutlet(outletId, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.DeleteOutlet(outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task MapEmployeesToOutlet_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var mapRequest = new MapEmployeesToOutletRequest
            {
                BranchId = 1,
                EmployeeIds = new List<int> { 1, 2, 3 }
            };

            MockOutletService.Setup(x => x.MapEmployeesToOutlet(mapRequest, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.MapEmployeesToOutlet(mapRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task MapProductsToOutlet_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var mapRequest = new MapProductsToOutletRequest
            {
                BranchId = 1,
                ProductVariantIds = new List<int> { 1, 2, 3 }
            };

            MockOutletService.Setup(x => x.MapProductsToOutlet(mapRequest, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.MapProductsToOutlet(mapRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateOutletStatus_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var statusRequest = new UpdateOutletStatusRequest
            {
                BranchId = 1,
                IsOpen = true
            };

            MockOutletService.Setup(x => x.UpdateOutletStatus(statusRequest, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.UpdateOutletStatus(statusRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetNearbyOutlets_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var searchParams = new NearbyOutletSearchParams
            {
                Latitude = 40.7128,
                Longitude = -74.0060,
                RadiusKm = 10,
                Category = "restaurant",
                MinRating = 4.0,
                SortBy = "rating",
                SortDirection = "desc",
                Page = 1,
                PageSize = 10
            };

            var nearbyOutlets = new GetDataResult<List<NearbyOutletResult>>
            {
                Data = new List<NearbyOutletResult>
                {
                    new NearbyOutletResult
                    {
                        OutletId = 1,
                        BusinessName = "Local Restaurant",
                        OutletName = "Main Branch",
                        Address = "123 Main St",
                        City = "Main City",
                        Distance = 2.5,
                        Rating = 4.5,
                        IsOpen = true,
                        ImageUrl = "https://example.com/outlet.jpg"
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            MockOutletService.Setup(x => x.GetNearbyOutlets(searchParams))
                .ReturnsAsync(nearbyOutlets);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            // Act
            var result = await Controller.GetNearbyOutlets(searchParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<NearbyOutletResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].BusinessName.Should().Be("Local Restaurant");
        }

        [Fact]
        public async Task GetCustomerOutletDetails_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;
            var customerOutletDetails = new CustomerOutletDetailsResult
            {
                OutletId = outletId,
                BusinessName = "Restaurant Name",
                OutletName = "Main Branch",
                Address = "123 Main St",
                City = "Main City",
                Phone = "+1234567890",
                Email = "contact@restaurant.com",
                Rating = 4.5,
                IsOpen = true,
                OpeningHours = "9:00 AM - 10:00 PM",
                Images = new List<string> { "image1.jpg", "image2.jpg" },
                IsSubscribed = false
            };

            MockOutletService.Setup(x => x.GetCustomerOutletDetails(outletId))
                .ReturnsAsync(customerOutletDetails);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            // Act
            var result = await Controller.GetCustomerOutletDetails(outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<CustomerOutletDetailsResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.OutletId.Should().Be(outletId);
            response.Data.BusinessName.Should().Be("Restaurant Name");
        }

        [Fact]
        public async Task SubscribeToOutlet_WithValidId_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var outletId = 1;

            MockOutletService.Setup(x => x.SubscribeToOutlet(outletId, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupMemberContext(Controller);

            // Act
            var result = await Controller.SubscribeToOutlet(outletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<bool>;
            response.Should().NotBeNull();
            response.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetSubscribedOutlets_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var subscriptionParams = new SubscriptionListParams
            {
                Page = 1,
                PageSize = 10
            };

            var subscribedOutlets = new GetDataResult<List<SubscribedOutletResult>>
            {
                Data = new List<SubscribedOutletResult>
                {
                    new SubscribedOutletResult
                    {
                        OutletId = 1,
                        BusinessName = "Subscribed Restaurant",
                        OutletName = "Main Branch",
                        Address = "123 Main St",
                        Rating = 4.5,
                        IsOpen = true,
                        SubscribedDate = DateTime.Now.AddDays(-30)
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            MockOutletService.Setup(x => x.GetSubscribedOutlets(subscriptionParams, It.IsAny<string>()))
                .ReturnsAsync(subscribedOutlets);

            Controller = new OutletController(
                MockLogger.Object,
                MockOutletService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockApplicationConfigurationService.Object);

            SetupMemberContext(Controller);

            // Act
            var result = await Controller.GetSubscribedOutlets(subscriptionParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<GetDataResult<List<SubscribedOutletResult>>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().HaveCount(1);
            response.Data.Data[0].BusinessName.Should().Be("Subscribed Restaurant");
        }

        protected override void Cleanup()
        {
            base.Cleanup();
        }
    }
}