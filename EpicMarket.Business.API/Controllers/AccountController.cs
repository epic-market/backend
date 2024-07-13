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

namespace EpicMarket.Business.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> logger;
        private readonly ICommunicationService communication;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper, ILogger<AccountController> logger, ICommunicationService communication, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            this.logger = logger;
            this.communication = communication;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<UserDto>>> Register(RegisterDto registerDto)
        {
            var response = new OperationResult<UserDto>();

            if (await UserExists(registerDto.Email)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Email.ToLower();
            user.Email = registerDto.Email.ToLower();
            user.PhoneNumber = registerDto.Phone;


            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, ROLES.MEMBER);

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

			response.Data =  new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone  = user.PhoneNumber
            };

            return response;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<TokenDto>>> Login(LoginDto loginDto)
        {

			var response = new OperationResult<TokenDto>();
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Email.ToLower());

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
        [HttpGet("info")]
        [Authorize]
        public async Task<ActionResult<OperationResult<LoginResult>>> Info()
        {

            var response = new OperationResult<LoginResult>();
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == this.LoggedInUserName);

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

            response.Data = new LoginResult()
            {
                UserDetails = new UserLoginDto
                {
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber
                },
                UserBusiness = userBusinessDto
            };

            return response;
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<ActionResult<OperationResult<string>>> changepassword(ChangePasswordParams changePasswordParams)
        {

            var response = new OperationResult<string>();

            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var UserName = this.User.FindFirst(ClaimTypes.Name).Value;


            var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == UserName);
            var result = await _userManager
                .ChangePasswordAsync(user, changePasswordParams.CurrentPassword, changePasswordParams.NewPassword);

            if (!result.Succeeded) return Unauthorized();

            response.Data = "Password changed Succufully";

            return response;
        }


        [HttpPost("ResetPassword")]
        public async Task<ActionResult<OperationResult<string>>> ResetPassword(ResetPasswordParams resetPassword)
        {

            var response = new OperationResult<string>();

            if (await UserExists(resetPassword.UserName))
            {

            }


            //var user = await _userManager.Users
            //.SingleOrDefaultAsync(x => x.UserName == UserName);
            //var result = await _userManager
            //    .ChangePasswordAsync(user, changePasswordParams.CurrentPassword, changePasswordParams.NewPassword);

            //if (!result.Succeeded) return Unauthorized();

            response.Data = "Password changed Succufully";

            return response;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}
