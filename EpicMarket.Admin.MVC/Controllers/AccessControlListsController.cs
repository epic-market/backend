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
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using EpicMarket.Admin.MVC.Contracts;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class AccessControlListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;

        public AccessControlListsController(
            ApplicationDbContext context,
            IEventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        [HttpGet]
        [SecurableAuthorize(SecurableConstants.AccessControlListsView)]
        public async Task<IActionResult> Index()
        {
            var roles = _context.Roles.ToList();
            var securables = _context.ApplicationSecurables.ToList();
            var accessTypes = _context.AccessTypes.OrderBy(at => at.Priority).ToList();

            // Assume "Denied" has ID 0
            var deniedAccessTypeId = accessTypes.FirstOrDefault(at => at.Name.ToLower() == "denied")?.Id ?? 0;

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

        [HttpPost]
        [SecurableAuthorize(SecurableConstants.AccessControlListsEdit)]
        public async Task<IActionResult> UpdateAccessType(string securable, string role, int accessTypeId)
        {
            try
            {
                var roleSecurable = _context.AccessControlLists
                    .FirstOrDefault(rs => rs.Securable.Name == securable && rs.Role.Name == role);

                var isNewEntry = roleSecurable == null;
                
                if (isNewEntry)
                {
                    // Create a new entry if it doesn't exist
                    roleSecurable = new AccessControlList
                    {
                        Role = _context.Roles.FirstOrDefault(r => r.Name == role),
                        Securable = _context.ApplicationSecurables.FirstOrDefault(s => s.Name == securable),
                        AccessTypeID = accessTypeId
                    };
                    _context.AccessControlLists.Add(roleSecurable);

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.AddAccessControlList,
                        EntityName = EntityConstants.AccessControlList,
                        Source = "AccessControlListsController",
                        Description = $"Added access control for role '{role}' on securable '{securable}'",
                        Data = $"AccessTypeID: {accessTypeId}",
                        RecordId = roleSecurable.ID,
                        BusinessID = 0, // Set appropriate business ID if available
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
                        Description = $"Updated access control for role '{role}' on securable '{securable}'",
                        Data = $"New AccessTypeID: {accessTypeId}",
                        RecordId = roleSecurable.ID,
                        BusinessID = 0, // Set appropriate business ID if available
                        LoggedInUserName = User.Identity.Name
                    });
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = "An error occurred while updating the access type." });
            }
        }
    }
}
