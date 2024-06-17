using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string LoggedInUsername => throw new NotImplementedException();

        public List<AccessControlList> Permissions => throw new NotImplementedException();

        public bool IsBusinessVerified(int id)
        {
            var status = context.Businesses.Where(c => c.PersonID == id).Include(c => c.Status).Select(c => c.Status.Status).FirstOrDefault();
            return status == Business_Status.BUSINESS_VERIFIED; 
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public bool HasPermission(string username, string securable)
        {
            throw new NotImplementedException();
        }
    }
}
