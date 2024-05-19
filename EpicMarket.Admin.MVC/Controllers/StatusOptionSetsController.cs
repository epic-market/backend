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
    public class StatusOptionSetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusOptionSetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatusOptionSets
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatusOptionSets.ToListAsync());
        }

        // GET: StatusOptionSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOptionSet = await _context.StatusOptionSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusOptionSet == null)
            {
                return NotFound();
            }

            return View(statusOptionSet);
        }

        // GET: StatusOptionSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusOptionSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,StatusDescription")] StatusOptionSet statusOptionSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusOptionSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusOptionSet);
        }

        // GET: StatusOptionSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOptionSet = await _context.StatusOptionSets.FindAsync(id);
            if (statusOptionSet == null)
            {
                return NotFound();
            }
            return View(statusOptionSet);
        }

        // POST: StatusOptionSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,StatusDescription")] StatusOptionSet statusOptionSet)
        {
            if (id != statusOptionSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusOptionSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusOptionSetExists(statusOptionSet.Id))
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
            return View(statusOptionSet);
        }

        // GET: StatusOptionSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOptionSet = await _context.StatusOptionSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusOptionSet == null)
            {
                return NotFound();
            }

            return View(statusOptionSet);
        }

        // POST: StatusOptionSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusOptionSet = await _context.StatusOptionSets.FindAsync(id);
            if (statusOptionSet != null)
            {
                _context.StatusOptionSets.Remove(statusOptionSet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusOptionSetExists(int id)
        {
            return _context.StatusOptionSets.Any(e => e.Id == id);
        }
    }
}
