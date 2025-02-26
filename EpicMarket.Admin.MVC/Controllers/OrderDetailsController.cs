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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [SecurableAuthorize(SecurableConstants.OrderDetailsView)]
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public OrderDetailsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: OrderDetails
        [SecurableAuthorize(SecurableConstants.OrderDetailsView)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderDetails.Include(o => o.CatalogVariants).Include(o => o.Order);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        [SecurableAuthorize(SecurableConstants.OrderDetailsView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.CatalogVariants)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        [SecurableAuthorize(SecurableConstants.OrderDetailsAdd)]
        public IActionResult Create()
        {
            ViewData["CatalogID"] = new SelectList(_context.Catalogs, "ID", "ID");
            ViewData["OrderID"] = new SelectList(_context.Orders, "ID", "ID");
            return View();
        }

        // POST: OrderDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.OrderDetailsAdd)]
        public async Task<IActionResult> Create([Bind("ID,OrderID,CatalogID,Quantity,Rate,TotalPrice,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CreateOrderDetail,
                    EntityName = EntityConstants.OrderDetail,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created order detail for order ID: {orderDetail.OrderID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(orderDetail),
                    RecordId = orderDetail.ID,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatalogID"] = new SelectList(_context.Catalogs, "ID", "ID", orderDetail.CatalogVariants.CatalogID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "ID", "ID", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        [SecurableAuthorize(SecurableConstants.OrderDetailsEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["CatalogID"] = new SelectList(_context.Catalogs, "ID", "ID", orderDetail.CatalogVariants.CatalogID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "ID", "ID", orderDetail.OrderID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.OrderDetailsEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OrderID,CatalogID,Quantity,Rate,TotalPrice,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] OrderDetail orderDetail)
        {
            if (id != orderDetail.ID)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.OrderDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(od => od.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditOrderDetail,
                        EntityName = EntityConstants.OrderDetail,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated order detail ID: {orderDetail.ID}",
                        Data = System.Text.Json.JsonSerializer.Serialize(orderDetail),
                        RecordId = orderDetail.ID,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.ID))
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
            ViewData["CatalogID"] = new SelectList(_context.Catalogs, "ID", "ID", orderDetail.CatalogVariants.CatalogID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "ID", "ID", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        [SecurableAuthorize(SecurableConstants.OrderDetailsDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.CatalogVariants)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.OrderDetailsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteOrderDetail,
                    EntityName = EntityConstants.OrderDetail,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted order detail ID: {orderDetail.ID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(orderDetail),
                    RecordId = orderDetail.ID,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.ID == id);
        }
    }
}
