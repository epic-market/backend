using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public AppUsersController(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: AppUsers
        [SecurableAuthorize(SecurableConstants.AppUsersView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [HttpGet]
        [SecurableAuthorize(SecurableConstants.AppUsersView)]
        public JsonResult GetUsers(string search)
        {
            var users = _context.Users
                .Where(u => u.UserName.Contains(search))
                .Select(u => new { id = u.Id, text = u.UserName })
                .Take(10)
                .ToList();

            return Json(users);
        }

        // GET: AppUsers/Details/5
        [SecurableAuthorize(SecurableConstants.AppUsersView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                       .Include(u => u.UserRoles)
                       .ThenInclude(ur => ur.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }
            var roles = string.Join(", ", appUser.UserRoles.Select(ur => ur.Roles.Name));
            // Pass roles to the view
            ViewData["Roles"] = roles;

            return View(appUser);
        }

        // GET: AppUsers/Create
        [SecurableAuthorize(SecurableConstants.AppUsersAdd)]
        public async Task<IActionResult> Create()
        {
            var roles = await roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles.Select(r => new { Name = r.Name, Selected = false }).ToList();
            return View();
        }

        // POST: AppUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.AppUsersAdd)]
        public async Task<IActionResult> Create(AppUser user, List<string> SelectedRoles)
        {
            if (ModelState.IsValid)
            {
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, SelectedRoles);
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.CreateAppUser,
                        EntityName = EntityConstants.AppUser,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Created user '{user.UserName}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(user),
                        RecordId = user.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var roles = await roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles.Select(r => new { Name = r.Name, Selected = SelectedRoles.Contains(r.Name) }).ToList();
            return View(user);
        }

        // GET: AppUsers/Edit/5
        [SecurableAuthorize(SecurableConstants.AppUsersEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            var roles = await userManager.GetRolesAsync(appUser);
            var allRoles = await roleManager.Roles.ToListAsync();

            var selectedRoles = allRoles.Select(r => new { Name = r.Name, Selected = roles.Contains(r.Name) }).ToList();

            ViewBag.Roles = selectedRoles;

            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.AppUsersEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,UniqueGuid,OTP,LastActive,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser user, List<string> SelectedRoles)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByIdAsync(id.ToString());
                if (existingUser == null)
                {
                    return NotFound();
                }

                var originalUser = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                // Update user properties
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.TwoFactorEnabled = user.TwoFactorEnabled;
                existingUser.LastActive = DateTime.Now;
                existingUser.NormalizedUserName = userManager.NormalizeName(user.UserName);
                existingUser.NormalizedEmail = userManager.NormalizeName(user.Email);
                // Update user roles
                var userRoles = await userManager.GetRolesAsync(existingUser);
                await userManager.RemoveFromRolesAsync(existingUser, userRoles);
                await userManager.AddToRolesAsync(existingUser, SelectedRoles);

                // Update user in database
                var result = await userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    // Create simplified objects for serialization to avoid circular references
                    var simplifiedOriginal = new
                    {
                        Id = originalUser.Id,
                        UserName = originalUser.UserName,
                        Email = originalUser.Email,
                        FirstName = originalUser.FirstName,
                        LastName = originalUser.LastName,
                        PhoneNumber = originalUser.PhoneNumber,
                        TwoFactorEnabled = originalUser.TwoFactorEnabled
                    };
                    
                    var simplifiedUpdated = new
                    {
                        Id = existingUser.Id,
                        UserName = existingUser.UserName,
                        Email = existingUser.Email,
                        FirstName = existingUser.FirstName,
                        LastName = existingUser.LastName,
                        PhoneNumber = existingUser.PhoneNumber,
                        TwoFactorEnabled = existingUser.TwoFactorEnabled
                    };
                    
                    // Configure JSON serializer options to handle circular references
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                        MaxDepth = 32
                    };
                    
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAppUser,
                        EntityName = EntityConstants.AppUser,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated user '{existingUser.UserName}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = simplifiedOriginal,
                            Updated = simplifiedUpdated,
                            Roles = SelectedRoles
                        }, options),
                        RecordId = existingUser.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            var roles = await roleManager.Roles.ToListAsync();
            var selectedRoles = roles.Select(r => new { Name = r.Name, Selected = SelectedRoles.Contains(r.Name) }).ToList();
            ViewBag.Roles = selectedRoles;

            return View(user);
        }

        // GET: AppUsers/Delete/5
        [SecurableAuthorize(SecurableConstants.AppUsersDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.AppUsersDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appUser = await _context.Users.FindAsync(id);
            if (appUser != null)
            {
                _context.Users.Remove(appUser);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteAppUser,
                    EntityName = EntityConstants.AppUser,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted user '{appUser.UserName}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(appUser),
                    RecordId = appUser.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
