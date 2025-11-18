using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Entities;
using EpicMarket.Admin.MVC.Contracts;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;
using Microsoft.Extensions.Logging;
using EpicMarket.Admin.MVC.Services;

namespace EpicMarket.Admin.MVC.Controllers
{
    // Only ROOT role can access this controller - this is a special case
    // where we want to bypass the normal securable authorization for the root user
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class AccessControlListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly ILogger<AccessControlListsController> _logger;

        public AccessControlListsController(
            ApplicationDbContext context,
            IEventService eventService,
            ILogger<AccessControlListsController> logger)
        {
            _context = context;
            _eventService = eventService;
            _logger = logger;
        }

        // Special case: Root user should always be able to see the Access Control List
        // We're using the Authorize attribute at the controller level instead of SecurableAuthorize
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Check if the user is in the ROOT role
                if (!User.IsInRole(ROLES.ROOT))
                {
                    return View("~/Views/Shared/_AccessDeniedPartial.cshtml", "Access Control Lists");
                }

                var roles = await _context.Roles.ToListAsync();
                var securables = await _context.ApplicationSecurables.ToListAsync();
                var accessTypes = await _context.AccessTypes.OrderBy(at => at.Priority).ToListAsync();

                // Find the "Denied" access type
                var deniedAccessType = accessTypes.FirstOrDefault(at => at.Name.ToLower() == "denied");
                var deniedAccessTypeId = deniedAccessType?.Id ?? 0;

                var viewModel = new SecurityMatrixViewModel
                {
                    RoleNames = roles.Select(r => r.Name).ToList(),
                    Securables = securables.Select(s => new SecurableViewModel
                    {
                        Name = s.Name,
                        RoleAccess = roles.ToDictionary(
                            r => r.Name,
                            r => _context.AccessControlLists
                                .FirstOrDefault(rs => rs.RoleID == r.Id && rs.SecurableID == s.Id)?.AccessTypeID ?? deniedAccessTypeId
                        )
                    }).OrderBy(s => s.Name).ToList(),
                    AccessTypes = accessTypes.Select(at => new AccessTypeViewModel
                    {
                        ID = at.Id,
                        Name = at.Name
                    }).ToList(),
                    DefaultAccessTypeId = deniedAccessTypeId
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading access control list matrix");
                TempData["ErrorMessage"] = "An error occurred while loading the access control list matrix.";
                return View(new SecurityMatrixViewModel());
            }
        }

        // Special case: Root user should always be able to update Access Control Lists
        // We're using the Authorize attribute at the controller level instead of SecurableAuthorize
        [HttpPost]
        public async Task<IActionResult> UpdateAccessType(string securable, string role, int accessTypeId)
        {
            try
            {
                // Check if the user is in the ROOT role
                if (!User.IsInRole(ROLES.ROOT))
                {
                    return Json(new { success = false, message = "Access denied. Only root users can modify access control lists." });
                }

                if (string.IsNullOrEmpty(securable) || string.IsNullOrEmpty(role) || accessTypeId <= 0)
                {
                    return Json(new { success = false, message = "Invalid parameters provided." });
                }

                // Get the role and securable entities
                var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);
                var securableEntity = await _context.ApplicationSecurables.FirstOrDefaultAsync(s => s.Name == securable);
                var accessType = await _context.AccessTypes.FirstOrDefaultAsync(at => at.Id == accessTypeId);

                if (roleEntity == null || securableEntity == null || accessType == null)
                {
                    return Json(new { success = false, message = "Role, securable, or access type not found." });
                }

                // Special case: Prevent root user from denying themselves access to AccessControlLists
                if (role == ROLES.ROOT && 
                    (securable == SecurableConstants.AccessControlListsView || 
                     securable == SecurableConstants.AccessControlListsEdit) && 
                    accessType.Name.ToLower() == "denied")
                {
                    return Json(new { 
                        success = false, 
                        message = "Cannot deny ROOT role access to Access Control Lists as this would prevent management of permissions." 
                    });
                }

                // Find existing access control list entry
                var roleSecurable = await _context.AccessControlLists
                    .FirstOrDefaultAsync(rs => rs.SecurableID == securableEntity.Id && rs.RoleID == roleEntity.Id);

                var isNewEntry = roleSecurable == null;
                
                if (isNewEntry)
                {
                    // Create a new entry if it doesn't exist
                    roleSecurable = new AccessControlList
                    {
                        RoleID = roleEntity.Id,
                        SecurableID = securableEntity.Id,
                        AccessTypeID = accessTypeId
                    };
                    _context.AccessControlLists.Add(roleSecurable);

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.AddAccessControlList,
                        EntityName = EntityConstants.AccessControlList,
                        Source = "AccessControlListsController",
                        Description = $"Added access control for role '{role}' on securable '{securable}' with access type '{accessType.Name}'",
                        Data = $"AccessTypeID: {accessTypeId}",
                        RecordId = roleSecurable.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                else
                {
                    roleSecurable.AccessTypeID = accessTypeId;

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAccessControlList,
                        EntityName = EntityConstants.AccessControlList,
                        Source = "AccessControlListsController",
                        Description = $"Updated access control for role '{role}' on securable '{securable}' to access type '{accessType.Name}'",
                        Data = $"New AccessTypeID: {accessTypeId}",
                        RecordId = roleSecurable.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating access type for securable {Securable} and role {Role}", securable, role);
                return Json(new { success = false, message = "An error occurred while updating the access type." });
            }
        }
    }
}
