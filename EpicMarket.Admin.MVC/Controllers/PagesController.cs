using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pages
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Pages.Include(p => p.ApplicationsTable);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .Include(p => p.ApplicationsTable)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name");
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,ApplicationId,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Page page)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            page.CreateBy = userName;
            page.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name", page.ApplicationId);
            return View(page);
        }

        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name", page.ApplicationId);
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,ApplicationId,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Page page)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            page.ModifiedBy = userName;
            page.ModifiedDate = DateTime.UtcNow;
            if (id != page.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.ID))
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
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name", page.ApplicationId);
            return View(page);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .Include(p => p.ApplicationsTable)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                _context.Pages.Remove(page);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.ID == id);
        }
    }
}
