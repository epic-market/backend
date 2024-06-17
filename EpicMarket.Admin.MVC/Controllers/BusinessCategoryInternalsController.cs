using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class BusinessCategoryInternalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessCategoryInternalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BusinessCategoryInternals
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessCategories.ToListAsync());
        }

        // GET: BusinessCategoryInternals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusinessCategoryInternals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Type,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BusinessCategoryInternal businessCategoryInternal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessCategoryInternal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories.FindAsync(id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }
            return View(businessCategoryInternal);
        }

        // POST: BusinessCategoryInternals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Type,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BusinessCategoryInternal businessCategoryInternal)
        {
            if (id != businessCategoryInternal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessCategoryInternal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessCategoryInternalExists(businessCategoryInternal.ID))
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
            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            return View(businessCategoryInternal);
        }

        // POST: BusinessCategoryInternals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessCategoryInternal = await _context.BusinessCategories.FindAsync(id);
            if (businessCategoryInternal != null)
            {
                _context.BusinessCategories.Remove(businessCategoryInternal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessCategoryInternalExists(int id)
        {
            return _context.BusinessCategories.Any(e => e.ID == id);
        }
    }
}
