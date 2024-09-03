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
    public class HelpItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HelpItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HelpItems
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.HelpItems.Include(h => h.Pages);
            return View(await authDbContext.ToListAsync());
        }

        // GET: HelpItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems
                .Include(h => h.Pages)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (helpItem == null)
            {
                return NotFound();
            }

            return View(helpItem);
        }

        // GET: HelpItems/Create
        public IActionResult Create()
        {
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "Name");
            return View();
        }

        // POST: HelpItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Title,Description,PageID,IsShownOnPage,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] HelpItem helpItem)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            helpItem.CreateBy = userName;
            helpItem.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(helpItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "Name", helpItem.PageID);
            return View(helpItem);
        }

        // GET: HelpItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems.FindAsync(id);
            if (helpItem == null)
            {
                return NotFound();
            }
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "Name", helpItem.PageID);
            return View(helpItem);
        }

        // POST: HelpItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Title,Description,PageID,IsShownOnPage,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] HelpItem helpItem)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            helpItem.ModifiedBy = userName;
            helpItem.ModifiedDate = DateTime.UtcNow;
            if (id != helpItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(helpItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HelpItemExists(helpItem.ID))
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
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "Name", helpItem.PageID);
            return View(helpItem);
        }

        // GET: HelpItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems
                .Include(h => h.Pages)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (helpItem == null)
            {
                return NotFound();
            }

            return View(helpItem);
        }

        // POST: HelpItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var helpItem = await _context.HelpItems.FindAsync(id);
            if (helpItem != null)
            {
                _context.HelpItems.Remove(helpItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HelpItemExists(int id)
        {
            return _context.HelpItems.Any(e => e.ID == id);
        }
    }
}
