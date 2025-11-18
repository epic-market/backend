using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EpicMarket.Entities.Constants;

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
            
            // Special case: If the user is in the ROOT role, they have access to everything
            var rootRoleId = _context.Roles
                .Where(r => r.Name == ROLES.ROOT)
                .Select(r => r.Id)
                .FirstOrDefault();
                
            if (roleIds.Contains(rootRoleId))
                return true;
            
            // Get the securable ID - modified to use case-insensitive comparison that EF Core can translate
            var securableId = _context.ApplicationSecurables
                .Where(s => s.Name.ToLower() == securable.ToLower())
                .Select(s => s.Id)
                .FirstOrDefault();
                
            if (securableId == 0)
                return false;
                
            // Get the denied access type ID
            var deniedAccessTypeId = _context.AccessTypes
                .Where(at => at.Name.ToLower() == "denied")
                .Select(at => at.Id)
                .FirstOrDefault();
                
            // Check if any of the user's roles have permission for this securable
            // A role has permission if there's an ACL entry with an access type other than "Denied"
            var hasAccess = _context.AccessControlLists
                .Any(acl => 
                    roleIds.Contains(acl.RoleID) && 
                    acl.SecurableID == securableId && 
                    acl.AccessTypeID != deniedAccessTypeId);
                
            return hasAccess;
        }

        public async Task<IEnumerable<string>> GetUserPermissions(string username)
        {
            // Get the user with their roles
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Roles)
                .FirstOrDefaultAsync(u => u.UserName == username);
                
            if (user == null)
                return new List<string>();
                
            // Get the roles for this user
            var roleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            
            // Special case: If the user is in the ROOT role, they have access to all securables
            var rootRoleId = await _context.Roles
                .Where(r => r.Name == ROLES.ROOT)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
                
            if (roleIds.Contains(rootRoleId))
            {
                // Return all securables for ROOT users
                var allSecurables = await _context.ApplicationSecurables
                    .Select(s => s.Name)
                    .ToListAsync();
                    
                return allSecurables;
            }
            
            // Get the denied access type ID
            var deniedAccessTypeId = await _context.AccessTypes
                .Where(at => at.Name.ToLower() == "denied")
                .Select(at => at.Id)
                .FirstOrDefaultAsync();
                
            // Get all securables that the user has access to
            // A user has access to a securable if there's an ACL entry with an access type other than "Denied"
            var securables = await _context.AccessControlLists
                .Where(acl => 
                    roleIds.Contains(acl.RoleID) && 
                    acl.AccessTypeID != deniedAccessTypeId)
                .Select(acl => acl.Securable.Name)
                .Distinct()
                .ToListAsync();
                
            return securables;
        }
    }
} 