using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class AppRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public AppRolesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: AppRoles
        [SecurableAuthorize(SecurableConstants.AppRolesView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: AppRoles/Details/5
        [SecurableAuthorize(SecurableConstants.AppRolesView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // GET: AppRoles/Create
        [SecurableAuthorize(SecurableConstants.AppRolesAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.AppRolesAdd)]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        {
            appRole.NormalizedName = appRole.Name.ToUpper();
            if (ModelState.IsValid)
            {
                _context.Add(appRole);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddAppRole,
                    EntityName = EntityConstants.AppRole,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added app role '{appRole.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(appRole),
                    RecordId = appRole.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }

        // GET: AppRoles/Edit/5
        [SecurableAuthorize(SecurableConstants.AppRolesEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _context.Roles.FindAsync(id);
            if (appRole == null)
            {
                return NotFound();
            }
            return View(appRole);
        }

        // POST: AppRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.AppRolesEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        {
            appRole.NormalizedName = appRole.Name.ToUpper();
            if (id != appRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.Roles.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    _context.Update(appRole);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAppRole,
                        EntityName = EntityConstants.AppRole,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated app role '{appRole.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = appRole
                        }),
                        RecordId = appRole.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppRoleExists(appRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(appRole);
        }

        // GET: AppRoles/Delete/5
        [SecurableAuthorize(SecurableConstants.AppRolesDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // POST: AppRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.AppRolesDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appRole = await _context.Roles.FindAsync(id);
            if (appRole != null)
            {
                _context.Roles.Remove(appRole);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteAppRole,
                    EntityName = EntityConstants.AppRole,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted app role '{appRole.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(appRole),
                    RecordId = appRole.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppRoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
