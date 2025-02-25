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
using EpicMarket.Admin.MVC.Services;
using System.Text.Json;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public PagesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Pages
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Pages.Include(p => p.ApplicationsTable);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .Include(p => p.ApplicationsTable)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Pages/Create
        public IActionResult Create(string returnUrl = null)
        {
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,ApplicationId,Url,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Page page, string returnUrl = null)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            page.CreateBy = userName;
            page.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(page);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CREATE,
                    EntityName = EntityConstants.Page,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created page: {page.Name}",
                    Data = JsonSerializer.Serialize(page),
                    RecordId = page.ID,
                    BusinessID = 0,
                    LoggedInUserName = userName
                });
                
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ReturnUrl = returnUrl;
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name", page.ApplicationId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name", page.ApplicationId);
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,ApplicationId,Url,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Page page)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            page.ModifiedBy = userName;
            page.ModifiedDate = DateTime.UtcNow;
            
            // Get original entity for event logging
            var originalEntity = await _context.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);
                
            if (id != page.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.UPDATE,
                        EntityName = EntityConstants.Page,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated page: {page.Name}",
                        Data = JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = page
                        }),
                        RecordId = page.ID,
                        BusinessID = 0,
                        LoggedInUserName = userName
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.ID))
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
            ViewData["ApplicationId"] = new SelectList(_context.ApplicationsTable, "ID", "Name", page.ApplicationId);
            return View(page);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .Include(p => p.ApplicationsTable)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                _context.Pages.Remove(page);
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DELETE,
                    EntityName = EntityConstants.Page,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted page: {page.Name}",
                    Data = JsonSerializer.Serialize(page),
                    RecordId = page.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.ID == id);
        }
    }
}
