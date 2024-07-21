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

namespace EpicMarket.Admin.MVC.Controllers
{
    public class AccessControlListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessControlListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AccessControlLists
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.AccessControlLists.Include(a => a.AccessType).Include(a => a.Role).Include(a => a.Securable);
            return View(await authDbContext.ToListAsync());
        }

        // GET: AccessControlLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessControlList = await _context.AccessControlLists
				.Include(a => a.AccessType)
                .Include(a => a.Role)
                .Include(a => a.Securable)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (accessControlList == null)
            {
                return NotFound();
            }

            return View(accessControlList);
        }

        // GET: AccessControlLists/Create
        public IActionResult Create()
        {
            ViewData["AccessTypeID"] = new SelectList(_context.Set<AccessType>(), "Id", "Name");
            ViewData["RoleID"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["SecurableID"] = new SelectList(_context.Set<ApplicationSecurables>(), "Id", "Name");
            return View();
        }

        // POST: AccessControlLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RoleID,AccessTypeID,SecurableID")] AccessControlList accessControlList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accessControlList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccessTypeID"] = new SelectList(_context.Set<AccessType>(), "Id", "Name", accessControlList.AccessTypeID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "Id", "Name", accessControlList.RoleID);
            ViewData["SecurableID"] = new SelectList(_context.Set<ApplicationSecurables>(), "Id", "Name", accessControlList.SecurableID);
            return View(accessControlList);
        }

        // GET: AccessControlLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessControlList = await _context.AccessControlLists.FindAsync(id);
            if (accessControlList == null)
            {
                return NotFound();
            }
            ViewData["AccessTypeID"] = new SelectList(_context.Set<AccessType>(), "Id", "Name", accessControlList.AccessTypeID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "Id", "Name", accessControlList.RoleID);
            ViewData["SecurableID"] = new SelectList(_context.Set<ApplicationSecurables>(), "Id", "Name", accessControlList.SecurableID);
            return View(accessControlList);
        }

        // POST: AccessControlLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RoleID,AccessTypeID,SecurableID")] AccessControlList accessControlList)
        {
            if (id != accessControlList.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accessControlList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessControlListExists(accessControlList.ID))
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
            ViewData["AccessTypeID"] = new SelectList(_context.Set<AccessType>(), "Id", "Name", accessControlList.AccessTypeID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "Id", "Name", accessControlList.RoleID);
            ViewData["SecurableID"] = new SelectList(_context.Set<ApplicationSecurables>(), "Id", "Name", accessControlList.SecurableID);
            return View(accessControlList);
        }

        // GET: AccessControlLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessControlList = await _context.AccessControlLists
				.Include(a => a.AccessType)
                .Include(a => a.Role)
                .Include(a => a.Securable)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (accessControlList == null)
            {
                return NotFound();
            }

            return View(accessControlList);
        }

        // POST: AccessControlLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessControlList = await _context.AccessControlLists.FindAsync(id);
            if (accessControlList != null)
            {
                _context.AccessControlLists.Remove(accessControlList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessControlListExists(int id)
        {
            return _context.AccessControlLists.Any(e => e.ID == id);
        }
    }
}
