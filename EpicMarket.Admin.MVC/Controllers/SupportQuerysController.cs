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
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.SUPPORT},{ROLES.ROOT},{ROLES.ADMIN}")]
    public class SupportQuerysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public SupportQuerysController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: SupportQuerys
        public async Task<IActionResult> Index()
        {
            ViewBag.PersonTypes = await _context.PersonTypes.ToListAsync();
            ViewBag.TaskTypes = await _context.TaskTypes.ToListAsync();
            
            // Return the view with empty model - data will be loaded via AJAX
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredData([FromBody] SupportQueryFilterModel filter)
        {
            try
            {
                // Start with the base query
                var query = _context.SupportQueries
                    .Include(s => s.PersonType)
                    .Include(s => s.TaskTypes)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.Query))
                {
                    query = query.Where(s => s.Query.Contains(filter.Query));
                }

                if (filter.PersonTypeId.HasValue)
                {
                    query = query.Where(s => s.TypeofPersonid == filter.PersonTypeId);
                }

                if (filter.TaskTypeId.HasValue)
                {
                    query = query.Where(s => s.TaskTypeID == filter.TaskTypeId);
                }

                // Get total count for pagination
                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = ApplySorting(query, filter.SortColumn, filter.SortDirection);

                // Apply pagination and project to DTO to avoid circular references
                var supportQueries = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(s => new SupportQueryViewModel
                    {
                        ID = s.ID,
                        Query = s.Query,
                        PersonTypeName = s.PersonType.Type,
                        TypeofPersonid = s.TypeofPersonid,
                        TaskTypeName = s.TaskTypes.Name,
                        TaskTypeID = s.TaskTypeID,
                        CreateDate = s.CreateDate,
                        CreateBy = s.CreateBy
                    })
                    .ToListAsync();

                return Json(new
                {
                    data = supportQueries,
                    totalRecords
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private IQueryable<SupportQueries> ApplySorting(IQueryable<SupportQueries> query, string sortColumn, string sortDirection)
        {
            // Default sort by ID if column not specified
            if (string.IsNullOrEmpty(sortColumn))
            {
                return sortDirection == "desc" ? query.OrderByDescending(s => s.ID) : query.OrderBy(s => s.ID);
            }

            // Apply sorting based on the column
            return sortColumn.ToLower() switch
            {
                "query" => sortDirection == "desc" ? query.OrderByDescending(s => s.Query) : query.OrderBy(s => s.Query),
                "persontype" => sortDirection == "desc" ? query.OrderByDescending(s => s.PersonType.Type) : query.OrderBy(s => s.PersonType.Type),
                "tasktype" => sortDirection == "desc" ? query.OrderByDescending(s => s.TaskTypes.Name) : query.OrderBy(s => s.TaskTypes.Name),
                "createdate" => sortDirection == "desc" ? query.OrderByDescending(s => s.CreateDate) : query.OrderBy(s => s.CreateDate),
                _ => sortDirection == "desc" ? query.OrderByDescending(s => s.ID) : query.OrderBy(s => s.ID)
            };
        }

        // GET: SupportQuerys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportQuerys = await _context.SupportQueries
                .Include(s => s.PersonType)
                .Include(s => s.TaskTypes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supportQuerys == null)
            {
                return NotFound();
            }

            return View(supportQuerys);
        }

        // GET: SupportQuerys/Create
        public IActionResult Create()
        {
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type");
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name");
            return View();
        }

        // POST: SupportQuerys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Query,TaskTypeID,TypeofPersonid,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] SupportQueries supportQuerys)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            supportQuerys.CreateBy = userName;
            supportQuerys.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(supportQuerys);
                await _context.SaveChangesAsync();
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddSupportQuery,
                    EntityName = EntityConstants.SupportQuery,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added support query '{supportQuerys.Query}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(supportQuerys),
                    RecordId = supportQuerys.ID,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type", supportQuerys.TypeofPersonid);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", supportQuerys.TaskTypeID);
            return View(supportQuerys);
        }

        // GET: SupportQuerys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportQuerys = await _context.SupportQueries.FindAsync(id);
            if (supportQuerys == null)
            {
                return NotFound();
            }
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type", supportQuerys.TypeofPersonid);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", supportQuerys.TaskTypeID);
            return View(supportQuerys);
        }

        // POST: SupportQuerys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Query,TaskTypeID,TypeofPersonid,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] SupportQueries supportQuerys)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            supportQuerys.ModifiedBy = userName;
            supportQuerys.ModifiedDate = DateTime.UtcNow;
            
            if (id != supportQuerys.ID)
            {
                return NotFound();
            }

            // Get the original entity for comparison
            var originalSupportQuery = await _context.SupportQueries
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportQuerys);
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditSupportQuery,
                        EntityName = EntityConstants.SupportQuery,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated support query '{supportQuerys.Query}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            Original = originalSupportQuery, 
                            Updated = supportQuerys 
                        }),
                        RecordId = supportQuerys.ID,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportQuerysExists(supportQuerys.ID))
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
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type", supportQuerys.TypeofPersonid);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", supportQuerys.TaskTypeID);
            return View(supportQuerys);
        }

        // GET: SupportQuerys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportQuerys = await _context.SupportQueries
                .Include(s => s.PersonType)
                .Include(s => s.TaskTypes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supportQuerys == null)
            {
                return NotFound();
            }

            return View(supportQuerys);
        }

        // POST: SupportQuerys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportQuerys = await _context.SupportQueries.FindAsync(id);
            if (supportQuerys != null)
            {
                _context.SupportQueries.Remove(supportQuerys);
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteSupportQuery,
                    EntityName = EntityConstants.SupportQuery,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted support query '{supportQuerys.Query}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(supportQuerys),
                    RecordId = supportQuerys.ID,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportQuerysExists(int id)
        {
            return _context.SupportQueries.Any(e => e.ID == id);
        }
    }
}
