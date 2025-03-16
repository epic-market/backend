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
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class ApplicationConfigurationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public ApplicationConfigurationsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: ApplicationConfigurations
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsView)]
        public async Task<IActionResult> Index(bool IsActive = true)
        {
            return View();
        }

        [HttpPost]
        [Route("ApplicationConfigurations/GetFilteredData")]
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsView)]
        public async Task<IActionResult> GetFilteredData([FromBody] ConfigFilterViewModel filter)
        {
            try
            {
                var query = _context.ApplicationConfigurations.AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.ConfigId))
                {
                    query = query.Where(c => c.ID.ToString().Contains(filter.ConfigId));
                }

                if (!string.IsNullOrWhiteSpace(filter.ConfigName))
                {
                    query = query.Where(c => c.Name.Contains(filter.ConfigName));
                }

                if (!string.IsNullOrWhiteSpace(filter.Value))
                {
                    query = query.Where(c => c.Value.Contains(filter.Value));
                }

                // Filter by active status if specified
                if (!string.IsNullOrWhiteSpace(filter.IsActive))
                {
                    if (bool.TryParse(filter.IsActive, out bool isActive))
                    {
                        query = query.Where(c => c.IsActive == isActive);
                    }
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(c => c.ID) : query.OrderByDescending(c => c.ID),
                    "name" => filter.SortDirection == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                    "value" => filter.SortDirection == "asc" ? query.OrderBy(c => c.Value) : query.OrderByDescending(c => c.Value),
                    "description" => filter.SortDirection == "asc" ? query.OrderBy(c => c.Description) : query.OrderByDescending(c => c.Description),
                    "isactive" => filter.SortDirection == "asc" ? query.OrderBy(c => c.IsActive) : query.OrderByDescending(c => c.IsActive),
                    _ => query.OrderBy(c => c.ID)
                };

                // Apply pagination and project to DTO
                var configurations = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(c => new ConfigDto
                    {
                        Id = c.ID,
                        Name = c.Name,
                        Value = c.Value,
                        Description = c.Description,
                        IsActive = c.IsActive
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = configurations });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsView)]
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var showActiveOnly = Request.Form["showActiveOnly"].FirstOrDefault() == "true";

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data = _context.ApplicationConfigurations.AsQueryable();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.Name.Contains(searchValue));
                }

                if (showActiveOnly)
                {
                    // Assuming you have an 'IsActive' property in your model
                    data = data.Where(m => m.IsActive == true);
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    if (sortColumnDir.ToLower() == "asc")
                    {
                        data = data.OrderBy(item => EF.Property<object>(item, sortColumn));
                    }
                    else
                    {
                        data = data.OrderByDescending(item => EF.Property<object>(item, sortColumn));
                    }
                }
                recordsTotal = data.Count();
                var dataPage = data.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataPage };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
 

// GET: ApplicationConfigurations/Details/5
[SecurableAuthorize(SecurableConstants.ApplicationConfigurationsView)]
public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationConfiguration = await _context.ApplicationConfigurations
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationConfiguration == null)
            {
                return NotFound();
            }

            return View(applicationConfiguration);
        }

        // GET: ApplicationConfigurations/Create
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationConfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsAdd)]
        public async Task<IActionResult> Create([Bind("ID,Name,Value,Description,IsActive,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationConfiguration applicationConfiguration)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            applicationConfiguration.CreateBy = userName;
            applicationConfiguration.CreateDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(applicationConfiguration);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddApplicationConfiguration,
                    EntityName = EntityConstants.ApplicationConfiguration,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added application configuration '{applicationConfiguration.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(applicationConfiguration),
                    RecordId = applicationConfiguration.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(applicationConfiguration);
        }

        // GET: ApplicationConfigurations/Edit/5
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationConfiguration = await _context.ApplicationConfigurations.FindAsync(id);
            if (applicationConfiguration == null)
            {
                return NotFound();
            }
            return View(applicationConfiguration);
        }

        // POST: ApplicationConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Value,Description,IsActive,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationConfiguration applicationConfiguration)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            applicationConfiguration.ModifiedBy = userName;
            applicationConfiguration.ModifiedDate = DateTime.UtcNow;

            if (id != applicationConfiguration.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.ApplicationConfigurations.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(applicationConfiguration);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditApplicationConfiguration,
                        EntityName = EntityConstants.ApplicationConfiguration,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated application configuration '{applicationConfiguration.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = applicationConfiguration
                        }),
                        RecordId = applicationConfiguration.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationConfigurationExists(applicationConfiguration.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(applicationConfiguration);
        }

        // GET: ApplicationConfigurations/Delete/5
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationConfiguration = await _context.ApplicationConfigurations
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationConfiguration == null)
            {
                return NotFound();
            }

            return View(applicationConfiguration);
        }

        // POST: ApplicationConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ApplicationConfigurationsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationConfiguration = await _context.ApplicationConfigurations.FindAsync(id);
            if (applicationConfiguration != null)
            {
                _context.ApplicationConfigurations.Remove(applicationConfiguration);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteApplicationConfiguration,
                    EntityName = EntityConstants.ApplicationConfiguration,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted application configuration '{applicationConfiguration.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(applicationConfiguration),
                    RecordId = applicationConfiguration.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationConfigurationExists(int id)
        {
            return _context.ApplicationConfigurations.Any(e => e.ID == id);
        }
    }

    public class ConfigFilterViewModel
    {
        public string ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string Value { get; set; }
        public string IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "id";
        public string SortDirection { get; set; } = "asc";
    }

    public class ConfigDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
