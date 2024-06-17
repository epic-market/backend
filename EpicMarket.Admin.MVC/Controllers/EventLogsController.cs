using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class EventLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventLogs
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.EventLog.Include(e => e.Entity).Include(e => e.Event);
            return View(await authDbContext.ToListAsync());
        }

        // GET: EventLogs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLog = await _context.EventLog
                .Include(e => e.Entity)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventLog == null)
            {
                return NotFound();
            }

            return View(eventLog);
        }

        // GET: EventLogs/Create
        public IActionResult Create()
        {
            ViewData["EntityID"] = new SelectList(_context.Set<Entity>(), "ID", "ID");
            ViewData["EventID"] = new SelectList(_context.Set<Event>(), "ID", "ID");
            return View();
        }

        // POST: EventLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,EventID,EntityID,RecordID,Source,Description,Data,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] EventLog eventLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntityID"] = new SelectList(_context.Set<Entity>(), "ID", "ID", eventLog.EntityID);
            ViewData["EventID"] = new SelectList(_context.Set<Event>(), "ID", "ID", eventLog.EventID);
            return View(eventLog);
        }

        // GET: EventLogs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLog = await _context.EventLog.FindAsync(id);
            if (eventLog == null)
            {
                return NotFound();
            }
            ViewData["EntityID"] = new SelectList(_context.Set<Entity>(), "ID", "ID", eventLog.EntityID);
            ViewData["EventID"] = new SelectList(_context.Set<Event>(), "ID", "ID", eventLog.EventID);
            return View(eventLog);
        }

        // POST: EventLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,EventID,EntityID,RecordID,Source,Description,Data,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] EventLog eventLog)
        {
            if (id != eventLog.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventLogExists(eventLog.ID))
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
            ViewData["EntityID"] = new SelectList(_context.Set<Entity>(), "ID", "ID", eventLog.EntityID);
            ViewData["EventID"] = new SelectList(_context.Set<Event>(), "ID", "ID", eventLog.EventID);
            return View(eventLog);
        }

        // GET: EventLogs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLog = await _context.EventLog
                .Include(e => e.Entity)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventLog == null)
            {
                return NotFound();
            }

            return View(eventLog);
        }

        // POST: EventLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var eventLog = await _context.EventLog.FindAsync(id);
            if (eventLog != null)
            {
                _context.EventLog.Remove(eventLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventLogExists(long id)
        {
            return _context.EventLog.Any(e => e.ID == id);
        }
    }
}
