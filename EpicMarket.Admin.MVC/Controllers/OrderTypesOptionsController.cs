using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class OrderTypesOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public OrderTypesOptionsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: OrderTypesOptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderTypesOptions.ToListAsync());
        }

        // GET: OrderTypesOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTypesOptions = await _context.OrderTypesOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderTypesOptions == null)
            {
                return NotFound();
            }

            return View(orderTypesOptions);
        }

        // GET: OrderTypesOptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderTypesOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ordertype")] OrderTypesOptions orderTypesOptions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderTypesOptions);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CREATE,
                    EntityName = EntityConstants.OrderTypesOption,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created order type option: {orderTypesOptions.Ordertype}",
                    Data = System.Text.Json.JsonSerializer.Serialize(orderTypesOptions),
                    RecordId = orderTypesOptions.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            return View(orderTypesOptions);
        }

        // GET: OrderTypesOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTypesOptions = await _context.OrderTypesOptions.FindAsync(id);
            if (orderTypesOptions == null)
            {
                return NotFound();
            }
            return View(orderTypesOptions);
        }

        // POST: OrderTypesOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ordertype")] OrderTypesOptions orderTypesOptions)
        {
            if (id != orderTypesOptions.Id)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.OrderTypesOptions
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderTypesOptions);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.UPDATE,
                        EntityName = EntityConstants.OrderTypesOption,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated order type option: {orderTypesOptions.Ordertype}",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = orderTypesOptions
                        }),
                        RecordId = orderTypesOptions.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderTypesOptionsExists(orderTypesOptions.Id))
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
            return View(orderTypesOptions);
        }

        // GET: OrderTypesOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTypesOptions = await _context.OrderTypesOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderTypesOptions == null)
            {
                return NotFound();
            }

            return View(orderTypesOptions);
        }

        // POST: OrderTypesOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderTypesOptions = await _context.OrderTypesOptions.FindAsync(id);
            if (orderTypesOptions != null)
            {
                _context.OrderTypesOptions.Remove(orderTypesOptions);
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DELETE,
                    EntityName = EntityConstants.OrderTypesOption,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted order type option: {orderTypesOptions.Ordertype}",
                    Data = System.Text.Json.JsonSerializer.Serialize(orderTypesOptions),
                    RecordId = orderTypesOptions.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderTypesOptionsExists(int id)
        {
            return _context.OrderTypesOptions.Any(e => e.Id == id);
        }
    }
}
