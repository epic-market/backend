using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppUserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppUserRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppUserRoles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserRoles.Include(a => a.Roles).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AppUserRoles/Details/5
        public async Task<IActionResult> Details(int userid, int roleid)
        {
            if (userid == null )
            {
                return NotFound();
            }

            var appUserRole = await _context.UserRoles
                .Include(a => a.Roles)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == userid && m.RoleId == roleid);
            if (appUserRole == null)
            {
                return NotFound();
            }

            return View(appUserRole);
        }

        // GET: AppUserRoles/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: AppUserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] AppUserRole appUserRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUserRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", appUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", appUserRole.UserId);
            return View(appUserRole);
        }

        // GET: AppUserRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUserRole = await _context.UserRoles.FindAsync(id);
            if (appUserRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", appUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", appUserRole.UserId);
            return View(appUserRole);
        }

        // POST: AppUserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RoleId")] AppUserRole appUserRole)
        {
            if (id != appUserRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUserRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserRoleExists(appUserRole.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", appUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", appUserRole.UserId);
            return View(appUserRole);
        }

        // GET: AppUserRoles/Delete/5
        public async Task<IActionResult> Delete(int userid, int roleid)
        {
            if (userid == null)
            {
                return NotFound();
            }

            var appUserRole = await _context.UserRoles
                .Include(a => a.Roles)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == userid && m.RoleId == roleid);
            if (appUserRole == null)
            {
                return NotFound();
            }

            return View(appUserRole);
        }

        // POST: AppUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userid, int roleid)
        {
            var appUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.UserId == userid && m.RoleId == roleid); ;

            if (appUserRole != null)
            {
                _context.UserRoles.Remove(appUserRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserRoleExists(int id)
        {
            return _context.UserRoles.Any(e => e.UserId == id);
        }
    }
}
