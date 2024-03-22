using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NVelocity;
using NVelocity.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IApplicationConfigurationService applicationConfiguration;
        private readonly UserManager<AppUser> userManager;
        private readonly ICommunicationService communicationService;

        public EmployeeService(ApplicationDbContext context, IMapper mapper , IApplicationConfigurationService applicationConfiguration,ICommunicationService communicationService, UserManager<AppUser> userManager)
        {
            _context = context;
            this.mapper = mapper;
            this.applicationConfiguration = applicationConfiguration;
            this.userManager = userManager;
            this.communicationService = communicationService;
        }
        public async Task<AddEmployeeResult> Register(AddEmployeeParam addEmployeeParam)
        {
            var employee = new AddEmployeeResult();

            var UniqueGuid = Guid.NewGuid();

            var user = new AppUser() { FirstName = addEmployeeParam.FirstName , UniqueGuid = UniqueGuid.ToString() };

            if (await UserExists(addEmployeeParam.EmailID)) return BadRequest("Username is taken");

            user.UserName = addEmployeeParam.EmailID.ToLower();

            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault().ToString());

            var roleResult = await userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors.FirstOrDefault().ToString());


            string queryToken = user.Id + "." + UniqueGuid;

            var EmailTemplete = applicationConfiguration.GetApplicationConfigurationValue("BusinessOwnerInvitation");

            var Business = _context.Businesses.Find(addEmployeeParam.BusinessID);

            var User = _context.Users.Find(addEmployeeParam.UserID);

            var _ =  _context.BusinessEmployeeMaps.Add(new BusinessEmployeeMap() {  BussinessID  = addEmployeeParam.BusinessID , EmployeeID = user.Id});
            
            await _context.SaveChangesAsync();
            
            Hashtable ListValues = new Hashtable();

            ListValues.Add("fromAddress", addEmployeeParam.FirstName);
            ListValues.Add("BussinessName", Business.Name);
            ListValues.Add("BusinessOwnerName", User.FirstName);
            ListValues.Add("RedirectUrl", "https://owner.epicmarket.in/registerEmployee/"+ queryToken);

            VelocityEngine v = new VelocityEngine();
            v.Init();

            VelocityContext context = new VelocityContext(ListValues);
            StringWriter writer = new StringWriter();
            v.Evaluate(context, writer, string.Empty, EmailTemplete);
            var message = writer.ToString();

            communicationService.SendEmailAsync(addEmployeeParam.EmailID, "Welcome to Epic Market", message);

            return employee;


        }

        private AddEmployeeResult BadRequest(string v)
        {
            throw new Exception(v);
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        public CheckLinkResult CheckEmployeeLink(string queryParam)
        {
            string[] tokenParts = queryParam.Split('.');

            string userId = tokenParts[0];
            string uniqueGuid = tokenParts[1];
            int UserID;
            int.TryParse(userId, out UserID);
            var User = _context.Users.Find(UserID);

            if (User.UniqueGuid == uniqueGuid)
            {
                var Business = _context.BusinessEmployeeMaps.Where(a => a.EmployeeID == User.Id).Include(a => a.Bussiness).FirstOrDefault();
                var result = new CheckLinkResult()
                {

                    UserID = userId,
                    FirstName = User.FirstName,
                    BusinessName = Business.Bussiness.Name,
                    BusinessEmail = Business.Bussiness.ContactEmail,
                };

                return result;
            }else
            {
                throw new Exception("Link is Not correct");
            }
        }
    }
}
