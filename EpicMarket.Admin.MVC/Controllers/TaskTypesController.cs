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
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class TaskTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public TaskTypesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: TaskTypes
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.TaskTypes.Include(t => t.EventCategorys);
            return View(await authDbContext.ToListAsync());
        }

        // GET: TaskTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _context.TaskTypes
                .Include(t => t.EventCategorys)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // GET: TaskTypes/Create
        public IActionResult Create()
        {
            ViewData["TaskCategoryID"] = new SelectList(_context.Set<ApplicationsTable>(), "ID", "Name");
            return View();
        }

        // POST: TaskTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,TaskCategoryID,DefaultDueDateHours,ShortDescription,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] TaskType taskType)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            taskType.CreateBy = userName;
            taskType.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(taskType);
                await _context.SaveChangesAsync();
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddTaskType,
                    EntityName = EntityConstants.TaskType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added task type '{taskType.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(taskType),
                    RecordId = taskType.ID,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskCategoryID"] = new SelectList(_context.Set<ApplicationsTable>(), "ID", "Name", taskType.TaskCategoryID);
            return View(taskType);
        }

        // GET: TaskTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _context.TaskTypes.FindAsync(id);
            if (taskType == null)
            {
                return NotFound();
            }
            ViewData["TaskCategoryID"] = new SelectList(_context.Set<ApplicationsTable>(), "ID", "Name", taskType.TaskCategoryID);
            return View(taskType);
        }

        // POST: TaskTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,TaskCategoryID,DefaultDueDateHours,ShortDescription,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] TaskType taskType)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            taskType.ModifiedBy = userName;
            taskType.ModifiedDate = DateTime.UtcNow;
            
            if (id != taskType.ID)
            {
                return NotFound();
            }

            // Get the original entity for comparison
            var originalTaskType = await _context.TaskTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskType);
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditTaskType,
                        EntityName = EntityConstants.TaskType,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated task type '{taskType.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            Original = originalTaskType, 
                            Updated = taskType 
                        }),
                        RecordId = taskType.ID,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTypeExists(taskType.ID))
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
            ViewData["TaskCategoryID"] = new SelectList(_context.Set<ApplicationsTable>(), "ID", "Name", taskType.TaskCategoryID);
            return View(taskType);
        }

        // GET: TaskTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _context.TaskTypes
                .Include(t => t.EventCategorys)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // POST: TaskTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskType = await _context.TaskTypes.FindAsync(id);
            if (taskType != null)
            {
                _context.TaskTypes.Remove(taskType);
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteTaskType,
                    EntityName = EntityConstants.TaskType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted task type '{taskType.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(taskType),
                    RecordId = taskType.ID,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskTypeExists(int id)
        {
            return _context.TaskTypes.Any(e => e.ID == id);
        }
    }
}
