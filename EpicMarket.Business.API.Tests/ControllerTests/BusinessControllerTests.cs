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
    public class BusinessControllerTests : BaseControllerTest<BusinessController>
    {
        [Fact]
        public void Setup_BusinessController()
        {
            // Arrange & Act
            Setup();
            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Assert
            Controller.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterBusiness_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            Setup();
            var businessDto = TestDataHelper.GetValidBusinessRegisterDto();
            var businessResult = new BusinessDTO_Result
            {
                BusinessId = 1,
                ProofId = 1
            };

            MockBusinessService.Setup(x => x.RegisterBusiness(
                It.IsAny<BusinessRegisterDto>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(businessResult);

            MockUserManager.Setup(x => x.Users)
                .Returns(new List<AppUser> { TestDataHelper.GetTestBusinessOwner() }.AsQueryable());

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.RegisterBusiness(businessDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);

            var response = createdResult.Value as OperationResult<BusinessDTO_Result>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.BusinessId.Should().Be(1);
            response.Data.ProofId.Should().Be(1);
        }

        [Fact]
        public async Task RegisterBusiness_WithNullData_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.RegisterBusiness(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Invalid request");
        }

        [Fact]
        public async Task RegisterBusiness_WithServiceFailure_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            var businessDto = TestDataHelper.GetValidBusinessRegisterDto();

            MockBusinessService.Setup(x => x.RegisterBusiness(
                It.IsAny<BusinessRegisterDto>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync((BusinessDTO_Result)null);

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.RegisterBusiness(businessDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Failed to register business");
        }

        [Fact]
        public async Task GetBusinessByID_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var businessDetails = new BusinessDetailResult
            {
                BusinessId = 1,
                BusinessName = "Test Business",
                ContactNumber = "+1234567890",
                ContactEmail = "business@example.com",
                Address = "123 Test Street",
                City = "Test City",
                State = "Test State",
                Status = "Active",
                Description = "Test business description"
            };

            MockBusinessService.Setup(x => x.GetBusinessByID(It.IsAny<int>()))
                .ReturnsAsync(businessDetails);

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetBusinessByID();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<BusinessDetailResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.BusinessId.Should().Be(1);
            response.Data.BusinessName.Should().Be("Test Business");
        }

        [Fact]
        public async Task GetBusinessByID_WithNonExistentBusiness_ReturnsNotFound()
        {
            // Arrange
            Setup();

            MockBusinessService.Setup(x => x.GetBusinessByID(It.IsAny<int>()))
                .ReturnsAsync((BusinessDetailResult)null);

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetBusinessByID();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("Business details not found");
        }

        [Fact]
        public async Task UpdateBusiness_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var updateDto = new UpdateBusinessRegisterDto
            {
                BusinessName = "Updated Business Name",
                ContactNumber = "+1234567890",
                ContactEmail = "updated@business.com",
                Address = "456 Updated Street",
                City = "Updated City",
                State = "Updated State",
                Description = "Updated description"
            };

            MockBusinessService.Setup(x => x.UpdateBusiness(
                It.IsAny<int>(),
                It.IsAny<UpdateBusinessRegisterDto>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(1);

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.UpdateBusiness(updateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<int>;
            response.Should().NotBeNull();
            response.Data.Should().Be(1);
        }

        [Fact]
        public async Task GetBusinessCategories_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var categories = TestDataHelper.GetTestBusinessCategories();

            MockBusinessService.Setup(x => x.GetBusinessCategories())
                .ReturnsAsync(categories);

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Act
            var result = await Controller.GetBusinessCategories();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<BusinessCategoryDto>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(2);
            response.Data[0].Name.Should().Be("Restaurant");
            response.Data[1].Name.Should().Be("Retail");
        }

        [Fact]
        public async Task GetBusinessCategories_WithNoCategories_ReturnsNotFound()
        {
            // Arrange
            Setup();

            MockBusinessService.Setup(x => x.GetBusinessCategories())
                .ReturnsAsync(new List<BusinessCategoryDto>());

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Act
            var result = await Controller.GetBusinessCategories();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("No business categories available");
        }

        [Fact]
        public async Task GetBusinessCategories_WithServiceException_ReturnsInternalServerError()
        {
            // Arrange
            Setup();

            MockBusinessService.Setup(x => x.GetBusinessCategories())
                .ThrowsAsync(new Exception("Service error"));

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Act
            var result = await Controller.GetBusinessCategories();

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be("An error occurred while retrieving business categories");
        }

        [Fact]
        public async Task GetBusinessListings_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var businessListings = new BusinessGroupsResponseDto
            {
                BusinessGroups = new List<BusinessGroupDto>
                {
                    new BusinessGroupDto
                    {
                        Title = "Trending",
                        Businesses = new List<BusinessListingDto>
                        {
                            new BusinessListingDto
                            {
                                BusinessId = 1,
                                BusinessName = "Popular Restaurant",
                                Rating = 4.5,
                                ImageUrl = "https://example.com/business.jpg",
                                Category = "Restaurant"
                            }
                        }
                    }
                }
            };

            MockBusinessService.Setup(x => x.GetBusinessListings(It.IsAny<string>()))
                .ReturnsAsync(businessListings);

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Act
            var result = await Controller.GetBusinessListings("restaurant");

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<BusinessGroupsResponseDto>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.BusinessGroups.Should().HaveCount(1);
            response.Data.BusinessGroups[0].Title.Should().Be("Trending");
            response.Data.BusinessGroups[0].Businesses.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetBusinessListings_WithNoListings_ReturnsNotFound()
        {
            // Arrange
            Setup();

            MockBusinessService.Setup(x => x.GetBusinessListings(It.IsAny<string>()))
                .ReturnsAsync(new BusinessGroupsResponseDto
                {
                    BusinessGroups = new List<BusinessGroupDto>()
                });

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Act
            var result = await Controller.GetBusinessListings("nonexistent");

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("No business listings available");
        }

        [Fact]
        public async Task GetBusinessListings_WithServiceException_ReturnsInternalServerError()
        {
            // Arrange
            Setup();

            MockBusinessService.Setup(x => x.GetBusinessListings(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Service error"));

            Controller = new BusinessController(
                MockLogger.Object,
                MockUserManager.Object,
                MockBusinessService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockAttachmentService.Object,
                MockFileService.Object,
                MockCommunicationService.Object,
                MockApplicationConfigurationService.Object,
                MockConfiguration.Object);

            // Act
            var result = await Controller.GetBusinessListings("restaurant");

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be("An error occurred while retrieving business listings");
        }

        protected override void Cleanup()
        {
            base.Cleanup();
        }
    }
}