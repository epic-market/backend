using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class AccessTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public AccessTypesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: AccessTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccessTypes.ToListAsync());
        }

        // GET: AccessTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessType = await _context.AccessTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accessType == null)
            {
                return NotFound();
            }

            return View(accessType);
        }

        // GET: AccessTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccessTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Priority")] AccessType accessType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accessType);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddAccessType,
                    EntityName = EntityConstants.AccessType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added access type '{accessType.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(accessType),
                    RecordId = accessType.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(accessType);
        }

        // GET: AccessTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessType = await _context.AccessTypes.FindAsync(id);
            if (accessType == null)
            {
                return NotFound();
            }
            return View(accessType);
        }

        // POST: AccessTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Priority")] AccessType accessType)
        {
            if (id != accessType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.AccessTypes.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    _context.Update(accessType);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAccessType,
                        EntityName = EntityConstants.AccessType,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated access type '{accessType.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = accessType
                        }),
                        RecordId = accessType.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessTypeExists(accessType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(accessType);
        }

        // GET: AccessTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessType = await _context.AccessTypes
				.FirstOrDefaultAsync(m => m.Id == id);
            if (accessType == null)
            {
                return NotFound();
            }

            return View(accessType);
        }

        // POST: AccessTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessType = await _context.AccessTypes.FindAsync(id);
            if (accessType != null)
            {
                _context.AccessTypes.Remove(accessType);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteAccessType,
                    EntityName = EntityConstants.AccessType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted access type '{accessType.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(accessType),
                    RecordId = accessType.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AccessTypeExists(int id)
        {
            return _context.AccessTypes.Any(e => e.Id == id);
        }
    }
}
