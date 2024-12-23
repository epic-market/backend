using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NVelocity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IProfileService profileService;

        public UserRepository(ApplicationDbContext context,
            IProfileService profileService)
        {
            this.context = context;
            this.profileService = profileService;
        }

		public UserRepository(string loggedInUsername, ApplicationDbContext context,
			IProfileService profileService)
		{
	        this.loggedInUsername = loggedInUsername;
			this.context = context;
			this.profileService = profileService;
		}

            
		private List<AccessControlList_Result> permissions;
        private string loggedInUsername;

        public IConfiguration Configuration;

        public List<AccessControlList_Result> Permissions
        {
            get
            {
                if (this.permissions == null)
                {
                    this.permissions = this.profileService.GetAccessControlList(new Profile_SearchParams() { ApplicationName = null, LoggedInUserName = this.loggedInUsername, UserRole = null });
                }

                return this.permissions;
            }
        }
        public string LoggedInUsername
        {
            get
            {
                return this.loggedInUsername;
            }
        }

        public bool IsBusinessVerified(int id)
        {
            var status = context.Businesses.Where(c => c.PersonID == id).Include(c => c.Status).Select(c => c.Status.Status).FirstOrDefault();
            return status == StatusConstants.VERIFIED; 
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public bool HasPermission(string username, string securable)
        {
            this.loggedInUsername = username;
            this.permissions = this.Permissions;
            return this.permissions.Any(x => x.SecurableName.Equals(securable, System.StringComparison.InvariantCultureIgnoreCase) && (x.AccessType == Constants.ACCESSTYPE_READWRITE));
        }
    }
}
