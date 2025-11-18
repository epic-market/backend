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
using EpicMarket.Entities;
using Newtonsoft.Json;
using System.Drawing.Printing;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using System.Text.Json;
using EpicMarket.Entities.Constants;

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderStatuses()
        {
            var statuses = await _context.OrderStatusOptions
                .OrderBy(s => s.Id)
                .Select(s => new { id = s.Id, orderStatus = s.OrderStatus })
                .ToListAsync();
            return Json(statuses);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderTypes()
        {
            var types = await _context.OrderTypesOptions
                .OrderBy(t => t.Id)
                .Select(t => new { id = t.Id, ordertype = t.Ordertype })
                .ToListAsync();
            return Json(types);
        }

        [HttpPost]
        [Route("Orders/GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] OrderFilterViewModel filter)
        {
            try
            {
                var query = _context.Orders
                    .Include(o => o.Person)
                    .Include(o => o.Outlet)
                    .Include(o => o.OrderStatusOptions)
                    .Include(o => o.OrderTypesOptions)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.OrderId))
                {
                    query = query.Where(o => o.ID.ToString().Contains(filter.OrderId));
                }

                if (!string.IsNullOrWhiteSpace(filter.CustomerName))
                {
                    query = query.Where(o => o.Person.UserName.Contains(filter.CustomerName) || 
                                            o.Person.FirstName.Contains(filter.CustomerName) || 
                                            o.Person.LastName.Contains(filter.CustomerName));
                }

                if (!string.IsNullOrWhiteSpace(filter.OutletName))
                {
                    query = query.Where(o => o.Outlet.Name.Contains(filter.OutletName));
                }

                if (!string.IsNullOrWhiteSpace(filter.OrderStatus))
                {
                    if (int.TryParse(filter.OrderStatus, out int statusId))
                    {
                        query = query.Where(o => o.StatusId == statusId);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filter.OrderType))
                {
                    if (int.TryParse(filter.OrderType, out int typeId))
                    {
                        query = query.Where(o => o.OrderTypeId == typeId);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filter.PaymentMode))
                {
                    query = query.Where(o => o.PaymentMode.Contains(filter.PaymentMode));
                }

                if (!string.IsNullOrWhiteSpace(filter.DateFrom))
                {
                    if (DateTime.TryParse(filter.DateFrom, out DateTime dateFrom))
                    {
                        query = query.Where(o => o.OrderAt >= dateFrom);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filter.DateTo))
                {
                    if (DateTime.TryParse(filter.DateTo, out DateTime dateTo))
                    {
                        // Add one day to include the entire day
                        dateTo = dateTo.AddDays(1).AddSeconds(-1);
                        query = query.Where(o => o.OrderAt <= dateTo);
                    }
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(o => o.ID) : query.OrderByDescending(o => o.ID),
                    "customername" => filter.SortDirection == "asc" ? query.OrderBy(o => o.Person.UserName) : query.OrderByDescending(o => o.Person.UserName),
                    "outletname" => filter.SortDirection == "asc" ? query.OrderBy(o => o.Outlet.Name) : query.OrderByDescending(o => o.Outlet.Name),
                    "totalprice" => filter.SortDirection == "asc" ? query.OrderBy(o => o.TotalPrice) : query.OrderByDescending(o => o.TotalPrice),
                    "orderat" => filter.SortDirection == "asc" ? query.OrderBy(o => o.OrderAt) : query.OrderByDescending(o => o.OrderAt),
                    "status" => filter.SortDirection == "asc" ? query.OrderBy(o => o.OrderStatusOptions.OrderStatus) : query.OrderByDescending(o => o.OrderStatusOptions.OrderStatus),
                    "ordertype" => filter.SortDirection == "asc" ? query.OrderBy(o => o.OrderTypesOptions.Ordertype) : query.OrderByDescending(o => o.OrderTypesOptions.Ordertype),
                    _ => query.OrderByDescending(o => o.OrderAt) // Default to newest orders first
                };

                // Apply pagination and project to DTO
                var orders = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(o => new OrderDto
                    {
                        ID = o.ID,
                        CustomerName = o.Person.UserName,
                        OutletName = o.Outlet.Name,
                        OutletId = o.OutletID,
                        TotalPrice = o.TotalPrice,
                        TotalItems = o.TotalItems,
                        OrderAt = o.OrderAt,
                        StatusName = o.OrderStatusOptions.OrderStatus,
                        StatusId = o.StatusId,
                        OrderTypeName = o.OrderTypesOptions.Ordertype,
                        OrderTypeId = o.OrderTypeId,
                        PaymentMode = o.PaymentMode
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = orders });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
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
                .Include(o => o.OrderStatusOptions)
                .Include(o => o.OrderTypesOptions)
                .FirstOrDefaultAsync(m => m.ID == id);


            var orderDetails = await _context.OrderDetails
                .Where(o => o.OrderID == id)
                .Include(o => o.ProductVariants)
                .ThenInclude(cv => cv.Product)
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
                .Include(p => p.ProductVariants)
                .ThenInclude(cv => cv.Product)
                .Select(p => new {
                    Id = p.ProductVariants.Product.ID,
                    Name = p.ProductVariants.Product.Name,
                    Price = (decimal)p.ProductVariants.SalePrice,
                    VariantId = p.ProductVariants.ID,
                    Attributes = p.ProductVariants.Attributes
                })
                .ToList();
            return Json(products);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid order data" });
            }

            try
            {
                // Get the current user
                var userName = User.Identity.Name;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create customer if needed
                var customer = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.CustomerEmail);
                if (customer == null)
                {
                    // Create a new customer
                    customer = new AppUser
                    {
                        UserName = model.CustomerEmail,
                        Email = model.CustomerEmail,
                        PhoneNumber = model.CustomerPhone,
                        FirstName = model.CustomerName,
                    };
                    
                    // Note: In a real application, you would need to create this user properly through Identity
                    // This is a simplified version for demonstration
                    _context.Users.Add(customer);
                    await _context.SaveChangesAsync();
                }

                // Create the order
                var order = new Order
                {
                    PersonID = customer.Id,
                    OutletID = model.OutletId,
                    OrderTypeId = 2, // Assuming 2 is for offline/POS orders
                    TotalPrice = model.TotalPrice,
                    TotalItems = model.TotalItems,
                    OrderAt = DateTime.UtcNow,
                    StatusId = 1, // Initial status (e.g., "Pending")
                    PaymentMode = model.PaymentMode,
                    CreateBy = userName,
                    CreateDate = DateTime.UtcNow
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order details
                foreach (var item in model.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.ID,
                        VariantID = item.VariantId,
                        Quantity = item.Quantity,
                        Rate = item.Rate,
                        TotalPrice = item.TotalPrice,
                        CreateBy = userName,
                            CreateDate = DateTime.UtcNow
                    };

                    _context.OrderDetails.Add(orderDetail);
                }

                await _context.SaveChangesAsync();

                // Update inventory quantities
                // foreach (var item in model.OrderDetails)
                // {
                //     var inventory = await _context.Inventory
                //         .FirstOrDefaultAsync(i => i.ProductVariants.ID == item.VariantId && i.OutletID == model.OutletId);
                    
                //     if (inventory != null)
                //     {
                //         inventory.Quantity -= item.Quantity;
                //         inventory.ModifiedBy = userName;
                //         inventory.ModifiedDate = DateTime.UtcNow;
                //         _context.Inventory.Update(inventory);
                //     }
                // }

                await _context.SaveChangesAsync();

                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CREATE,
                    EntityName = EntityConstants.Order,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created new order with ID: {order.ID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(order),
                    RecordId = order.ID,
                    BusinessID = 0,
                    LoggedInUserName = userName
                });

                return Ok(new { success = true, orderId = order.ID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while placing the order: " + ex.Message });
            }
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
            var variants = _context.ProductVariants
                .Where(v => v.ProductID == productId)
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
