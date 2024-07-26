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

namespace EpicMarket.Admin.MVC.Controllers
{
    public class TaskTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskTypesController(ApplicationDbContext context)
        {
            _context = context;
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskType);
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
