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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using System.Text.Json;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]   
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public OrdersController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
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
                .Include(p => p.CatalogVariants)
                .ThenInclude(cv => cv.Catalog)
                .Select(p => new {
                    Id = p.CatalogVariants.Catalog.ID,
                    Name = p.CatalogVariants.Catalog.Name,
                    Price = (decimal)p.CatalogVariants.SalePrice,
                    VariantId = p.CatalogVariants.ID
                })
                .ToList();
            return Json(products);
        }

        // [HttpPost]
        // public IActionResult PlaceOrder([FromBody] PlaceOrderViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(new { message = "Invalid order data" });
        //     }

        //     try
        //     {
        //         // // Get the current user
        //         // var userName = User.Identity.Name;
        //         // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //         // // Create customer if needed
        //         // var customer = _context.ApplicationUsers.FirstOrDefault(u => u.Email == model.CustomerEmail);
        //         // if (customer == null)
        //         // {
        //         //     // Create a new customer
        //         //     customer = new AppUser
        //         //     {
        //         //         UserName = model.CustomerEmail,
        //         //         Email = model.CustomerEmail,
        //         //         PhoneNumber = model.CustomerPhone,
        //         //         Name = model.CustomerName,
        //         //         CreatedAt = DateTime.UtcNow,
        //         //         CreatedBy = userName
        //         //     };
                    
        //         //     // Note: In a real application, you would need to create this user properly through Identity
        //         //     // This is a simplified version for demonstration
        //         //     _context.ApplicationUsers.Add(customer);
        //         //     _context.SaveChanges();
        //         // }

        //         // // Create the order
        //         // var order = new Order
        //         // {
        //         //     PersonID = int.Parse(userId),
        //         //     OutletID = model.OutletId,
        //         //     OrderTypeId = 2, // Assuming 2 is for offline/POS orders
        //         //     TotalPrice = model.TotalPrice,
        //         //     TotalItems = model.TotalItems,
        //         //     OrderAt = DateTime.UtcNow,
        //         //     StatusId = 1, // Initial status (e.g., "Pending")
        //         //     PaymentMode = model.PaymentMode,
        //         //     CreatedBy = userName,
        //         //     CreatedAt = DateTime.UtcNow
        //         // };

        //         // _context.Orders.Add(order);
        //         // _context.SaveChanges();

        //         // // Create order details
        //         // foreach (var item in model.OrderDetails)
        //         // {
        //         //     var orderDetail = new OrderDetail
        //         //     {
        //         //         OrderID = order.ID,
        //         //         VariantID = item.VariantId,
        //         //         Quantity = item.Quantity,
        //         //         Rate = item.Rate,
        //         //         TotalPrice = item.TotalPrice,
        //         //         CreatedBy = userName,
        //         //         CreatedAt = DateTime.UtcNow
        //         //     };

        //         //     _context.OrderDetails.Add(orderDetail);
        //         // }

        //         // _context.SaveChanges();

        //         // // Log the event
        //         // _eventService.LogEvent(new EventLog
        //         // {
        //         //     EventType = EventType.Create,
        //         //     TableName = "Orders",
        //         //     Data = System.Text.Json.JsonSerializer.Serialize(order),
        //         //     RecordId = order.ID,
        //         //     BusinessID = 0,
        //         //     LoggedInUserName = userName
        //         // });

        //         // return Ok(new { success = true, orderId = order.ID });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { message = "An error occurred while placing the order: " + ex.Message });
        //     }
        // }

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
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DELETE,
                    EntityName = EntityConstants.Order,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted order with ID: {order.ID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(order),
                    RecordId = order.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
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
            
            // Get original entity for event logging
            var originalEntity = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ID == id);

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
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.UPDATE,
                        EntityName = EntityConstants.Order,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated order with ID: {order.ID}",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = order
                        }),
                        RecordId = order.ID,
                        BusinessID = 0,
                        LoggedInUserName = userName
                    });
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

        [HttpGet]
        public IActionResult GetProductVariants(int productId)
        {
            var variants = _context.CatalogVariants
                .Where(v => v.CatalogID == productId)
                .Select(v => new {
                    id = v.ID,
                    salePrice = v.SalePrice,
                    attributes = v.Attributes
                })
                .ToList();
            return Json(variants);
        }

    }
}
