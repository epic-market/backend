using AutoMapper;
using EpicMarket.Business.API.Controllers;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace EpicMarket.Business.API.Tests.TestHelpers
{
    public abstract class BaseControllerTest<TController> where TController : ControllerBase
    {
        protected Mock<UserManager<AppUser>> MockUserManager { get; set; }
        protected Mock<SignInManager<AppUser>> MockSignInManager { get; set; }
        protected Mock<ITokenService> MockTokenService { get; set; }
        protected Mock<IMapper> MockMapper { get; set; }
        protected Mock<ILogger<TController>> MockLogger { get; set; }
        protected Mock<ICommunicationService> MockCommunicationService { get; set; }
        protected Mock<IHttpContextAccessor> MockHttpContextAccessor { get; set; }
        protected Mock<IConfiguration> MockConfiguration { get; set; }
        protected Mock<IOTPService> MockOTPService { get; set; }
        protected Mock<IBusinessService> MockBusinessService { get; set; }
        protected Mock<IProductService> MockProductService { get; set; }
        protected Mock<IOutletService> MockOutletService { get; set; }
        protected Mock<IOrderService> MockOrderService { get; set; }
        protected Mock<IEmployeeService> MockEmployeeService { get; set; }
        protected Mock<IProfileService> MockProfileService { get; set; }
        protected Mock<IAttachmentService> MockAttachmentService { get; set; }
        protected Mock<IFileService> MockFileService { get; set; }
        protected Mock<IApplicationConfigurationService> MockApplicationConfigurationService { get; set; }
        protected Mock<IRatingService> MockRatingService { get; set; }
        protected Mock<IProductCategoryService> MockProductCategoryService { get; set; }
        protected Mock<IDashboardService> MockDashboardService { get; set; }
        protected Mock<INotificationService> MockNotificationService { get; set; }
        protected Mock<IReviewService> MockReviewService { get; set; }
        protected Mock<ISearchService> MockSearchService { get; set; }
        protected Mock<ISupportService> MockSupportService { get; set; }
        protected Mock<IStaticContentService> MockStaticContentService { get; set; }
        protected Mock<IActivityService> MockActivityService { get; set; }
        protected Mock<IHomeService> MockHomeService { get; set; }

        protected ApplicationDbContext DbContext { get; set; }
        protected TController Controller { get; set; }

        protected virtual void Setup()
        {
            // Setup DbContext with InMemory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            DbContext = new ApplicationDbContext(options);

            // Setup mocks
            MockUserManager = MockUserManagerHelper.GetMockUserManager<AppUser>();
            MockSignInManager = MockSignInManagerHelper.GetMockSignInManager<AppUser>();
            MockTokenService = new Mock<ITokenService>();
            MockMapper = new Mock<IMapper>();
            MockLogger = new Mock<ILogger<TController>>();
            MockCommunicationService = new Mock<ICommunicationService>();
            MockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            MockConfiguration = new Mock<IConfiguration>();
            MockOTPService = new Mock<IOTPService>();
            MockBusinessService = new Mock<IBusinessService>();
            MockProductService = new Mock<IProductService>();
            MockOutletService = new Mock<IOutletService>();
            MockOrderService = new Mock<IOrderService>();
            MockEmployeeService = new Mock<IEmployeeService>();
            MockProfileService = new Mock<IProfileService>();
            MockAttachmentService = new Mock<IAttachmentService>();
            MockFileService = new Mock<IFileService>();
            MockApplicationConfigurationService = new Mock<IApplicationConfigurationService>();
            MockRatingService = new Mock<IRatingService>();
            MockProductCategoryService = new Mock<IProductCategoryService>();
            MockDashboardService = new Mock<IDashboardService>();
            MockNotificationService = new Mock<INotificationService>();
            MockReviewService = new Mock<IReviewService>();
            MockSearchService = new Mock<ISearchService>();
            MockSupportService = new Mock<ISupportService>();
            MockStaticContentService = new Mock<IStaticContentService>();
            MockActivityService = new Mock<IActivityService>();
            MockHomeService = new Mock<IHomeService>();

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            MockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            // Setup default return values
            MockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>()))
                .ReturnsAsync("mock-jwt-token");

            MockOTPService.Setup(x => x.VerifyOTPAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            MockOTPService.Setup(x => x.SendOTPAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(TestDataHelper.GetValidOTPResponse());

            // Setup default configuration values
            MockConfiguration.Setup(x => x[It.IsAny<string>()])
                .Returns("test-config-value");

            // Setup default application configuration
            MockApplicationConfigurationService.Setup(x => x.GetApplicationConfigurationValue(It.IsAny<string>()))
                .Returns("/test/path");

            // Setup default file service
            MockFileService.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync("test-file-key");

            // Setup default attachment service
            MockAttachmentService.Setup(x => x.GetAttachmentId(It.IsAny<string>()))
                .ReturnsAsync(1);

            MockAttachmentService.Setup(x => x.InsertAttachmentLink(It.IsAny<AttachmentLinkDTO>(), It.IsAny<int>()))
                .ReturnsAsync(1);

            // Setup default communication service
            MockCommunicationService.Setup(x => x.SendTemplatedEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            MockCommunicationService.Setup(x => x.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Setup default services
            MockBusinessService.Setup(x => x.GetBusinessCategories())
                .ReturnsAsync(TestDataHelper.GetTestBusinessCategories());

            MockProductService.Setup(x => x.GetAllProducts(It.IsAny<ProductListParams>(), It.IsAny<int>()))
                .ReturnsAsync(TestDataHelper.GetTestProductResults());

            MockOutletService.Setup(x => x.GetAllOutlets(It.IsAny<OutletListParams>(), It.IsAny<int>()))
                .ReturnsAsync(TestDataHelper.GetTestOutletResults());

            MockOrderService.Setup(x => x.GetAllOrders(It.IsAny<OrderListParams>(), It.IsAny<int>()))
                .ReturnsAsync(TestDataHelper.GetTestOrderResults());

            MockEmployeeService.Setup(x => x.GetAllEmployees(It.IsAny<EmployeeListParams>(), It.IsAny<int>()))
                .ReturnsAsync(TestDataHelper.GetTestEmployeeResults());

            // Setup UserManager default behavior
            MockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            MockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            MockUserManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);

            MockUserManager.Setup(x => x.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            MockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(new List<string> { "BUSINESS_OWNER" });

            // Setup SignInManager default behavior
            MockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            // Setup default test data in DbContext
            SeedTestData();
        }

        protected virtual void SeedTestData()
        {
            // Add test users
            var testUser = TestDataHelper.GetTestUser();
            var businessOwner = TestDataHelper.GetTestBusinessOwner();
            
            DbContext.Users.AddRange(testUser, businessOwner);

            // Add test business
            var testBusiness = new Business
            {
                ID = 1,
                PersonID = businessOwner.Id,
                BusinessName = "Test Business",
                ContactNumber = "+1234567890",
                ContactEmail = "business@example.com",
                Address = "123 Test Street",
                City = "Test City",
                State = "Test State",
                StatusId = 1,
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            DbContext.Businesses.Add(testBusiness);

            // Add test categories
            var testCategory = new CatelogCategorys
            {
                ID = 1,
                Name = "Electronics",
                Description = "Electronic items",
                IsActive = true,
                CreatedBy = "system",
                CreatedDate = DateTime.Now
            };

            DbContext.CatelogCategorys.Add(testCategory);

            // Add test business category
            var businessCategory = new BusinessCategory
            {
                ID = 1,
                Name = "Restaurant",
                Description = "Food & Beverage",
                IsActive = true,
                CreatedBy = "system",
                CreatedDate = DateTime.Now
            };

            DbContext.BusinessCategories.Add(businessCategory);

            // Add test roles
            var roles = new List<IdentityRole<int>>
            {
                new IdentityRole<int> { Id = 1, Name = "BUSINESS_OWNER", NormalizedName = "BUSINESS_OWNER" },
                new IdentityRole<int> { Id = 2, Name = "BUSINESS_EMPLOYEE", NormalizedName = "BUSINESS_EMPLOYEE" },
                new IdentityRole<int> { Id = 3, Name = "MEMBER", NormalizedName = "MEMBER" }
            };

            DbContext.Roles.AddRange(roles);

            DbContext.SaveChanges();
        }

        protected void SetupControllerContext(TController controller, List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
            controller.ControllerContext = context;
        }

        protected void SetupBusinessOwnerContext(TController controller)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "2"),
                new Claim(ClaimTypes.Name, "businessowner@example.com"),
                new Claim(ClaimTypes.Role, "BUSINESS_OWNER")
            };
            SetupControllerContext(controller, claims);
        }

        protected void SetupMemberContext(TController controller)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser@example.com"),
                new Claim(ClaimTypes.Role, "MEMBER")
            };
            SetupControllerContext(controller, claims);
        }

        protected void SetupEmployeeContext(TController controller)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "3"),
                new Claim(ClaimTypes.Name, "employee@example.com"),
                new Claim(ClaimTypes.Role, "BUSINESS_EMPLOYEE")
            };
            SetupControllerContext(controller, claims);
        }

        protected virtual void Cleanup()
        {
            DbContext?.Dispose();
        }
    }
}