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
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class FAQsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public FAQsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: FAQs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FAQs.Include(f => f.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FAQs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQs
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // GET: FAQs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.FAQCategories, "Id", "CategoryTitle");
            return View();
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer,CategoryId,RoleType")] FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fAQ);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CreateFAQ,
                    EntityName = EntityConstants.FAQ,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created FAQ: {fAQ.Title}",
                    Data = System.Text.Json.JsonSerializer.Serialize(fAQ),
                    RecordId = fAQ.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.FAQCategories, "Id", "CategoryTitle", fAQ.CategoryId);
            return View(fAQ);
        }

        // GET: FAQs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.FAQCategories, "Id", "CategoryTitle", fAQ.CategoryId);
            return View(fAQ);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer,CategoryId,RoleType")] FAQ fAQ)
        {
            if (id != fAQ.Id)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.FAQs
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fAQ);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditFAQ,
                        EntityName = EntityConstants.FAQ,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated FAQ: {fAQ.Title}",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = fAQ
                        }),
                        RecordId = fAQ.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(fAQ.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.FAQCategories, "Id", "CategoryTitle", fAQ.CategoryId);
            return View(fAQ);
        }

        // GET: FAQs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQs
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ != null)
            {
                _context.FAQs.Remove(fAQ);
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteFAQ,
                    EntityName = EntityConstants.FAQ,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted FAQ: {fAQ.Title}",
                    Data = System.Text.Json.JsonSerializer.Serialize(fAQ),
                    RecordId = fAQ.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FAQExists(int id)
        {
            return _context.FAQs.Any(e => e.Id == id);
        }
    }
}
