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
            await userManager.AddToRolesAsync(admin, new[] { "admin" });

        }


        public static async Task Seeddata(ApplicationDbContext context)
        {
            if (!await context.AccessTypes.AnyAsync()) {

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
            }

            if (!await context.StatusOptionSets.AnyAsync())
            {
                var statusOptions = new List<StatusOptionSet>
                {
                    new StatusOptionSet{ Status = "Unverified" , StatusDescription = "Not yet been verified"},
                    new StatusOptionSet{ Status = "Pending" , StatusDescription = "verification is in progress"},
                    new StatusOptionSet{ Status = "Verified" , StatusDescription = "successfully verified"},
                    new StatusOptionSet{ Status = "Rejected" , StatusDescription = "verification was unsuccessful"},
                };

                foreach (var statusOption in statusOptions)
                {
                    await context.StatusOptionSets.AddAsync(statusOption);
                }
            }

            if (!await context.OrderStatusOptions.AnyAsync())
            {
                var orderOptions = new List<OrderStatusOptions>
                {
                    new OrderStatusOptions{ OrderStatus = "Order Placed" },
                    new OrderStatusOptions{ OrderStatus = "Order Confirmed"},
                    new OrderStatusOptions{ OrderStatus = "Order Processing" },
                    new OrderStatusOptions{ OrderStatus = "Canceled"},
                    new OrderStatusOptions{ OrderStatus = "Awaiting Pickup" },
                    new OrderStatusOptions{ OrderStatus = "Delivered"},
                };

                foreach (var orderOption in orderOptions)
                {
                    await context.OrderStatusOptions.AddAsync(orderOption);
                }
            }

            if (!await context.SupportTicketTypes.AnyAsync())
            {
                var supportTickets = new List<SupportTicketType>
                {
                    new SupportTicketType{ Name = "New"  , Description = "Newly Created ticket"},
                    new SupportTicketType{ Name = "Pending",  Description = "requires additional information or action"},
                    new SupportTicketType{ Name = "On Hold",  Description = "cannot be resolved immediately due to external dependencies"},
                    new SupportTicketType{ Name = "Resolved",  Description = "ticket is not yet closed"},
                    new SupportTicketType{ Name = "Closed",  Description = "it has been closed"},

                };

                foreach (var tickettype in supportTickets)
                {
                    await context.SupportTicketTypes.AddAsync(tickettype);
                }
            }
            if (!await context.TaskStatusTypes.AnyAsync())
            {
                var taskStatuses = new List<TaskStatusType>
                {
                    new TaskStatusType{ Status = "New"  , StatusDescription = "Newly Created ticket"},
                    new TaskStatusType{ Status = "Pending",  StatusDescription = "requires additional information or action"},
                    new TaskStatusType{ Status = "On Hold",  StatusDescription = "cannot be resolved immediately due to external dependencies"},
                    new TaskStatusType{ Status = "Resolved",  StatusDescription = "ticket is not yet closed"},
                    new TaskStatusType{ Status = "Closed",  StatusDescription = "it has been closed"},

                };

                foreach (var tickettype in taskStatuses)
                {
                    await context.TaskStatusTypes.AddAsync(tickettype);
                }
            }

            await context.SaveChangesAsync();

        }
    }
}
