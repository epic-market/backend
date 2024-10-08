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
    public class ApplicationsTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationsTables
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationsTable.ToListAsync());
        }

        // GET: ApplicationsTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationsTable = await _context.ApplicationsTable
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationsTable == null)
            {
                return NotFound();
            }

            return View(applicationsTable);
        }

        // GET: ApplicationsTables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationsTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Sequence,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationsTable applicationsTable)
        {
			var userName = this.User.FindFirst(ClaimTypes.Name).Value;
			applicationsTable.CreateBy = userName;
			applicationsTable.CreateDate = DateTime.UtcNow;
			if (ModelState.IsValid)
            {
                _context.Add(applicationsTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationsTable);
        }

        // GET: ApplicationsTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationsTable = await _context.ApplicationsTable.FindAsync(id);
            if (applicationsTable == null)
            {
                return NotFound();
            }
            return View(applicationsTable);
        }

        // POST: ApplicationsTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Sequence,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ApplicationsTable applicationsTable)
        {
			var userName = this.User.FindFirst(ClaimTypes.Name).Value;
			applicationsTable.ModifiedBy = userName;
			applicationsTable.ModifiedDate = DateTime.UtcNow;
			if (id != applicationsTable.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationsTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationsTableExists(applicationsTable.ID))
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
            return View(applicationsTable);
        }

        // GET: ApplicationsTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationsTable = await _context.ApplicationsTable
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationsTable == null)
            {
                return NotFound();
            }

            return View(applicationsTable);
        }

        // POST: ApplicationsTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationsTable = await _context.ApplicationsTable.FindAsync(id);
            if (applicationsTable != null)
            {
                _context.ApplicationsTable.Remove(applicationsTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationsTableExists(int id)
        {
            return _context.ApplicationsTable.Any(e => e.ID == id);
        }
    }
}
