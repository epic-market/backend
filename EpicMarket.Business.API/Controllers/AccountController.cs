using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities.Entities;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EpicMarket.Services;
using Newtonsoft.Json;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Manages user account operations including registration, authentication, and password management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> logger;
        private readonly ICommunicationService communication;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IProfileService profileService;
        public AccountController(
                                    UserManager<AppUser> userManager,
                                    SignInManager<AppUser> signInManager,
                                    ITokenService tokenService,
                                    IMapper mapper,
                                    ILogger<AccountController> logger,
                                    ICommunicationService communication,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IProfileService profileService
                                    ) : base(dbContext, httpContextAccessor)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            this.logger = logger;
            this.communication = communication;
            _tokenService = tokenService;
            this.profileService = profileService;
        }

        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="registerDto">Registration information including email, password, and phone</param>
        /// <returns>Authentication token upon successful registration</returns>
        /// <response code="200">Returns authentication token for the newly registered user</response>
        /// <response code="400">If username is already taken or validation fails</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/account/register
        ///     {
        ///        "email": "user@example.com",
        ///        "password": "SecurePassword123!",
        ///        "phone": "+1234567890",
        ///        "firstName": "John",
        ///        "lastName": "Doe"
        ///     }
        /// </remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<TokenDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<OperationResult<TokenDto>>> Register(RegisterDto registerDto)
        {
            var response = new OperationResult<TokenDto>();

            if (await UserExists(registerDto.Email)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Email.ToLower();
            user.Email = registerDto.Email.ToLower();
            user.PhoneNumber = registerDto.Phone;


            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, ROLES.MEMBER);

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

			response.Data =  new TokenDto
            {
                Token = await _tokenService.CreateToken(user)
            };

            return response;
        }

        /// <summary>
        /// Authenticates a user and returns an access token
        /// </summary>
        /// <param name="loginDto">Login credentials containing email and password</param>
        /// <returns>Authentication token upon successful login</returns>
        /// <response code="200">Returns authentication token</response>
        /// <response code="401">Invalid username or password</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/account/login
        ///     {
        ///        "email": "user@example.com",
        ///        "password": "SecurePassword123!"
        ///     }
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<TokenDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OperationResult<TokenDto>>> Login(LoginDto loginDto)
        {

			var response = new OperationResult<TokenDto>();

            var user = await GetUser(loginDto.Email);

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized() ;

            response.Data = new TokenDto
            {
                Token = await _tokenService.CreateToken(user)
            };

            return response;
        }
        /// <summary>
        /// Retrieves current user information including roles, permissions, and business details
        /// </summary>
        /// <returns>User details, business information, and access permissions</returns>
        /// <response code="200">Returns user information with roles and permissions</response>
        /// <response code="401">User is not authenticated</response>
        /// <remarks>
        /// Requires authentication. Returns different business information based on user role:
        /// - Business Owner: Returns owned business details
        /// - Business Employee: Returns associated business details
        /// - Member: Returns basic user information
        /// </remarks>
        [HttpGet("info")]
        [Authorize]
        [ProducesResponseType(typeof(OperationResult<LoginResult>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OperationResult<LoginResult>>> Info()
        {

            var response = new OperationResult<LoginResult>();
            var user = await GetUser(this.LoggedInUserName); ;

            if (user == null) return Unauthorized("Invalid username");

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

            return response;
        }

        /// <summary>
        /// Changes the password for the authenticated user
        /// </summary>
        /// <param name="changePasswordParams">Current and new password information</param>
        /// <returns>Success message upon successful password change</returns>
        /// <response code="200">Password successfully changed</response>
        /// <response code="401">Current password is incorrect or user is not authenticated</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/account/changepassword
        ///     {
        ///        "currentPassword": "OldPassword123!",
        ///        "newPassword": "NewSecurePassword456!",
        ///        "confirmPassword": "NewSecurePassword456!"
        ///     }
        /// </remarks>
        [HttpPost("changepassword")]
        [Authorize]
        [ProducesResponseType(typeof(OperationResult<string>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OperationResult<string>>> changepassword(ChangePasswordParams changePasswordParams)
        {

            var response = new OperationResult<string>();

            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;


            var user = await GetUser(UserName);

			var result = await _userManager
                .ChangePasswordAsync(user, changePasswordParams.CurrentPassword, changePasswordParams.NewPassword);

            if (!result.Succeeded) return Unauthorized();

            response.Data = "Password changed Succufully";

            return response;
        }


        /// <summary>
        /// Initiates password reset process by sending reset link to user's email
        /// </summary>
        /// <param name="resetPassword">Email address for password reset</param>
        /// <returns>Status message indicating email has been sent</returns>
        /// <response code="200">Reset link sent successfully or user not found (for security)</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/account/ResetPassword
        ///     {
        ///        "email": "user@example.com"
        ///     }
        /// 
        /// Note: For security reasons, always returns success even if email doesn't exist
        /// </remarks>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<string>), 200)]
        public async Task<ActionResult<OperationResult<string>>> ResetPassword(ResetPasswordParams resetPassword)
        {

            var response = new OperationResult<string>();

            if (await UserExists(resetPassword.Email))
            {
                response.Data = await this._tokenService.ResetPassword(resetPassword);
            }
            else
            {
                response.Status = "ERROR";
                response.Data = "Invalid User Name";
            }
            return response;
        }
        /// <summary>
        /// Validates a password reset link token
        /// </summary>
        /// <param name="queryParam">Encrypted token from password reset email link</param>
        /// <returns>Validation result with token status and user information</returns>
        /// <response code="200">Returns token validation status</response>
        /// <remarks>
        /// This endpoint is typically called when user clicks on password reset link.
        /// The queryParam contains encrypted user information and expiration data.
        /// </remarks>
        [HttpGet("CheckResetPasswordLink")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<CheckResetLinkResult>), 200)]
        public async Task<ActionResult<OperationResult<CheckResetLinkResult>>> CheckResetPasswordLink(string queryParam)
        {
            var response = new OperationResult<CheckResetLinkResult>();


           var result = _tokenService.CheckResetPasswordLink(queryParam);
            
            response.Data = result;

            return Ok(response);
        }
        /// <summary>
        /// Sets a new password using a valid reset token
        /// </summary>
        /// <param name="setNewPasswordParams">Reset token and new password information</param>
        /// <returns>Success message upon successful password reset</returns>
        /// <response code="200">Password successfully reset</response>
        /// <response code="400">Invalid or expired token</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/account/setNewPassword
        ///     {
        ///        "token": "encrypted-reset-token",
        ///        "newPassword": "NewSecurePassword123!",
        ///        "confirmPassword": "NewSecurePassword123!"
        ///     }
        /// </remarks>
        [HttpPost("setNewPassword")]
		[AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<string>), 200)]
        [ProducesResponseType(400)]
		public async Task<ActionResult<OperationResult<string>>> setNewPassword(SetNewPasswordParams setNewPasswordParams)
        {

            var response = new OperationResult<string>();

            
            response.Data = await this._tokenService.setNewPassword(setNewPasswordParams);
            
            return response;
        }

        /// <summary>
        /// Checks if a user with the given username exists in the system
        /// </summary>
        /// <param name="username">Username/email to check</param>
        /// <returns>True if user exists and is active, false otherwise</returns>
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower() && x.IsActive == true);
        }

		/// <summary>
        /// Retrieves a user by username from the database
        /// </summary>
        /// <param name="username">Username/email to search for</param>
        /// <returns>User object if found and active, null otherwise</returns>
        private async Task<AppUser> GetUser(string username)
		{
			return await _userManager.Users
             .SingleOrDefaultAsync(x => x.UserName == username.ToLower() && x.IsActive == true);
		}

	}
}
