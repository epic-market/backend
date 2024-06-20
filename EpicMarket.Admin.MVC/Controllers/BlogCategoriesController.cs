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
    public class BlogCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlogCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogCategory.ToListAsync());
        }

        // GET: BlogCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = await _context.BlogCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogCategory == null)
            {
                return NotFound();
            }

            return View(blogCategory);
        }

        // GET: BlogCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BlogCategory blogCategory)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            blogCategory.CreateBy = userName;
            blogCategory.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(blogCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogCategory);
        }

        // GET: BlogCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = await _context.BlogCategory.FindAsync(id);
            if (blogCategory == null)
            {
                return NotFound();
            }
            return View(blogCategory);
        }

        // POST: BlogCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BlogCategory blogCategory)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            blogCategory.ModifiedBy = userName;
            blogCategory.ModifiedDate = DateTime.UtcNow;
            if (id != blogCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogCategoryExists(blogCategory.Id))
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
            return View(blogCategory);
        }

        // GET: BlogCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = await _context.BlogCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogCategory == null)
            {
                return NotFound();
            }

            return View(blogCategory);
        }

        // POST: BlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogCategory = await _context.BlogCategory.FindAsync(id);
            if (blogCategory != null)
            {
                _context.BlogCategory.Remove(blogCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogCategoryExists(int id)
        {
            return _context.BlogCategory.Any(e => e.Id == id);
        }
    }
}
