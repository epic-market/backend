using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Models;
using System.Security.Claims;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OutletsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OutletsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Outlets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Outlets.Include(o => o.Address).Include(o => o.Bussiness);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Outlets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletDetails = new OutletsDetailsModel();


            var outlet = await _context.Outlets
                .Include(o => o.Address)
                .Include(o => o.Bussiness)
                .FirstOrDefaultAsync(m => m.ID == id);

            var orders = await _context.Orders.Where(c => c.OutletID == id).Include(o => o.Address).ToListAsync();
            var outletPersons = await _context.OutletPeople.Where(c => c.OutletId == id).Include(o => o.Person).ToListAsync();
            var outletProducts = await _context.OutletProducts.Where(c => c.OutletID == id).Include(o => o.Product).ToListAsync();
            
            outletDetails.Outlet = outlet;
            outletDetails.Orders = orders;
            outletDetails.OutletProducts = outletProducts;
            outletDetails.OutletEmployees = outletPersons;

            if (outlet == null)
            {
                return NotFound();
            }

            return View(outletDetails);
        }

        // GET: Outlets/Create
        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID");
            return View();
        }

        // POST: Outlets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BussinessID,AddressID,Name,Description,ContactNumber,ContactEmail,Rating,ReviewCount,IsOpen,Weight,Status,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Outlet outlet)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;


            outlet.Address.CreateBy = userName;
            outlet.Address.CreateDate = DateTime.UtcNow;
            outlet.CreateBy = userName;
            outlet.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(outlet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", outlet.AddressID);
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", outlet.BussinessID);
            return View(outlet);
        }

        // GET: Outlets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var outlet = await _context.Outlets.Where(b => b.ID == id).Include(c => c.Address).FirstOrDefaultAsync();
            if (outlet == null)
            {
                return NotFound();
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", outlet.AddressID);
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", outlet.BussinessID);
            return View(outlet);
        }

        // POST: Outlets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BussinessID,AddressID,Name,Description,ContactNumber,ContactEmail,Rating,ReviewCount,IsOpen,Weight,Status,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Outlet outlet)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;


            outlet.Address.ModifiedBy = userName;
            outlet.Address.ModifiedDate = DateTime.UtcNow;
            outlet.Address.Id = outlet.AddressID;
            outlet.ModifiedBy = userName;
            outlet.ModifiedDate = DateTime.UtcNow;

            if (id != outlet.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outlet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutletExists(outlet.ID))
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
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", outlet.AddressID);
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", outlet.BussinessID);
            return View(outlet);
        }

        // GET: Outlets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outlet = await _context.Outlets
                .Include(o => o.Address)
                .Include(o => o.Bussiness)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outlet == null)
            {
                return NotFound();
            }

            return View(outlet);
        }

        // POST: Outlets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outlet = await _context.Outlets.FindAsync(id);
            if (outlet != null)
            {
                _context.Outlets.Remove(outlet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutletExists(int id)
        {
            return _context.Outlets.Any(e => e.ID == id);
        }
    }
}
