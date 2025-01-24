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
using System.Security.Claims;
using EpicMarket.Entities;
using Newtonsoft.Json;
using System.Drawing.Printing;

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
            var applicationDbContext = _context.Orders.Include(o => o.Address).Include(o => o.Outlet).Include(o => o.Person).Include(o=>o.OrderStatusOptions).Include(o => o.OrderTypesOptions);
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
                .Include(o=> o.OrderStatusOptions)
                .Include(o=>o.OrderTypesOptions)
                .FirstOrDefaultAsync(m => m.ID == id);


            var orderDetails = await _context.OrderDetails.
                Where(o=> o.OrderID == id)
               .Include(o => o.CatalogVariants)
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
            var products = _context.Inventory
                .Where(p => p.OutletID == branchId)
                .Include(p=>p.CatalogVariants)
                .Select(p => new Product { Id = p.CatalogVariants.Catalog.ID,Name =  p.CatalogVariants.Catalog.Name,Price = (decimal)p.CatalogVariants.SalePrice })
                .ToList();
            return Json(products);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] SingleOrder order)
        {

            var User = new AppUser();
            var totalItems = 0;
            double totalPrice = 0.0;
            var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == order.CustomerDetails.Email || u.PhoneNumber == order.CustomerDetails.Phone);

            var orderStatusID =  _context.OrderStatusOptions.FirstOrDefault(c => c.OrderStatus == "Order Placed").Id;

            var OrderTypeID = await _context.OrderTypesOptions
                      .FirstOrDefaultAsync(u => u.Ordertype == OrderType.OFFLINE);

            if (existingUser == null)
            {

                User.FirstName = order.CustomerDetails.Phone;
                User.UserName = order.CustomerDetails.Email;
                User.Email = order.CustomerDetails.Phone;
                User.PhoneNumber = order.CustomerDetails.Phone;
                await _context.Users.AddAsync(User);
                await _context.SaveChangesAsync();
            }
            else
            {
                User.Id = existingUser.Id;
            }
            var listoforderDetails = new List<OrderDetail>();


            foreach (var orderDetail in order.Items)
            {
                var productVariant = _context.CatalogVariants.FirstOrDefault(p => p.VariantID == orderDetail.ProductId);
                var singleOrderDetail = new OrderDetail();
                singleOrderDetail.VariantID = orderDetail.ProductId;
                singleOrderDetail.Quantity = orderDetail.Quantity;
                singleOrderDetail.Rate = productVariant.SalePrice;
                singleOrderDetail.TotalPrice = productVariant.SalePrice * orderDetail.Quantity;
                listoforderDetails.Add(singleOrderDetail);
                totalItems += orderDetail.Quantity;
                totalPrice += (productVariant.SalePrice * orderDetail.Quantity);
            }


            var newOrder = new Order()
            {
                PersonID = User.Id,
                OutletID = order.OutletID,
                OrderTypeId = OrderTypeID.Id,
                OrderAt = DateTime.Now,
                StatusId = orderStatusID,
                PaymentMode = order.PaymentMode,
                TotalItems = totalItems,
                TotalPrice = totalPrice,
            };

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();


            listoforderDetails.ForEach(od => od.OrderID = newOrder.ID);
            await _context.OrderDetails.AddRangeAsync(listoforderDetails);
            await _context.SaveChangesAsync();


            var saved = _context.Orders.FirstOrDefault(o => o.ID == newOrder.ID);


             return RedirectToAction(nameof(Index)); ;
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Where(o => o.ID == id).Include(c => c.Address).Include(c => c.OrderTypesOptions).Include(c => c.Outlet).Include(c => c.Person).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatusOptions, "Id", "OrderStatus", order.StatusId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,BusinessID,OrderType,TotalPrice,TotalItems,OrderAt,OrderTypeId,OutletID,StatusId,PaymentMode,AddressID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Order order)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;


            if (order.AddressID != null)
            {
                order.Address.ModifiedBy = userName;
                order.Address.ModifiedDate = DateTime.UtcNow;
            }
            else {
                order.Address.CreateBy = userName;
                order.Address.CreateDate = DateTime.UtcNow;
            }

            order.ModifiedBy = userName;
            order.ModifiedDate = DateTime.UtcNow;
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
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatusOptions, "Id", "OrderStatus", order.StatusId);
            return View(order);
        }

    }
}
