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
using EpicMarket.Entities.CustomModels;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class ApplicationsTablesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public ApplicationsTablesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: ApplicationsTables
        [SecurableAuthorize(SecurableConstants.ApplicationTablesView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationsTable.ToListAsync());
        }

        // GET: ApplicationsTables/Details/5
        [SecurableAuthorize(SecurableConstants.ApplicationTablesView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationsTable = await _context.ApplicationsTable
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationsTable == null)
            {
                return NotFound();
            }

            return View(applicationsTable);
        }

        // GET: ApplicationsTables/Create
        [SecurableAuthorize(SecurableConstants.ApplicationTablesAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationsTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationTablesAdd)]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Sequence,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationsTable applicationsTable)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            applicationsTable.CreateBy = userName;
            applicationsTable.CreateDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(applicationsTable);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddApplicationTable,
                    EntityName = EntityConstants.ApplicationTable,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added application table '{applicationsTable.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(applicationsTable),
                    RecordId = applicationsTable.ID,
                    BusinessID = 0,
                    LoggedInUserName = userName
                });

                return RedirectToAction(nameof(Index));
            }
            return View(applicationsTable);
        }

        // GET: ApplicationsTables/Edit/5
        [SecurableAuthorize(SecurableConstants.ApplicationTablesEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationsTable = await _context.ApplicationsTable.FindAsync(id);
            if (applicationsTable == null)
            {
                return NotFound();
            }
            return View(applicationsTable);
        }

        // POST: ApplicationsTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationTablesEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Sequence,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ApplicationsTable applicationsTable)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            applicationsTable.ModifiedBy = userName;
            applicationsTable.ModifiedDate = DateTime.UtcNow;

            if (id != applicationsTable.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.ApplicationsTable.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(applicationsTable);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditApplicationTable,
                        EntityName = EntityConstants.ApplicationTable,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated application table '{applicationsTable.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = applicationsTable
                        }),
                        RecordId = applicationsTable.ID,
                        BusinessID = 0,
                        LoggedInUserName = userName
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationsTableExists(applicationsTable.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(applicationsTable);
        }

        // GET: ApplicationsTables/Delete/5
        [SecurableAuthorize(SecurableConstants.ApplicationTablesDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationsTable = await _context.ApplicationsTable
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationsTable == null)
            {
                return NotFound();
            }

            return View(applicationsTable);
        }

        // POST: ApplicationsTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationTablesDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationsTable = await _context.ApplicationsTable.FindAsync(id);
            if (applicationsTable != null)
            {
                _context.ApplicationsTable.Remove(applicationsTable);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteApplicationTable,
                    EntityName = EntityConstants.ApplicationTable,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted application table '{applicationsTable.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(applicationsTable),
                    RecordId = applicationsTable.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationsTableExists(int id)
        {
            return _context.ApplicationsTable.Any(e => e.ID == id);
        }
    }
}
