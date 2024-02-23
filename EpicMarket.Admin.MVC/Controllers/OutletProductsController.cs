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
    public class OutletProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OutletProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OutletProducts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OutletProducts.Include(o => o.Outlet).Include(o => o.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OutletProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletProduct = await _context.OutletProducts
                .Include(o => o.Outlet)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outletProduct == null)
            {
                return NotFound();
            }

            return View(outletProduct);
        }

        // GET: OutletProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Outlets, "ID", "ID");
            ViewData["ProductID"] = new SelectList(_context.Catalogs, "ID", "ID");
            return View();
        }

        // POST: OutletProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OutletID,ProductID")] OutletProduct outletProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outletProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Outlets, "ID", "ID", outletProduct.ProductID);
            ViewData["ProductID"] = new SelectList(_context.Catalogs, "ID", "ID", outletProduct.ProductID);
            return View(outletProduct);
        }

        // GET: OutletProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletProduct = await _context.OutletProducts.FindAsync(id);
            if (outletProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Outlets, "ID", "ID", outletProduct.ProductID);
            ViewData["ProductID"] = new SelectList(_context.Catalogs, "ID", "ID", outletProduct.ProductID);
            return View(outletProduct);
        }

        // POST: OutletProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OutletID,ProductID")] OutletProduct outletProduct)
        {
            if (id != outletProduct.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outletProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutletProductExists(outletProduct.ID))
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
            ViewData["ProductID"] = new SelectList(_context.Outlets, "ID", "ID", outletProduct.ProductID);
            ViewData["ProductID"] = new SelectList(_context.Catalogs, "ID", "ID", outletProduct.ProductID);
            return View(outletProduct);
        }

        // GET: OutletProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletProduct = await _context.OutletProducts
                .Include(o => o.Outlet)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outletProduct == null)
            {
                return NotFound();
            }

            return View(outletProduct);
        }

        // POST: OutletProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outletProduct = await _context.OutletProducts.FindAsync(id);
            if (outletProduct != null)
            {
                _context.OutletProducts.Remove(outletProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutletProductExists(int id)
        {
            return _context.OutletProducts.Any(e => e.ID == id);
        }
    }
}
