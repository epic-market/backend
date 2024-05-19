using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EpicMarket.Data.Common
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
  

			if (roleManager.Roles.Where(c => c.Name == "businessOwner").ToList().Count == 0) 
            {
                var newrole = new AppRole { Name = "businessOwner" };
				await roleManager.CreateAsync(newrole);
			}

			if (roleManager.Roles.Where(c => c.Name == "businessEmployee").ToList().Count == 0)
			{
				var newrole = new AppRole { Name = "businessEmployee" };
				await roleManager.CreateAsync(newrole);
			}

			if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "member"},
                new AppRole{Name = "admin"},
                new AppRole{Name = "moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin@epicmarket.in"
            };

            await userManager.CreateAsync(admin, "Epicmarket@2024");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });

        }


        public static async Task Seeddata(ApplicationDbContext context)
        {
            if (await context.AccessTypes.AnyAsync()) return;

            var accessTypes = new List<AccessType>
            {
              new AccessType{ Name = "ReadOnly" , Priority = 2},
              new AccessType{ Name = "ReadWrite" , Priority = 1},
              new AccessType{ Name = "Denied" , Priority = 3}
            };

            foreach (var accessType in accessTypes)
            {
                await context.AccessTypes.AddAsync(accessType);
            }

            await context.SaveChangesAsync();

        }
    }
}
