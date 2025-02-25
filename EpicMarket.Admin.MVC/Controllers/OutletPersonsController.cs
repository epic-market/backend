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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using System.Text.Json;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class OutletPersonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public OutletPersonsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: OutletPersons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OutletPeople.Include(o => o.Outlet).Include(o => o.Outlet.Bussiness).Include(o => o.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OutletPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletPerson = await _context.OutletPeople
                .Include(o => o.Outlet)
                .Include(o => o.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outletPerson == null)
            {
                return NotFound();
            }

            return View(outletPerson);
        }

        // GET: OutletPersons/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID");
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: OutletPersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PersonId,OutletId")] OutletPerson outletPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outletPerson);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CREATE,
                    EntityName = EntityConstants.OutletPerson,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created outlet person association with ID: {outletPerson.ID}",
                    Data = JsonSerializer.Serialize(outletPerson),
                    RecordId = outletPerson.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID", outletPerson.PersonId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", outletPerson.PersonId);
            return View(outletPerson);
        }

        // GET: OutletPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletPerson = await _context.OutletPeople.FindAsync(id);
            if (outletPerson == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID", outletPerson.PersonId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", outletPerson.PersonId);
            return View(outletPerson);
        }

        // POST: OutletPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonId,OutletId")] OutletPerson outletPerson)
        {
            if (id != outletPerson.ID)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.OutletPeople
                .AsNoTracking()
                .FirstOrDefaultAsync(op => op.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outletPerson);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.UPDATE,
                        EntityName = EntityConstants.OutletPerson,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated outlet person association with ID: {outletPerson.ID}",
                        Data = JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = outletPerson
                        }),
                        RecordId = outletPerson.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutletPersonExists(outletPerson.ID))
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
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID", outletPerson.PersonId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", outletPerson.PersonId);
            return View(outletPerson);
        }

        // GET: OutletPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletPerson = await _context.OutletPeople
                .Include(o => o.Outlet)
                .Include(o => o.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outletPerson == null)
            {
                return NotFound();
            }

            return View(outletPerson);
        }

        // POST: OutletPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outletPerson = await _context.OutletPeople.FindAsync(id);
            if (outletPerson != null)
            {
                _context.OutletPeople.Remove(outletPerson);
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DELETE,
                    EntityName = EntityConstants.OutletPerson,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted outlet person association with ID: {outletPerson.ID}",
                    Data = JsonSerializer.Serialize(outletPerson),
                    RecordId = outletPerson.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutletPersonExists(int id)
        {
            return _context.OutletPeople.Any(e => e.ID == id);
        }
    }
}
