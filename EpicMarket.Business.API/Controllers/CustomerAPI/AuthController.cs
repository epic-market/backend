using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.Constants;
using EpicMarket.Entities.CustomerAPI;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers.CustomerAPI
{
    /// <summary>
    /// Authentication API for consumer application.
    /// Provides sign in, sign up, and user profile endpoints.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly ICommunicationService _communication;
        private readonly ApplicationDbContext _dbContext;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            ILogger<AuthController> logger,
            ICommunicationService communication,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
            _communication = communication;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Sign in with email and password.
        /// </summary>
        /// <remarks>
        /// Route: POST api/auth/signin
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="request">Sign in credentials</param>
        /// <returns>Authenticated user with token</returns>
        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<AuthUserResponse>>> SignIn([FromBody] SignInRequest request)
        {
            var response = new OperationResult<AuthUserResponse>();

            try
            {
                _logger.LogInformation("AuthController -> SignIn() -> email: {email}", request.Email);

                if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid credentials";
                    response.ErrorDetail = "Email and password are required";
                    return BadRequest(response);
                }

                var user = await _userManager.Users
                    .Include(u => u.UserAddresses)
                        .ThenInclude(ua => ua.Address)
                    .SingleOrDefaultAsync(x => x.UserName == request.Email.ToLower() && x.IsActive);

                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {email}", request.Email);
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid credentials";
                    response.ErrorDetail = "User not found";
                    return Unauthorized(response);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                {
                    _logger.LogError("Password check failed for user: {email}", request.Email);
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid credentials";
                    response.ErrorDetail = "Invalid password";
                    return Unauthorized(response);
                }

                var token = await _tokenService.CreateToken(user);
                
                response.Message = "Login successful";
                response.Data = MapToAuthUserResponse(user, token);

                _logger.LogInformation("User logged in successfully: {email}", request.Email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sign in for email: {email}", request.Email);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred during sign in";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <remarks>
        /// Route: POST api/auth/signup
        /// Auth: AllowAnonymous
        /// </remarks>
        /// <param name="request">Registration details</param>
        /// <returns>Newly created user with token</returns>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<AuthUserResponse>>> SignUp([FromBody] SignUpRequest request)
        {
            var response = new OperationResult<AuthUserResponse>();

            try
            {
                _logger.LogInformation("AuthController -> SignUp() -> email: {email}", request.Email);

                if (request == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Invalid request";
                    response.ErrorDetail = "Registration details are required";
                    return BadRequest(response);
                }

                // Check if user already exists
                var existingUser = await _userManager.Users.AnyAsync(x => x.UserName == request.Email.ToLower());
                if (existingUser)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Email already registered";
                    response.ErrorDetail = "A user with this email already exists";
                    return BadRequest(response);
                }

                var user = new AppUser
                {
                    UserName = request.Email.ToLower(),
                    Email = request.Email.ToLower(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.Phone,
                    IsActive = true,
                    LastActive = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(user, request.Password);

                if (!createResult.Succeeded)
                {
                    _logger.LogError("User creation failed: {@errors}", createResult.Errors);
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Registration failed";
                    response.ErrorDetail = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return BadRequest(response);
                }

                // Add to MEMBER role
                var roleResult = await _userManager.AddToRoleAsync(user, ROLES.MEMBER);
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Failed to add user to role: {@errors}", roleResult.Errors);
                }

                var token = await _tokenService.CreateToken(user);

                response.Message = "Registration successful";
                response.Data = MapToAuthUserResponse(user, token);

                _logger.LogInformation("User registered successfully: {email}", request.Email);
                return CreatedAtAction(nameof(SignUp), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sign up for email: {email}", request.Email);
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred during registration";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Get the currently authenticated user's profile.
        /// </summary>
        /// <remarks>
        /// Route: GET api/auth/me
        /// Auth: Authorize
        /// </remarks>
        /// <returns>Current user profile</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<OperationResult<CurrentUserResponse>>> GetCurrentUser()
        {
            var response = new OperationResult<CurrentUserResponse>();

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "Unauthorized";
                    response.ErrorDetail = "User not authenticated";
                    return Unauthorized(response);
                }

                var user = await _userManager.Users
                    .Include(u => u.UserAddresses)
                        .ThenInclude(ua => ua.Address)
                    .SingleOrDefaultAsync(u => u.Id == userId && u.IsActive);

                if (user == null)
                {
                    response.Status = OperationStatus.ERROR;
                    response.Message = "User not found";
                    return NotFound(response);
                }

                response.Message = "Success";
                response.Data = MapToCurrentUserResponse(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                response.Status = OperationStatus.ERROR;
                response.Message = "An error occurred";
                response.ErrorDetail = ex.Message;
                return StatusCode(500, response);
            }
        }

        #region Private Helper Methods

        private AuthUserResponse MapToAuthUserResponse(AppUser user, string token)
        {
            return new AuthUserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Phone = user.PhoneNumber,
                PhoneVerified = user.PhoneNumberConfirmed,
                EmailVerified = user.EmailConfirmed,
                Token = token,
                ProfileImage = null, // Can be extended to include profile image
                Addresses = MapUserAddresses(user),
                IsActive = user.IsActive,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
        }

        private CurrentUserResponse MapToCurrentUserResponse(AppUser user)
        {
            return new CurrentUserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Phone = user.PhoneNumber,
                PhoneVerified = user.PhoneNumberConfirmed,
                EmailVerified = user.EmailConfirmed,
                ProfileImage = null,
                Addresses = MapUserAddresses(user),
                IsActive = user.IsActive
            };
        }

        private List<UserAddressResponse> MapUserAddresses(AppUser user)
        {
            if (user.UserAddresses == null || !user.UserAddresses.Any())
                return new List<UserAddressResponse>();

            return user.UserAddresses.Select(ua => new UserAddressResponse
            {
                Id = ua.Id,
                UserId = ua.UserId,
                Label = ua.Address?.Address1 ?? "",
                AddressLine1 = ua.Address?.Address1 ?? "",
                AddressLine2 = ua.Address?.Address2 ?? "",
                City = ua.Address?.City ?? "",
                State = ua.Address?.State ?? "",
                Pincode = ua.Address?.Pincode.ToString() ?? "",
                Country = "India",
                Latitude = ua.Address?.Latitude ?? 0,
                Longitude = ua.Address?.Longitude ?? 0,
                IsDefault = false,
                AddressType = "Home",
                IsActive = true
            }).ToList();
        }

        #endregion
    }
}
