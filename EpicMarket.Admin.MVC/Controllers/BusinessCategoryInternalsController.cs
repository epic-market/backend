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
using EpicMarket.Data.ApplicationModels;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class BusinessCategoryInternalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public BusinessCategoryInternalsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: BusinessCategoryInternals
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessCategories.ToListAsync());
        }

        // GET: BusinessCategoryInternals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories.Include(c=>c.Businesses)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusinessCategoryInternals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Type,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BusinessCategoryInternal businessCategoryInternal)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            businessCategoryInternal.CreateBy = userName;
            businessCategoryInternal.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(businessCategoryInternal);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddBusinessCategory,
                    EntityName = EntityConstants.BusinessCategory,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added business category '{businessCategoryInternal.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(businessCategoryInternal),
                    RecordId = businessCategoryInternal.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories.FindAsync(id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }
            return View(businessCategoryInternal);
        }

        // POST: BusinessCategoryInternals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Type,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] BusinessCategoryInternal businessCategoryInternal)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            businessCategoryInternal.ModifiedBy = userName;
            businessCategoryInternal.ModifiedDate = DateTime.UtcNow;
            if (id != businessCategoryInternal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.BusinessCategories.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(businessCategoryInternal);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditBusinessCategory,
                        EntityName = EntityConstants.BusinessCategory,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated business category '{businessCategoryInternal.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = businessCategoryInternal
                        }),
                        RecordId = businessCategoryInternal.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessCategoryInternalExists(businessCategoryInternal.ID))
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
            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            return View(businessCategoryInternal);
        }

        // POST: BusinessCategoryInternals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessCategoryInternal = await _context.BusinessCategories.FindAsync(id);
            if (businessCategoryInternal != null)
            {
                _context.BusinessCategories.Remove(businessCategoryInternal);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteBusinessCategory,
                    EntityName = EntityConstants.BusinessCategory,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted business category '{businessCategoryInternal.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(businessCategoryInternal),
                    RecordId = businessCategoryInternal.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessCategoryInternalExists(int id)
        {
            return _context.BusinessCategories.Any(e => e.ID == id);
        }
    }
}
