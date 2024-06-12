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
using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Address).Include(o => o.Outlet).Include(o => o.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var orderModel = new OrderDetailsModel(); 
            var order = await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Outlet)
                .Include(o => o.Outlet.Bussiness)
				.Include(o => o.Person)
                .FirstOrDefaultAsync(m => m.ID == id);


            var orderDetails = await _context.OrderDetails.
                Where(o=> o.OrderID == id)
               .Include(o => o.Catalog)
                .ToListAsync();

            orderModel.Order = order;
            orderModel.OrderDetails = orderDetails;


            if (order == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID");
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PersonID,BusinessID,OrderType,TotalPrice,TotalItems,OrderAt,Status,PaymentMode,AddressID,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressID);
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID", order.OutletID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "Id", order.PersonID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Where(o=> o.ID == id).Include(c=>c.Address).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BusinessID"] = new SelectList(_context.Outlets, "ID", "Name", order.OutletID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName", order.PersonID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,BusinessID,OrderType,TotalPrice,TotalItems,OrderAt,Status,PaymentMode,AddressID,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
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
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressID);
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID", order.OutletID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "Id", order.PersonID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Outlet)
                .Include(o => o.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
