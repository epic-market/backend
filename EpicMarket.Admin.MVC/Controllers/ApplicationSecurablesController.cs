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
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
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
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesView)]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [Route("ApplicationSecurables/GetFilteredData")]
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesView)]
        public async Task<IActionResult> GetFilteredData([FromBody] SecurableFilterViewModel filter)
        {
            try
            {
                var query = _context.ApplicationSecurables.AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.SecurableId))
                {
                    query = query.Where(s => s.Id.ToString().Contains(filter.SecurableId));
                }

                if (!string.IsNullOrWhiteSpace(filter.SecurableName))
                {
                    query = query.Where(s => s.Name.Contains(filter.SecurableName));
                }

                if (!string.IsNullOrWhiteSpace(filter.Description))
                {
                    query = query.Where(s => s.Description.Contains(filter.Description));
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(s => s.Id) : query.OrderByDescending(s => s.Id),
                    "name" => filter.SortDirection == "asc" ? query.OrderBy(s => s.Name) : query.OrderByDescending(s => s.Name),
                    "description" => filter.SortDirection == "asc" ? query.OrderBy(s => s.Description) : query.OrderByDescending(s => s.Description),
                    _ => query.OrderBy(s => s.Id)
                };

                // Apply pagination and project to DTO
                var securables = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(s => new SecurableDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = securables });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: ApplicationSecurables/Details/5
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesView)]
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
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationSecurables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesAdd)]
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
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesEdit)]
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
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesEdit)]
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
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesDelete)]
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
        [SecurableAuthorize(SecurableConstants.ApplicationSecurablesDelete)]
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

    public class SecurableFilterViewModel
    {
        public string SecurableId { get; set; }
        public string SecurableName { get; set; }
        public string Description { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "id";
        public string SortDirection { get; set; } = "asc";
    }

    public class SecurableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
