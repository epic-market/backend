using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EpicMarket.Admin.MVC.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private string _loggedInUsername;
        private List<AccessControlList> _userPermissions;

        public UserRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public bool HasPermission(string username, string securable)
        {
            _loggedInUsername = username;
            
            // Get the user with their roles
            var user = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Roles)
                .FirstOrDefault(u => u.UserName == username);
                
            if (user == null)
                return false;
                
            // Get the roles for this user
            var roleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            
            // Get the securable ID - modified to use case-insensitive comparison that EF Core can translate
            var securableId = _context.ApplicationSecurables
                .Where(s => s.Name.ToLower() == securable.ToLower())
                .Select(s => s.Id)
                .FirstOrDefault();
                
            if (securableId == 0)
                return false;
                
            // Check if any of the user's roles have permission for this securable
            var hasAccess = _context.AccessControlLists
                .Any(acl => 
                    roleIds.Contains(acl.RoleID) && 
                    acl.SecurableID == securableId && 
                    acl.AccessType.Name == "ReadWrite");
                
            return hasAccess;
        }
    }
} 