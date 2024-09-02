using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext context;

        public ProfileService(ApplicationDbContext _context)
        {
            context = _context;
        }
        public List<AccessControlList_Result> GetAccessControlList(Profile_SearchParams searchParams)
        {
           var roles = context.UserRoles.Include(c=>c.User).Where(c=> c.User.UserName == searchParams.LoggedInUserName).Select(c=> c.RoleId).ToList();
			var accessControlList = context.AccessControlLists
								.Include(a => a.Securable)
								.Include(a => a.AccessType)
								.Where(a => a.AccessType.Name == Constants.ACCESSTYPE_READWRITE && roles.Contains(a.RoleID))
								.Select(a => new
								{
									securable = a.Securable.Name,
									accessType = a.AccessType.Name
								})
								.Distinct()
								.Select(a => new AccessControlList_Result
								{
									SecurableName = a.securable,
									AccessType = a.accessType
								})
								.ToList();

			return accessControlList;
        }

        public async Task<List<CustomerDetails>> GetCustomerDetails(string PhoneOrUserName)
        {
            var query = context.Users
                     .Where(customer => customer.UserName.ToLower().Contains(PhoneOrUserName.ToLower()) ||
                      customer.PhoneNumber.ToString().ToLower().Contains(PhoneOrUserName.ToLower()))
                      .Select(c => new CustomerDetails()
                      {
                          FirstName = c.FirstName,
                          PhoneNumber = c.PhoneNumber,
                          Username = c.UserName,
                      });

            var result = await query.ToListAsync();

            return result;
        }
    }
}
