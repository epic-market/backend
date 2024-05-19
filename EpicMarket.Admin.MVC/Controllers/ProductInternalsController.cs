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
    public class ProductInternalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductInternalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductInternals
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductInternals.ToListAsync());
        }

        // GET: ProductInternals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInternal = await _context.ProductInternals
                .FirstOrDefaultAsync(m => m.ID == id);
            if (productInternal == null)
            {
                return NotFound();
            }

            return View(productInternal);
        }

        // GET: ProductInternals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductInternals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BarCode,Name,Description,Images")] ProductInternal productInternal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInternal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productInternal);
        }

        // GET: ProductInternals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInternal = await _context.ProductInternals.FindAsync(id);
            if (productInternal == null)
            {
                return NotFound();
            }
            return View(productInternal);
        }

        // POST: ProductInternals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BarCode,Name,Description,Images")] ProductInternal productInternal)
        {
            if (id != productInternal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInternal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInternalExists(productInternal.ID))
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
            return View(productInternal);
        }

        // GET: ProductInternals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInternal = await _context.ProductInternals
                .FirstOrDefaultAsync(m => m.ID == id);
            if (productInternal == null)
            {
                return NotFound();
            }

            return View(productInternal);
        }

        // POST: ProductInternals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInternal = await _context.ProductInternals.FindAsync(id);
            if (productInternal != null)
            {
                _context.ProductInternals.Remove(productInternal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInternalExists(int id)
        {
            return _context.ProductInternals.Any(e => e.ID == id);
        }
    }
}
