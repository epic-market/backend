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
            var businesses = _context.Businesses.Select(c => new BusinessModel() { Name = c.Name , Id = c.ID}).ToList();
            return View(businesses);
        }

        [HttpGet]
        public IActionResult GetBranches(int businessId)
        {
            var branches = _context.Outlets
                .Where(b => b.BussinessID == businessId)
                .Select(b => new { b.ID, b.Name })
                .ToList();
            return Json(branches);
        }

        [HttpGet]
        public IActionResult GetProducts(int branchId)
        {
            var products = _context.OutletProducts
                .Where(p => p.OutletID == branchId)
                .Include(p=>p.Product)
                .Select(p => new Product { Id = p.Product.ID,Name =  p.Product.Name,Price = (decimal)p.Product.Rate })
                .ToList();
            return Json(products);
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] SingleOrder order)
        {
            // Process the order (save to database, etc.)
            return Ok();
        }






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
