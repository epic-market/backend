using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Collections.Generic;
using EpicMarket.Services;

//verified in postman
namespace EpicMarket.Business.API.Controllers
{

    [Route("api/user")]
    public class UserController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> logger;
        private readonly ICommunicationService communication;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IProfileService profileService;
        private readonly IConfiguration _configuration;
        public UserController(
                                    UserManager<AppUser> userManager,
                                    SignInManager<AppUser> signInManager,
                                    ITokenService tokenService,
                                    IMapper mapper,
                                    ILogger<UserController> logger,
                                    ICommunicationService communication,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IProfileService profileService,
                                    IConfiguration configuration
                                    ) : base(dbContext, httpContextAccessor)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            this.logger = logger;
            this.communication = communication;
            _tokenService = tokenService;
            this.profileService = profileService;
            _configuration = configuration;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<TokenDto>>> Register(RegisterDto registerDto)
        {
            try
            {
                logger.LogInformation("Account Controller -> Register()-> params {0}", JsonConvert.SerializeObject(new { Params = registerDto }));

                var response = new OperationResult<TokenDto>();

                if (registerDto == null)
                {
                    logger.LogError("RegisterDto is null");
                    return BadRequest("Invalid request");
                }

                if (await UserExists(registerDto.Email))
                {
                    logger.LogWarning("Username is already taken for email: {email}", registerDto.Email);
                    return BadRequest("Username is taken");
                }

                var user = _mapper.Map<AppUser>(registerDto);

                user.UserName = registerDto.Email.ToLower();
                user.Email = registerDto.Email.ToLower();
                user.PhoneNumber = registerDto.Phone;

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    logger.LogError("User creation failed with errors: {@errors}", result.Errors);
                    return BadRequest(result.Errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, ROLES.MEMBER);

                if (!roleResult.Succeeded)
                {
                    logger.LogError("Adding user to role failed with errors: {@errors}", roleResult.Errors);
                    return BadRequest(result.Errors);
                }

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EmailTemplates");
                var emailService = new EmailService(templatePath, _configuration); // Pass IConfiguration to EmailService
                var model = new 
                {
                    Document_Upload_URL = "owner.epicmarket.in", // Replace with actual URL
                    Support_Email = "support@epicmarket.in", // Replace with actual support email
                    Support_Phone = "123-456-7890", // Replace with actual support phone number
                    Current_Year = DateTime.Now.Year.ToString(),
                    Company_Address = "123 Epic Market St, Business City, BC 12345" // Replace with actual address
                };
                await emailService.SendEmailAsync("BusinessRegisterSuccussfullyAskingToCompleteDetetials", model, registerDto.Email, "Business Registration Successful - Complete Your Details");

                response.Data = new TokenDto
                {
                    Token = await _tokenService.CreateToken(user)
                };

                logger.LogInformation("Account Controller -> Register()-> return {0}", JsonConvert.SerializeObject(new { Value = response }));
                logger.LogInformation("User registered successfully with email: {email}", registerDto.Email);
                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while registering user with email: {email}", registerDto.Email);
                return StatusCode(500, "An error occurred while registering user");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<TokenDto>>> Login(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                logger.LogError("LoginDto is null");
                return BadRequest("Invalid request");
            }

            var user = await GetUser(loginDto.Email);

            if (user == null)
            {
                logger.LogWarning("User not found for email: {email}", loginDto.Email);
                return Unauthorized("Invalid username");
            }

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                logger.LogError("Password check failed for user: {email}", loginDto.Email);
                return Unauthorized("Invalid password");
            }

            var token = await _tokenService.CreateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                logger.LogError("Failed to generate token for user: {email}", loginDto.Email);
                return StatusCode(500, "Failed to generate token");
            }

            var response = new OperationResult<TokenDto>
            {
                Data = new TokenDto
                {
                    Token = token
                }
            };

            logger.LogInformation("User logged in successfully with email: {email}", loginDto.Email);
            return Ok(response);
        }
        
        [HttpGet("info")]
        [Authorize]
        public async Task<ActionResult<OperationResult<LoginResult>>> GetUserInfo()
        {
            try
            {
                var response = new OperationResult<LoginResult>();
                var user = await GetUser(this.LoggedInUserName); ;

                if (user == null) 
                {
                    logger.LogWarning("User not found for username: {username}", this.LoggedInUserName);
                    return NotFound("User not found");
                }

                var roles = await _userManager.GetRolesAsync(user);

                var userBusinessDto = new UserBusinessDto();

                if (roles.Contains(ROLES.BUSINESS_OWNER))
                {
                    userBusinessDto = dbContext.Businesses.Where(c => c.PersonID == user.Id).Include(c => c.Status).Select(c => new UserBusinessDto()
                    {
                        businessId = c.ID == 0 ? null : c.ID,
                        businessStatus = c.Status.Status,
                    }).FirstOrDefault();
                }
                else if (roles.Contains(ROLES.BUSINESS_EMPLOYEE))
                {
                    userBusinessDto = dbContext.BusinessEmployeeMaps.Where(c => c.EmployeeID == user.Id).Include(c => c.Bussiness).Include(c => c.Bussiness.Status).Select(c => new UserBusinessDto()
                    {
                        businessId = c.Bussiness.ID == 0 ? null : c.Bussiness.ID,
                        businessStatus = c.Bussiness.Status.Status,
                    }).FirstOrDefault();
                }
                List<AccessControlList_Result> permissions= this.profileService.GetAccessControlList(new Profile_SearchParams() { ApplicationName = null, LoggedInUserName = this.LoggedInUserName, UserRole = null });

                response.Data = new LoginResult()
                {
                    UserDetails = new UserLoginDto
                    {
                        Username = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Phone = user.PhoneNumber
                    },
                    UserBusiness = userBusinessDto,
                    Securables=permissions
                };

                logger.LogInformation("User info retrieved successfully for username: {username}", this.LoggedInUserName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while retrieving user info for username: {username}. Error: {error}", this.LoggedInUserName, ex.Message);
                return StatusCode(500, "An error occurred while retrieving user info");
            }
        }
        
        [HttpPut("info")]
        [Authorize]
        public async Task<ActionResult<OperationResult<string>>> PutInfo(LoginUserEditDTO loginUserEditDTO)
        {
            var user = await GetUser(this.LoggedInUserName);

            // Logging and handling the case where the user is not found
            if (user == null)
            {
                logger.LogError("Invalid username, Please LogOut and LogIn");
                return Unauthorized("Invalid username, Please LogOut and LogIn");
            }

            // Updating user information
            user.Email = loginUserEditDTO.Email;
            user.PhoneNumber = loginUserEditDTO.Phone;
            user.FirstName = loginUserEditDTO.FirstName;
            user.LastName = loginUserEditDTO.LastName;
            user.UserName = loginUserEditDTO.Email;
            
            // Attempting to update the user
            var result = await _userManager.UpdateAsync(user);

            var response = new OperationResult<string>();

            // Logging and handling the result of the update operation
            if (result.Succeeded)
            {
                logger.LogInformation("User info updated successfully for username: {username}", this.LoggedInUserName);
                response.Data = "User updated successfully.";
                return Ok(response);
            }
            else
            {
                logger.LogError("Failed to update user info for username: {username}. Error: {error}", this.LoggedInUserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("info/customer")]
        [Authorize]
        public async Task<ActionResult<CustomerBasicDetailsDto>> GetCustomerBasicDetails()
        {
            var customerDetails = await profileService.GetCustomerBasicDetailsAsync(this.LoggedInUserName);

            // Logging and handling the case where customer details are not found
            if (customerDetails == null)
            {
                logger.LogError("Customer details not found for username: {username}", this.LoggedInUserName);
                return NotFound();
            }

            // Logging success
            logger.LogInformation("Customer basic details retrieved successfully for username: {username}", this.LoggedInUserName);

            return Ok(customerDetails);
        }

        [HttpGet("info/customer/{phoneOrUsername}")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<List<CustomerDetails>>>> GetPersonDetails(string phoneOrUsername)
        {
            var response = new OperationResult<List<CustomerDetails>>();

            // Attempting to get person details
            response.Data = await this.profileService.GetCustomerDetails(phoneOrUsername);

            // Logging success
            logger.LogInformation("Person details retrieved successfully for PhoneOrUserName: {phoneOrUserName}", phoneOrUsername);

            return response;
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<OperationResult<string>>> ChangePassword(ChangePasswordParams changePasswordParams)
        {
            var response = new OperationResult<string>();

            // Extracting UserID and UserName from the current user's claims
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;

            // Attempting to get the user from the database
            var user = await GetUser(UserName);

            // Logging if user is not found
            if (user == null)
            {
                logger.LogError("User not found for username: {username}", UserName);
                return NotFound("User not found");
            }

            // Attempting to change the password
            var result = await _userManager.ChangePasswordAsync(user, changePasswordParams.CurrentPassword, changePasswordParams.NewPassword);

            // Logging and handling the result of the password change operation
            if (!result.Succeeded)
            {
                logger.LogError("Failed to change password for username: {username}. Error: {error}", UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                return StatusCode(500, "Failed to change password");
            }

            // Logging success and setting the response data
            logger.LogInformation("Password changed successfully for username: {username}", UserName);
            response.Data = "Password changed successfully";

            return Ok(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<string>>> ResetPassword(ResetPasswordParams resetPassword)
        {
            var response = new OperationResult<string>();

            // Checking if the user exists
            if (await UserExists(resetPassword.Email))
            {
                // Attempting to reset the password
                response.Data = await this._tokenService.ResetPassword(resetPassword);
            }
            else
            {
                // Logging and handling the case where the user is not found
                logger.LogError("Invalid User Name: {email}", resetPassword.Email);
                response.Status = "ERROR";
                response.Data = "Invalid User Name";
            }
            return response;
        }

        [HttpGet("check-reset-password-link")]
        [AllowAnonymous]
        public ActionResult<OperationResult<CheckResetLinkResult>> CheckResetPasswordLink(string queryParam)
        {
            var response = new OperationResult<CheckResetLinkResult>();

            // Attempting to check the reset password link
            var result = _tokenService.CheckResetPasswordLink(queryParam);

            // Logging and setting the response data
            logger.LogInformation("Reset password link checked for queryParam: {queryParam}", queryParam);
            response.Data = result;

            return Ok(response);
        }

        [HttpPost("set-new-password")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<string>>> SetNewPassword(SetNewPasswordParams setNewPasswordParams)
        {
            var response = new OperationResult<string>();

            // Attempting to set a new password
            response.Data = await this._tokenService.SetNewPassword(setNewPasswordParams);

            // Logging success
            logger.LogInformation("New password set successfully for params: {setNewPasswordParams}", setNewPasswordParams);

            return response;
        }
        
     
       
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower() && x.IsActive == true);
        }

        private async Task<AppUser> GetUser(string username)
        {
            return await _userManager.Users
             .SingleOrDefaultAsync(x => x.UserName == username.ToLower() && x.IsActive == true);
        }


    }
}
