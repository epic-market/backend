using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using System.Reflection.Metadata;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.Constants;
namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class EventsController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public EventsController(
            AuthDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Event.Include(c=> c.EventCategorys);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(c => c.EventCategorys)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["EventCategoryID"] = new SelectList(_context.ApplicationsTable, "ID", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,EventCategoryID,Name,Description,PriorityID,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Event @event)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            @event.CreateBy = userName;
            @event.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddEvent,
                    EntityName = EntityConstants.Event,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added event '{@event.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(@event),
                    RecordId = @event.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            ViewData["EventCategoryID"] = new SelectList(_context.ApplicationsTable, "ID", "Name", @event.EventCategoryID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["EventCategoryID"] = new SelectList(_context.ApplicationsTable, "ID", "Name", @event.EventCategoryID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,EventCategoryID,Name,Description,PriorityID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Event @event)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            @event.ModifiedBy = userName;
            @event.ModifiedDate = DateTime.UtcNow;
            if (id != @event.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.Event.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(@event);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditEvent,
                        EntityName = EntityConstants.Event,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated event '{@event.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = @event
                        }),
                        RecordId = @event.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["EventCategoryID"] = new SelectList(_context.ApplicationsTable, "ID", "Name", @event.EventCategoryID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(c => c.EventCategorys)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteEvent,
                    EntityName = EntityConstants.Event,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted event '{@event.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(@event),
                    RecordId = @event.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.ID == id);
        }
    }
}
