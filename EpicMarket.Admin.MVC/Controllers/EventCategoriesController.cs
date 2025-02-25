using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using System.Security.Claims;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class EventCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public EventCategoriesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: EventCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationsTable.ToListAsync());
        }

        // GET: EventCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.ApplicationsTable
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        // GET: EventCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Sequence")] ApplicationsTable eventCategory)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            eventCategory.CreateBy = userName;
            eventCategory.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(eventCategory);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddEventCategory,
                    EntityName = EntityConstants.EventCategory,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added event category '{eventCategory.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(eventCategory),
                    RecordId = eventCategory.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        // GET: EventCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.ApplicationsTable.FindAsync(id);
            if (eventCategory == null)
            {
                return NotFound();
            }
            return View(eventCategory);
        }

        // POST: EventCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Sequence,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ApplicationsTable eventCategory)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            eventCategory.ModifiedBy = userName;
            eventCategory.ModifiedDate = DateTime.UtcNow;
            if (id != eventCategory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.ApplicationsTable.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(eventCategory);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditEventCategory,
                        EntityName = EntityConstants.EventCategory,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated event category '{eventCategory.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = eventCategory
                        }),
                        RecordId = eventCategory.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategoryExists(eventCategory.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(eventCategory);
        }

        // GET: EventCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.ApplicationsTable
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        // POST: EventCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCategory = await _context.ApplicationsTable.FindAsync(id);
            if (eventCategory != null)
            {
                _context.ApplicationsTable.Remove(eventCategory);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteEventCategory,
                    EntityName = EntityConstants.EventCategory,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted event category '{eventCategory.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(eventCategory),
                    RecordId = eventCategory.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCategoryExists(int id)
        {
            return _context.ApplicationsTable.Any(e => e.ID == id);
        }
    }
}
