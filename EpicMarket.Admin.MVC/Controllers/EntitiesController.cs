using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Data.ApplicationModels;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class EntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public EntitiesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Entities
        [SecurableAuthorize(SecurableConstants.EntitiesView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Entity.ToListAsync());
        }

        // GET: Entities/Details/5
        [SecurableAuthorize(SecurableConstants.EntitiesView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Entities/Create
        [SecurableAuthorize(SecurableConstants.EntitiesAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.EntitiesAdd)]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Entity entity)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            entity.CreateBy = userName;
            entity.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddEntity,
                    EntityName = EntityConstants.Entity,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added entity '{entity.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(entity),
                    RecordId = entity.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        // GET: Entities/Edit/5
        [SecurableAuthorize(SecurableConstants.EntitiesEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.EntitiesEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Entity entity)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            entity.ModifiedBy = userName;
            entity.ModifiedDate = DateTime.UtcNow;
            if (id != entity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.Entity.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(entity);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditEntity,
                        EntityName = EntityConstants.Entity,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated entity '{entity.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = entity
                        }),
                        RecordId = entity.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(entity);
        }

        // GET: Entities/Delete/5
        [SecurableAuthorize(SecurableConstants.EntitiesDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.EntitiesDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.Entity.FindAsync(id);
            if (entity != null)
            {
                _context.Entity.Remove(entity);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteEntity,
                    EntityName = EntityConstants.Entity,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted entity '{entity.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(entity),
                    RecordId = entity.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EntityExists(int id)
        {
            return _context.Entity.Any(e => e.ID == id);
        }
    }
}
