using EpicMarket.Business.API.Controllers;
using EpicMarket.Business.API.Tests.TestHelpers;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EpicMarket.Business.API.Tests.ControllerTests
{
    public class UserControllerTests : BaseControllerTest<UserController>
    {
        [Fact]
        public void Setup_UserController()
        {
            // Arrange & Act
            Setup();
            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Assert
            Controller.Should().NotBeNull();
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            Setup();
            var registerDto = TestDataHelper.GetValidRegisterDto();
            var testUser = TestDataHelper.GetTestUser();

            MockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync((AppUser)null);

            MockMapper.Setup(x => x.Map<AppUser>(registerDto))
                .Returns(testUser);

            MockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            MockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "MEMBER"))
                .ReturnsAsync(IdentityResult.Success);

            MockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>()))
                .ReturnsAsync("mock-jwt-token");

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Register(registerDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);

            var response = createdResult.Value as OperationResult<TokenDto>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Token.Should().Be("mock-jwt-token");
        }

        [Fact]
        public async Task Register_WithNullData_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Register(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Invalid request");
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            var registerDto = TestDataHelper.GetValidRegisterDto();
            var existingUser = TestDataHelper.GetTestUser();

            MockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync(existingUser);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Register(registerDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Username is taken");
        }

        [Fact]
        public async Task RegisterBusiness_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            Setup();
            var registerDto = TestDataHelper.GetValidRegisterDto();
            var testUser = TestDataHelper.GetTestBusinessOwner();

            MockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync((AppUser)null);

            MockMapper.Setup(x => x.Map<AppUser>(registerDto))
                .Returns(testUser);

            MockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            MockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "BUSINESS_OWNER"))
                .ReturnsAsync(IdentityResult.Success);

            MockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>()))
                .ReturnsAsync("mock-jwt-token");

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.RegisterBusiness(registerDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);

            var response = createdResult.Value as OperationResult<TokenDto>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Token.Should().Be("mock-jwt-token");
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var loginDto = TestDataHelper.GetValidLoginDto();
            var testUser = TestDataHelper.GetTestUser();

            MockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(testUser);

            MockSignInManager.Setup(x => x.CheckPasswordSignInAsync(testUser, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            MockTokenService.Setup(x => x.CreateToken(testUser))
                .ReturnsAsync("mock-jwt-token");

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Login(loginDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<TokenDto>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Token.Should().Be("mock-jwt-token");
        }

        [Fact]
        public async Task Login_WithNullData_ReturnsBadRequest()
        {
            // Arrange
            Setup();
            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Login(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Invalid request");
        }

        [Fact]
        public async Task Login_WithInvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            Setup();
            var loginDto = TestDataHelper.GetValidLoginDto();

            MockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync((AppUser)null);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Login(loginDto);

            // Assert
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult.StatusCode.Should().Be(401);
            unauthorizedResult.Value.Should().Be("Invalid username");
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            Setup();
            var loginDto = TestDataHelper.GetValidLoginDto();
            var testUser = TestDataHelper.GetTestUser();

            MockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(testUser);

            MockSignInManager.Setup(x => x.CheckPasswordSignInAsync(testUser, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.Login(loginDto);

            // Assert
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult.StatusCode.Should().Be(401);
            unauthorizedResult.Value.Should().Be("Invalid password");
        }

        [Fact]
        public async Task LoginPhone_WithValidOTP_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var loginPhoneDto = TestDataHelper.GetValidLoginPhoneDto();
            var testUser = TestDataHelper.GetTestUser();

            MockOTPService.Setup(x => x.VerifyOTPAsync(loginPhoneDto.ReferenceId, loginPhoneDto.OTP))
                .ReturnsAsync(true);

            MockUserManager.Setup(x => x.Users)
                .Returns(new List<AppUser> { testUser }.AsQueryable());

            MockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>()))
                .ReturnsAsync("mock-jwt-token");

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.LoginPhone(loginPhoneDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<TokenDto>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Token.Should().Be("mock-jwt-token");
        }

        [Fact]
        public async Task LoginPhone_WithInvalidOTP_ReturnsUnauthorized()
        {
            // Arrange
            Setup();
            var loginPhoneDto = TestDataHelper.GetValidLoginPhoneDto();

            MockOTPService.Setup(x => x.VerifyOTPAsync(loginPhoneDto.ReferenceId, loginPhoneDto.OTP))
                .ReturnsAsync(false);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.LoginPhone(loginPhoneDto);

            // Assert
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult.StatusCode.Should().Be(401);
            unauthorizedResult.Value.Should().Be("Invalid OTP");
        }

        [Fact]
        public async Task GetUserInfo_WithValidUser_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var testUser = TestDataHelper.GetTestBusinessOwner();
            var userBusinessDto = new UserBusinessDto
            {
                businessId = 1,
                businessStatus = "Active"
            };

            MockUserManager.Setup(x => x.FindByNameAsync("businessowner@example.com"))
                .ReturnsAsync(testUser);

            MockUserManager.Setup(x => x.GetRolesAsync(testUser))
                .ReturnsAsync(new List<string> { "BUSINESS_OWNER" });

            MockProfileService.Setup(x => x.GetAccessControlList(It.IsAny<Profile_SearchParams>()))
                .Returns(new List<AccessControlList_Result>());

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetUserInfo();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<LoginResult>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.UserDetails.Should().NotBeNull();
            response.Data.UserDetails.Username.Should().Be(testUser.UserName);
        }

        [Fact]
        public async Task GetUserInfo_WithInvalidUser_ReturnsNotFound()
        {
            // Arrange
            Setup();

            MockUserManager.Setup(x => x.FindByNameAsync("businessowner@example.com"))
                .ReturnsAsync((AppUser)null);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.GetUserInfo();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("User not found");
        }

        [Fact]
        public async Task PutInfo_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var testUser = TestDataHelper.GetTestBusinessOwner();
            var editDto = new LoginUserEditDTO
            {
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "Name",
                Phone = "+1234567890"
            };

            MockUserManager.Setup(x => x.FindByNameAsync("businessowner@example.com"))
                .ReturnsAsync(testUser);

            MockUserManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.PutInfo(editDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<string>;
            response.Should().NotBeNull();
            response.Data.Should().Be("User updated successfully.");
        }

        [Fact]
        public async Task ChangePassword_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var testUser = TestDataHelper.GetTestBusinessOwner();
            var changePasswordParams = TestDataHelper.GetValidChangePasswordParams();

            MockUserManager.Setup(x => x.FindByNameAsync("businessowner@example.com"))
                .ReturnsAsync(testUser);

            MockUserManager.Setup(x => x.ChangePasswordAsync(testUser, changePasswordParams.CurrentPassword, changePasswordParams.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            SetupBusinessOwnerContext(Controller);

            // Act
            var result = await Controller.ChangePassword(changePasswordParams);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<string>;
            response.Should().NotBeNull();
            response.Data.Should().Be("Password changed successfully");
        }

        [Fact]
        public async Task SendOTP_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var otpRequest = TestDataHelper.GetValidOTPRequest();
            var otpResponse = TestDataHelper.GetValidOTPResponse();

            MockOTPService.Setup(x => x.SendOTPAsync(otpRequest.Phone, otpRequest.Purpose))
                .ReturnsAsync(otpResponse);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.SendOTP(otpRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<OTPResponse>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.ReferenceId.Should().Be(otpResponse.ReferenceId);
        }

        [Fact]
        public async Task GetCustomerBasicDetails_WithValidUser_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var customerDetails = new CustomerBasicDetailsDto
            {
                FirstName = "Test",
                LastName = "Customer",
                Email = "customer@example.com",
                Phone = "+1234567890"
            };

            MockProfileService.Setup(x => x.GetCustomerBasicDetailsAsync("testuser@example.com"))
                .ReturnsAsync(customerDetails);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            SetupMemberContext(Controller);

            // Act
            var result = await Controller.GetCustomerBasicDetails();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as CustomerBasicDetailsDto;
            response.Should().NotBeNull();
            response.FirstName.Should().Be(customerDetails.FirstName);
            response.Email.Should().Be(customerDetails.Email);
        }

        [Fact]
        public async Task GetPersonDetails_WithValidData_ReturnsOkResult()
        {
            // Arrange
            Setup();
            var phoneOrUsername = "+1234567890";
            var customerDetailsList = new List<CustomerDetails>
            {
                new CustomerDetails
                {
                    FirstName = "Test",
                    LastName = "Customer",
                    Email = "customer@example.com",
                    Phone = phoneOrUsername
                }
            };

            MockProfileService.Setup(x => x.GetCustomerDetails(phoneOrUsername))
                .ReturnsAsync(customerDetailsList);

            Controller = new UserController(
                MockUserManager.Object,
                MockSignInManager.Object,
                MockTokenService.Object,
                MockMapper.Object,
                MockLogger.Object,
                MockCommunicationService.Object,
                DbContext,
                MockHttpContextAccessor.Object,
                MockProfileService.Object,
                MockConfiguration.Object,
                MockOTPService.Object);

            // Act
            var result = await Controller.GetPersonDetails(phoneOrUsername);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var response = okResult.Value as OperationResult<List<CustomerDetails>>;
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(1);
            response.Data[0].Phone.Should().Be(phoneOrUsername);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
        }
    }
}