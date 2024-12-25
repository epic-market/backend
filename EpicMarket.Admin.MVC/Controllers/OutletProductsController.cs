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
            var applicationDbContext = _context.OutletProducts.Include(o => o.Outlet).Include(o => o.Outlet.Bussiness).Include(o => o.Product);
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
        public IActionResult Create([FromQuery]int? productID)
        {
            if (productID != null)
            {
                var businessid = _context.Catalogs.Where(c => c.ID == productID).FirstOrDefault().BusinessID;
                ViewData["Outlets"] = new SelectList(_context.Outlets.Where(c => c.BussinessID == businessid), "ID", "Name");
                ViewData["Products"] = new SelectList(_context.Catalogs.Where(c => c.ID == productID), "ID", "Name", productID);
            }
            else {
                ViewData["Outlets"] = new SelectList(_context.Outlets, "ID", "Name");
                ViewData["Products"] = new SelectList(_context.Catalogs, "ID", "Name", productID);
            }
		
            ViewBag.productID = productID;
            return View();
        }

        // POST: OutletProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOutletProduct([FromQuery] int? productID ,[Bind("ID,OutletID,ProductID,QuantityAvailable,MinimumStockLevel,MaximumStockLevel,ReorderPoint,BackOrders")] OutletProduct outletProduct)
        {

            outletProduct.ProductID = (int)productID;
            if (ModelState.IsValid)
            {
                _context.Add(outletProduct);
                await _context.SaveChangesAsync();
                if (productID != null)
                {
					return Redirect("/Catalogs/Details/"+productID);
				}
				return RedirectToAction(nameof(Index));

            }
            if (productID != null)
            {
                var businessid = _context.Catalogs.Where(c => c.ID == productID).FirstOrDefault().BusinessID;
                ViewData["Outlets"] = new SelectList(_context.Outlets.Where(c => c.BussinessID == businessid), "ID", "Name",outletProduct.OutletID);
                ViewData["Products"] = new SelectList(_context.Catalogs.Where(c => c.ID == productID), "ID", "Name", productID);
            }
            else
            {
                ViewData["Outlets"] = new SelectList(_context.Outlets, "ID", "Name", outletProduct.OutletID);
                ViewData["Products"] = new SelectList(_context.Catalogs, "ID", "Name", productID);
            }
            return  RedirectToAction(nameof(Create),new { ProductId = productID }) ;
        }

        // GET: OutletProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletProduct = await _context.OutletProducts
            .Include(op => op.Outlet)
            .Include(op => op.Product)
            .FirstOrDefaultAsync(m => m.ID == id);


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
        public async Task<IActionResult> Edit(int id, [Bind("ID,OutletID,ProductID,QuantityAvailable,MinimumStockLevel,MaximumStockLevel,ReorderPoint,BackOrders")] OutletProduct outletProduct)
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
            var outletProduct = await _context.OutletProducts
                               .Include(op => op.Outlet)
                               .Include(op => op.Product)
                               .FirstOrDefaultAsync(m => m.ID == id);
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
