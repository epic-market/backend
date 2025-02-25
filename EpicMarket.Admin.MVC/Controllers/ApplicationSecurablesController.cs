using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;
using System.Reflection.Metadata;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class ApplicationSecurablesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public ApplicationSecurablesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: ApplicationSecurables
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationSecurables.ToListAsync());
        }

        // GET: ApplicationSecurables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationSecurables = await _context.ApplicationSecurables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationSecurables == null)
            {
                return NotFound();
            }

            return View(applicationSecurables);
        }

        // GET: ApplicationSecurables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationSecurables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationSecurables applicationSecurables)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            applicationSecurables.CreateBy = userName;
            applicationSecurables.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(applicationSecurables);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddApplicationSecurable,
                    EntityName = EntityConstants.ApplicationSecurable,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added application securable '{applicationSecurables.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(applicationSecurables),
                    RecordId = applicationSecurables.Id,
                    BusinessID = 0,
                    LoggedInUserName = userName
                });

                return RedirectToAction(nameof(Index));
            }
            return View(applicationSecurables);
        }

        // GET: ApplicationSecurables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationSecurables = await _context.ApplicationSecurables.FindAsync(id);
            if (applicationSecurables == null)
            {
                return NotFound();
            }
            return View(applicationSecurables);
        }

        // POST: ApplicationSecurables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ApplicationSecurables applicationSecurables)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            applicationSecurables.ModifiedBy = userName;
            applicationSecurables.ModifiedDate = DateTime.UtcNow;

            if (id != applicationSecurables.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.ApplicationSecurables.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    _context.Update(applicationSecurables);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditApplicationSecurable,
                        EntityName = EntityConstants.ApplicationSecurable,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated application securable '{applicationSecurables.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = applicationSecurables
                        }),
                        RecordId = applicationSecurables.Id,
                        BusinessID = 0,
                        LoggedInUserName = userName
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationSecurablesExists(applicationSecurables.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(applicationSecurables);
        }

        // GET: ApplicationSecurables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationSecurables = await _context.ApplicationSecurables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationSecurables == null)
            {
                return NotFound();
            }

            return View(applicationSecurables);
        }

        // POST: ApplicationSecurables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationSecurables = await _context.ApplicationSecurables.FindAsync(id);
            if (applicationSecurables != null)
            {
                _context.ApplicationSecurables.Remove(applicationSecurables);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteApplicationSecurable,
                    EntityName = EntityConstants.ApplicationSecurable,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted application securable '{applicationSecurables.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(applicationSecurables),
                    RecordId = applicationSecurables.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationSecurablesExists(int id)
        {
            return _context.ApplicationSecurables.Any(e => e.Id == id);
        }
    }
}
