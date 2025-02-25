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

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
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
        public async Task<IActionResult> Index(bool IsActive = true)
        {
            return View();
        }

        [HttpPost]
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationConfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Value,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationConfiguration applicationConfiguration)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Value,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ApplicationConfiguration applicationConfiguration)
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
}
