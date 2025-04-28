using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EpicMarket.Admin.MVC.Controllers
{
    // [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public IActionResult Index()
        {
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name");
            ViewData["OutletID"] = new SelectList(_context.Outlets, "ID", "Name");
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Route("Inventories/GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] InventoryFilterViewModel filter)
        {
            try
            {
                var query = _context.Inventory
                    .Include(i => i.Outlet)
                    .Include(i => i.Outlet.Bussiness)
                    .Include(i => i.ProductVariants)
                    .Include(i => i.ProductVariants.Product)
                    .AsQueryable();

                // Apply filters
                if (filter.OutletID.HasValue && filter.OutletID > 0)
                {
                    query = query.Where(i => i.OutletID == filter.OutletID);
                }

                if (filter.BusinessID.HasValue && filter.BusinessID > 0)
                {
                    query = query.Where(i => i.Outlet.BussinessID == filter.BusinessID);
                }

                if (filter.ProductID.HasValue && filter.ProductID > 0)
                {
                    query = query.Where(i => i.ProductVariants.ProductID == filter.ProductID);
                }

                if (filter.ProductVariantID.HasValue && filter.ProductVariantID > 0)
                {
                    query = query.Where(i => i.ProductVariantID == filter.ProductVariantID);
                }

                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                {
                    query = query.Where(i => i.ProductVariants.Product.Name.Contains(filter.ProductName));
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(i => i.ID) : query.OrderByDescending(i => i.ID),
                    "outletname" => filter.SortDirection == "asc" ? query.OrderBy(i => i.Outlet.Name) : query.OrderByDescending(i => i.Outlet.Name),
                    "businessname" => filter.SortDirection == "asc" ? query.OrderBy(i => i.Outlet.Bussiness.Name) : query.OrderByDescending(i => i.Outlet.Bussiness.Name),
                    "catalogname" => filter.SortDirection == "asc" ? query.OrderBy(i => i.ProductVariants.Product.Name) : query.OrderByDescending(i => i.ProductVariants.Product.Name),
                    "quantityavailable" => filter.SortDirection == "asc" ? query.OrderBy(i => i.QuantityAvailable) : query.OrderByDescending(i => i.QuantityAvailable),
                    _ => query.OrderBy(i => i.ID)
                };

                // Apply pagination and project to DTO
                var inventories = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(i => new InventoryDto
                    {
                        ID = i.ID,
                        OutletID = i.OutletID,
                        OutletName = i.Outlet.Name,
                        BusinessID = i.Outlet.BussinessID,
                        BusinessName = i.Outlet.Bussiness.Name,
                        ProductVariantID = i.ProductVariantID,
                        ProductID = i.ProductVariants.ProductID,
                        ProductName = i.ProductVariants.Product.Name,
                        SKU = i.ProductVariants.SKU,
                        TrackInventory = i.TrackInventory,
                        IsInStock = i.IsInStock,
                        QuantityAvailable = i.QuantityAvailable,
                        MinimumStockLevel = i.MinimumStockLevel,
                        MaximumStockLevel = i.MaximumStockLevel,
                        ReorderPoint = i.ReorderPoint,
                        BackOrders = i.BackOrders
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = inventories });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Outlet)
                .Include(i => i.Outlet.Bussiness)
                .Include(i => i.ProductVariants)
                .Include(i => i.ProductVariants.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name");
            ViewData["OutletID"] = new SelectList(new List<Outlet>(), "ID", "Name");
            ViewData["ProductVariantID"] = new SelectList(new List<CatalogVariants>(), "ID", "SKU");
            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OutletID,ProductVariantID,TrackInventory,IsInStock,QuantityAvailable,MinimumStockLevel,MaximumStockLevel,ReorderPoint,BackOrders")] Inventory inventory)
        {
            // Check if inventory already exists for this outlet and product variant
            var existingInventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.OutletID == inventory.OutletID && i.ProductVariantID == inventory.ProductVariantID);

            if (existingInventory != null)
            {
                ModelState.AddModelError("", "Inventory for this outlet and product variant already exists.");
                
                // Get business ID for the selected outlet
                var outlet = await _context.Outlets.FindAsync(inventory.OutletID);
                int businessId = outlet?.BussinessID ?? 0;
                
                ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", businessId);
                ViewData["OutletID"] = new SelectList(_context.Outlets.Where(o => o.BussinessID == businessId), "ID", "Name", inventory.OutletID);
                
                // Get catalog ID for the selected variant
                var variant = await _context.ProductVariants.FindAsync(inventory.ProductVariantID);
                int catalogId = variant?.ProductID ?? 0;
                
                ViewData["ProductID"] = new SelectList(_context.Products.Where(c => c.BusinessID == businessId), "ID", "Name", catalogId);
                ViewData["ProductVariantID"] = new SelectList(_context.ProductVariants.Where(v => v.ProductID == catalogId), "ID", "SKU", inventory.ProductVariantID);
                
                return View(inventory);
            }

            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // If we get here, something failed, redisplay form
            var outletForBusiness = await _context.Outlets.FindAsync(inventory.OutletID);
            int businessIdForOutlet = outletForBusiness?.BussinessID ?? 0;
            
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", businessIdForOutlet);
            ViewData["OutletID"] = new SelectList(_context.Outlets.Where(o => o.BussinessID == businessIdForOutlet), "ID", "Name", inventory.OutletID);
            
            var variantForProduct = await _context.ProductVariants.FindAsync(inventory.ProductVariantID);
            int catalogIdForVariant = variantForProduct?.ProductID ?? 0;
            var catalogForBusiness = await _context.Products.FindAsync(catalogIdForVariant);
            
            ViewData["ProductID"] = new SelectList(_context.Products.Where(c => c.BusinessID == businessIdForOutlet), "ID", "Name", catalogIdForVariant);
            ViewData["ProductVariantID"] = new SelectList(_context.ProductVariants.Where(v => v.ProductID == catalogIdForVariant), "ID", "SKU", inventory.ProductVariantID);
            
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Outlet)
                .Include(i => i.ProductVariants)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (inventory == null)
            {
                return NotFound();
            }
            
            // Get business ID for the selected outlet
            var outlet = await _context.Outlets.FindAsync(inventory.OutletID);
            int businessId = outlet?.BussinessID ?? 0;
            
            // Get catalog ID for the selected variant
            var variant = await _context.ProductVariants.FindAsync(inventory.ProductVariantID);
            int catalogId = variant?.ProductID ?? 0;
            
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", businessId);
            ViewData["BusinessName"] = _context.Businesses.FirstOrDefault(b => b.ID == businessId)?.Name;
            ViewData["OutletName"] = outlet?.Name;
            ViewData["ProductName"] = _context.Products.FirstOrDefault(c => c.ID == catalogId)?.Name;
            ViewData["VariantSKU"] = variant?.SKU;
            
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OutletID,ProductVariantID,TrackInventory,IsInStock,QuantityAvailable,MinimumStockLevel,MaximumStockLevel,ReorderPoint,BackOrders")] Inventory inventory)
        {
            if (id != inventory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.ID))
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
            
            // If we get here, something failed, redisplay form
            var outlet = await _context.Outlets.FindAsync(inventory.OutletID);
            int businessId = outlet?.BussinessID ?? 0;
            
            var variant = await _context.ProductVariants.FindAsync(inventory.ProductVariantID);
            int catalogId = variant?.ProductID ?? 0;
            
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "Name", businessId);
            ViewData["BusinessName"] = _context.Businesses.FirstOrDefault(b => b.ID == businessId)?.Name;
            ViewData["OutletName"] = outlet?.Name;
            ViewData["ProductName"] = _context.Products.FirstOrDefault(c => c.ID == catalogId)?.Name;
            ViewData["VariantSKU"] = variant?.SKU;
            
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Outlet)
                .Include(i => i.Outlet.Bussiness)
                .Include(i => i.ProductVariants)
                .Include(i => i.ProductVariants.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.ID == id);
        }
        
        // API endpoints for cascading dropdowns
        [HttpGet]
        public async Task<IActionResult> GetOutletsByBusiness(int businessId)
        {
            var outlets = await _context.Outlets
                .Where(o => o.BussinessID == businessId)
                .Select(o => new { o.ID, o.Name })
                .ToListAsync();
                
            return Json(outlets);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProductsByBusiness(int businessId)
        {
            var catalogs = await _context.Products
                .Where(c => c.BusinessID == businessId)
                .Select(c => new { c.ID, c.Name })
                .ToListAsync();
                
            return Json(catalogs);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetVariantsByProduct(int catalogId)
        {
            var variants = await _context.ProductVariants
                .Where(v => v.ProductID == catalogId)
                .Select(v => new { v.ID, Name = v.SKU })
                .ToListAsync();
                
            return Json(variants);
        }
        
        [HttpGet]
        public async Task<IActionResult> CheckInventoryExists(int outletId, int variantId)
        {
            var exists = await _context.Inventory
                .AnyAsync(i => i.OutletID == outletId && i.ProductVariantID == variantId);
                
            return Json(new { exists });
        }
    }
}
