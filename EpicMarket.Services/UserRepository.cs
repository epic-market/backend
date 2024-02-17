using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

    }
}
