using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class StatusOptionSetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public StatusOptionSetsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: StatusOptionSets
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatusOptionSets.ToListAsync());
        }

        // GET: StatusOptionSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOptionSet = await _context.StatusOptionSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusOptionSet == null)
            {
                return NotFound();
            }

            return View(statusOptionSet);
        }

        // GET: StatusOptionSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusOptionSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,StatusDescription")] StatusOptionSet statusOptionSet)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            statusOptionSet.CreateBy = userName;
            statusOptionSet.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(statusOptionSet);
                await _context.SaveChangesAsync();
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddStatusOptionSet,
                    EntityName = EntityConstants.StatusOptionSet,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added status option '{statusOptionSet.Status}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(statusOptionSet),
                    RecordId = statusOptionSet.Id,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            return View(statusOptionSet);
        }

        // GET: StatusOptionSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOptionSet = await _context.StatusOptionSets.FindAsync(id);
            if (statusOptionSet == null)
            {
                return NotFound();
            }
            return View(statusOptionSet);
        }

        // POST: StatusOptionSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,StatusDescription,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] StatusOptionSet statusOptionSet)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            statusOptionSet.ModifiedBy = userName;
            statusOptionSet.ModifiedDate = DateTime.UtcNow;
            
            if (id != statusOptionSet.Id)
            {
                return NotFound();
            }

            // Get the original entity for comparison
            var originalStatusOptionSet = await _context.StatusOptionSets
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusOptionSet);
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditStatusOptionSet,
                        EntityName = EntityConstants.StatusOptionSet,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated status option '{statusOptionSet.Status}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            Original = originalStatusOptionSet, 
                            Updated = statusOptionSet 
                        }),
                        RecordId = statusOptionSet.Id,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusOptionSetExists(statusOptionSet.Id))
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
            return View(statusOptionSet);
        }

        // GET: StatusOptionSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusOptionSet = await _context.StatusOptionSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusOptionSet == null)
            {
                return NotFound();
            }

            return View(statusOptionSet);
        }

        // POST: StatusOptionSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusOptionSet = await _context.StatusOptionSets.FindAsync(id);
            if (statusOptionSet != null)
            {
                _context.StatusOptionSets.Remove(statusOptionSet);
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteStatusOptionSet,
                    EntityName = EntityConstants.StatusOptionSet,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted status option '{statusOptionSet.Status}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(statusOptionSet),
                    RecordId = statusOptionSet.Id,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusOptionSetExists(int id)
        {
            return _context.StatusOptionSets.Any(e => e.Id == id);
        }
    }
}
