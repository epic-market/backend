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
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;
namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class HelpItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public HelpItemsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: HelpItems
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.HelpItems.Include(h => h.Pages);
            return View(await authDbContext.ToListAsync());
        }

        // GET: HelpItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems
                .Include(h => h.Pages)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (helpItem == null)
            {
                return NotFound();
            }

            return View(helpItem);
        }

        // GET: HelpItems/Create
        public IActionResult Create()
        {
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "Name");
            return View();
        }

        // POST: HelpItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,PageID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] HelpItem helpItem)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            helpItem.CreateBy = userName;
            helpItem.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(helpItem);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CreateHelpItem,
                    EntityName = EntityConstants.HelpItem,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created help item: {helpItem.Title}",
                    Data = System.Text.Json.JsonSerializer.Serialize(helpItem),
                    RecordId = helpItem.ID,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "ID", helpItem.PageID);
            return View(helpItem);
        }

        // GET: HelpItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems.FindAsync(id);
            if (helpItem == null)
            {
                return NotFound();
            }
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "Name", helpItem.PageID);
            return View(helpItem);
        }

        // POST: HelpItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,PageID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] HelpItem helpItem)
        {
            if (id != helpItem.ID)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.HelpItems
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.ID == id);

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            helpItem.ModifiedBy = userName;
            helpItem.ModifiedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(helpItem);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditHelpItem,
                        EntityName = EntityConstants.HelpItem,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated help item: {helpItem.Title}",
                        Data = System.Text.Json.JsonSerializer.Serialize(helpItem),
                        RecordId = helpItem.ID,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HelpItemExists(helpItem.ID))
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
            ViewData["PageID"] = new SelectList(_context.Pages, "ID", "ID", helpItem.PageID);
            return View(helpItem);
        }

        // GET: HelpItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpItem = await _context.HelpItems
                .Include(h => h.Pages)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (helpItem == null)
            {
                return NotFound();
            }

            return View(helpItem);
        }

        // POST: HelpItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var helpItem = await _context.HelpItems.FindAsync(id);
            if (helpItem != null)
            {
                _context.HelpItems.Remove(helpItem);
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteHelpItem,
                    EntityName = EntityConstants.HelpItem,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted help item: {helpItem.Title}",
                    Data = System.Text.Json.JsonSerializer.Serialize(helpItem),
                    RecordId = helpItem.ID,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HelpItemExists(int id)
        {
            return _context.HelpItems.Any(e => e.ID == id);
        }
    }
}
