using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Authorization;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class TaskStatusTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public TaskStatusTypesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: TaskStatusTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaskStatusTypes.ToListAsync());
        }

        // GET: TaskStatusTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskStatusType = await _context.TaskStatusTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskStatusType == null)
            {
                return NotFound();
            }

            return View(taskStatusType);
        }

        // GET: TaskStatusTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskStatusTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,StatusDescription,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] TaskStatusType taskStatusType)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            taskStatusType.CreateBy = userName;
            taskStatusType.CreateDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(taskStatusType);
                await _context.SaveChangesAsync();
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddTaskStatusType,
                    EntityName = EntityConstants.TaskStatusType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added task status type '{taskStatusType.Status}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(taskStatusType),
                    RecordId = taskStatusType.ID,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            return View(taskStatusType);
        }

        // GET: TaskStatusTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskStatusType = await _context.TaskStatusTypes.FindAsync(id);
            if (taskStatusType == null)
            {
                return NotFound();
            }
            return View(taskStatusType);
        }

        // POST: TaskStatusTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,StatusDescription,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] TaskStatusType taskStatusType)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            taskStatusType.ModifiedBy = userName;
            taskStatusType.ModifiedDate = DateTime.UtcNow;
            
            if (id != taskStatusType.ID)
            {
                return NotFound();
            }

            // Get the original entity for comparison
            var originalTaskStatusType = await _context.TaskStatusTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskStatusType);
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditTaskStatusType,
                        EntityName = EntityConstants.TaskStatusType,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated task status type '{taskStatusType.Status}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            Original = originalTaskStatusType, 
                            Updated = taskStatusType 
                        }),
                        RecordId = taskStatusType.ID,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskStatusTypeExists(taskStatusType.ID))
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
            return View(taskStatusType);
        }

        // GET: TaskStatusTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskStatusType = await _context.TaskStatusTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskStatusType == null)
            {
                return NotFound();
            }

            return View(taskStatusType);
        }

        // POST: TaskStatusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskStatusType = await _context.TaskStatusTypes.FindAsync(id);
            if (taskStatusType != null)
            {
                _context.TaskStatusTypes.Remove(taskStatusType);
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteTaskStatusType,
                    EntityName = EntityConstants.TaskStatusType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted task status type '{taskStatusType.Status}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(taskStatusType),
                    RecordId = taskStatusType.ID,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskStatusTypeExists(int id)
        {
            return _context.TaskStatusTypes.Any(e => e.ID == id);
        }
    }
}
