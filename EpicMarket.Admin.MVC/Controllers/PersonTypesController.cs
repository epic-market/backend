using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using System.Text.Json;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EpicMarket.Admin.MVC.Controllers
{
     [Authorize(Roles = $"{ROLES.ROOT}")]
    public class PersonTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public PersonTypesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: PersonTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PersonTypes.ToListAsync());
        }

        // GET: PersonTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personType = await _context.PersonTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (personType == null)
            {
                return NotFound();
            }

            return View(personType);
        }

        // GET: PersonTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] PersonType personType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personType);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CREATE,
                    EntityName = EntityConstants.PersonType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created person type: {personType.Type}",
                    Data = JsonSerializer.Serialize(personType),
                    RecordId = personType.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            return View(personType);
        }

        // GET: PersonTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personType = await _context.PersonTypes.FindAsync(id);
            if (personType == null)
            {
                return NotFound();
            }
            return View(personType);
        }

        // POST: PersonTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] PersonType personType)
        {
            if (id != personType.ID)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.PersonTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personType);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.UPDATE,
                        EntityName = EntityConstants.PersonType,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated person type: {personType.Type}",
                        Data = JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = personType
                        }),
                        RecordId = personType.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonTypeExists(personType.ID))
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
            return View(personType);
        }

        // GET: PersonTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personType = await _context.PersonTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (personType == null)
            {
                return NotFound();
            }

            return View(personType);
        }

        // POST: PersonTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personType = await _context.PersonTypes.FindAsync(id);
            if (personType != null)
            {
                _context.PersonTypes.Remove(personType);
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DELETE,
                    EntityName = EntityConstants.PersonType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted person type: {personType.Type}",
                    Data = JsonSerializer.Serialize(personType),
                    RecordId = personType.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonTypeExists(int id)
        {
            return _context.PersonTypes.Any(e => e.ID == id);
        }
    }
}
