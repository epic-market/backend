using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class BusinessCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BusinessCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessCategories.ToListAsync());
        }

        // GET: BusinessCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategory = await _context.BusinessCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategory == null)
            {
                return NotFound();
            }

            return View(businessCategory);
        }

        // GET: BusinessCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusinessCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Type")] BusinessCategory businessCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businessCategory);
        }

        // GET: BusinessCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategory = await _context.BusinessCategories.FindAsync(id);
            if (businessCategory == null)
            {
                return NotFound();
            }
            return View(businessCategory);
        }

        // POST: BusinessCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Type")] BusinessCategory businessCategory)
        {
            if (id != businessCategory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessCategoryExists(businessCategory.ID))
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
            return View(businessCategory);
        }

        // GET: BusinessCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategory = await _context.BusinessCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategory == null)
            {
                return NotFound();
            }

            return View(businessCategory);
        }

        // POST: BusinessCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessCategory = await _context.BusinessCategories.FindAsync(id);
            if (businessCategory != null)
            {
                _context.BusinessCategories.Remove(businessCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessCategoryExists(int id)
        {
            return _context.BusinessCategories.Any(e => e.ID == id);
        }
    }
}
