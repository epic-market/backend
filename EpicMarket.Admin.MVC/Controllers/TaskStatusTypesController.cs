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

namespace EpicMarket.Admin.MVC.Controllers
{
    public class TaskStatusTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskStatusTypesController(ApplicationDbContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,StatusDescription,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] TaskStatusType taskStatusType)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            taskStatusType.ModifiedBy = userName;
            taskStatusType.ModifiedDate = DateTime.UtcNow;
            if (id != taskStatusType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskStatusType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskStatusTypeExists(taskStatusType.Id))
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskStatusTypeExists(int id)
        {
            return _context.TaskStatusTypes.Any(e => e.Id == id);
        }
    }
}
