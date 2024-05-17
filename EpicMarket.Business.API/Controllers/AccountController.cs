using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper, ILogger<AccountController> logger, ICommunicationService communication, ApplicationDbContext dbContext) : base(dbContext)
		{
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            this.logger = logger;
            this.communication = communication;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<OperationResult<UserDto>>> Register(RegisterDto registerDto)
        {
            var response = new OperationResult<UserDto>();

            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();
            user.Email = registerDto.Username.ToLower();
            user.PhoneNumber = registerDto.Phone;


            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

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
        public async Task<ActionResult<OperationResult<LoginResult>>> Login(LoginDto loginDto)
        {

			var response = new OperationResult<LoginResult>();
			//communication.SendEmail("akhil@epicmarket.in", "This is test mail form code", "Its working");

			var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized() ;


            var roles = await _userManager.GetRolesAsync(user);

            var userBusinessDto = new UserBusinessDto();

            if (roles.Contains("businessOwner"))
            {
                userBusinessDto = dbContext.Businesses.Where(c => c.PersonID == user.Id).Include(c => c.Status).Select(c => new UserBusinessDto()
                {
                    businessId = c.ID,
                    businessStatus = c.Status.Status,
                }).FirstOrDefault();
            } else if (roles.Contains("businessEmployee"))
            { 
                userBusinessDto = dbContext.BusinessEmployeeMaps.Where(c => c.EmployeeID == user.Id).Include(c=>c.Bussiness).Include(c => c.Bussiness.Status).Select(c => new UserBusinessDto()
				{
					businessId = c.Bussiness.ID,
					businessStatus = c.Bussiness.Status.Status,
				}).FirstOrDefault();
			}



            response.Data = new LoginResult() { 
                     UserDetails = new UserDto
			         {
				         Username = user.UserName,
				         Token = await _tokenService.CreateToken(user),
				         FirstName = user.FirstName,
				         LastName = user.LastName,
				         Phone = user.PhoneNumber
			         },
                     UserBusinessDto = userBusinessDto
		    };

			return response;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}
