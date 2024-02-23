using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class ApplicationSecurablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationSecurablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationSecurables
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationSecurables.ToListAsync());
        }

        // GET: ApplicationSecurables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationSecurables = await _context.ApplicationSecurables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationSecurables == null)
            {
                return NotFound();
            }

            return View(applicationSecurables);
        }

        // GET: ApplicationSecurables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationSecurables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationSecurables applicationSecurables)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationSecurables);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationSecurables);
        }

        // GET: ApplicationSecurables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationSecurables = await _context.ApplicationSecurables.FindAsync(id);
            if (applicationSecurables == null)
            {
                return NotFound();
            }
            return View(applicationSecurables);
        }

        // POST: ApplicationSecurables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationSecurables applicationSecurables)
        {
            if (id != applicationSecurables.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationSecurables);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationSecurablesExists(applicationSecurables.Id))
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
            return View(applicationSecurables);
        }

        // GET: ApplicationSecurables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationSecurables = await _context.ApplicationSecurables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationSecurables == null)
            {
                return NotFound();
            }

            return View(applicationSecurables);
        }

        // POST: ApplicationSecurables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationSecurables = await _context.ApplicationSecurables.FindAsync(id);
            if (applicationSecurables != null)
            {
                _context.ApplicationSecurables.Remove(applicationSecurables);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationSecurablesExists(int id)
        {
            return _context.ApplicationSecurables.Any(e => e.Id == id);
        }
    }
}
