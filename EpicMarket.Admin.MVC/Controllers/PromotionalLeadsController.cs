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

namespace EpicMarket.Admin.MVC.Controllers
{
    public class PromotionalLeadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PromotionalLeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PromotionalLeads
        public async Task<IActionResult> Index()
        {
            return View(await _context.PromotionalLeads.ToListAsync());
        }

        // GET: PromotionalLeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionalLeads = await _context.PromotionalLeads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotionalLeads == null)
            {
                return NotFound();
            }

            return View(promotionalLeads);
        }

        // GET: PromotionalLeads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PromotionalLeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Gmail,CreateDate,Time,WhichApplication")] PromotionalLeads promotionalLeads)
        {
            promotionalLeads.CreateDate = DateTime.UtcNow;
            promotionalLeads.Time = DateTime.Now.TimeOfDay;

            if (ModelState.IsValid)
            {
                _context.Add(promotionalLeads);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promotionalLeads);
        }

        // GET: PromotionalLeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionalLeads = await _context.PromotionalLeads.FindAsync(id);
            if (promotionalLeads == null)
            {
                return NotFound();
            }
            return View(promotionalLeads);
        }

        // POST: PromotionalLeads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Gmail,CreateDate,Time,WhichApplication")] PromotionalLeads promotionalLeads)
        {
            if (id != promotionalLeads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotionalLeads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionalLeadsExists(promotionalLeads.Id))
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
            return View(promotionalLeads);
        }

        // GET: PromotionalLeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionalLeads = await _context.PromotionalLeads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotionalLeads == null)
            {
                return NotFound();
            }

            return View(promotionalLeads);
        }

        // POST: PromotionalLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotionalLeads = await _context.PromotionalLeads.FindAsync(id);
            if (promotionalLeads != null)
            {
                _context.PromotionalLeads.Remove(promotionalLeads);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionalLeadsExists(int id)
        {
            return _context.PromotionalLeads.Any(e => e.Id == id);
        }
    }
}
