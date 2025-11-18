using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IApplicationConfigurationService applicationConfiguration;
        private readonly ICommunicationService communicationService;
        private readonly IConfiguration _configuration; // Added IConfiguration field

        public TokenService(IConfiguration config,
                            UserManager<AppUser> userManager,
                            ApplicationDbContext dbContext, 
                            IApplicationConfigurationService applicationConfiguration,
                            ICommunicationService communicationService)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            this.applicationConfiguration = applicationConfiguration;
            this.communicationService = communicationService;
            _configuration = config; // Initialize IConfiguration
        }

        public async Task<string> CreateToken(AppUser user)
        {
            // Check for null user
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> ResetPassword(ResetPasswordParams resetPassword)
        {
            try
            {
                // Check for null or empty email
                if (string.IsNullOrEmpty(resetPassword.Email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(resetPassword.Email));
                }

                var User = dbContext.Users.Where(u => resetPassword.Email == u.UserName).FirstOrDefault();
                if (User == null)
                {
                    throw new Exception("User not found");
                }

                var UniqueGuid = Guid.NewGuid();
                User.UniqueGuid = UniqueGuid.ToString();
                dbContext.Users.Update(User);

                var intialURL = applicationConfiguration.GetApplicationConfigurationValue("BusinessOwnerBaseURL");
                string queryToken = intialURL + resetPassword.Path + "/" + User.Id + "." + UniqueGuid;
                //await this.communicationQueueService.InsertCommunicationQueue(
                //    new Entities.CommunicationQueueDTO()
                //    {
                //        MessageData = queryToken,
                //        Subject = "Password Reset",
                //        NotificationRecipient = resetPassword.Email,
                //        ContactMethod = ContactMethodConstants.EMAIL,
                //        CreateBy = resetPassword.Email
                //    });

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EmailTemplates");
                var emailModel = EmailModel.GetResetPasswordModel(queryToken);

                await communicationService.SendTemplatedEmailAsync(
                    resetPassword.Email,
                    EmailSubjectConstants.ResetPasswordLink,
                    EmailTemplateConstants.ResetPasswordLink,
                    emailModel
                );

                return "Reset Link sent to registered Email";
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.Error.WriteLine("Error resetting password: " + ex.Message);
                return ex.Message;
            }
        }
        public async Task<string> SetNewPassword(SetNewPasswordParams setNewPasswordParams)
        {

            // Check for null or empty token
            if (string.IsNullOrEmpty(setNewPasswordParams.token))
            {
                throw new ArgumentException("Token cannot be null or empty", nameof(setNewPasswordParams.token));
            }

            string[] tokenParts = setNewPasswordParams.token.Split('.');
            string uniqueGuid = tokenParts[1];
            var User = dbContext.Users.Where(u => uniqueGuid == u.UniqueGuid).FirstOrDefault();
            if (User != null)
            {
                var result = await _userManager.RemovePasswordAsync(User);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed try again after some time");
                }
                else
                {
                    result = await _userManager.AddPasswordAsync(User, setNewPasswordParams.password);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed try again after some time");
                    }
                    else
                    {
                        var UniqueGuid = Guid.NewGuid();
                        User.UniqueGuid = null;
                        dbContext.Users.Update(User);
                        return "Successfull";
                    }
                }
            }
            else
            {
                throw new Exception("Something went wrong try again after some time");
            }
        }
        public CheckResetLinkResult CheckResetPasswordLink(string queryParam)
        {
            // Check for null or empty queryParam
            if (string.IsNullOrEmpty(queryParam))
            {
                throw new ArgumentException("Query parameter cannot be null or empty", nameof(queryParam));
            }

            string[] tokenParts = queryParam.Split('.');

            string userId = tokenParts[0];
            string uniqueGuid = tokenParts[1];
            int UserID;
            int.TryParse(userId, out UserID);
            var User = dbContext.Users.Find(UserID);

            if (User != null && User.UniqueGuid == uniqueGuid)
            {
                var result = new CheckResetLinkResult()
                {

                    UserID = userId,
                    FirstName = User.FirstName,
                    Email = User.Email,
                };

                return result;
            }
            else
            {
                throw new Exception("Link is Not correct");
            }
        }

    }
}
