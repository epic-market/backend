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
    public class CommunicationQueuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunicationQueuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CommunicationQueues
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.CommunicationQueue.Include(c => c.ContactMethod);
            return View(await authDbContext.ToListAsync());
        }

        // GET: CommunicationQueues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communicationQueue = await _context.CommunicationQueue
                .Include(c => c.ContactMethod)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (communicationQueue == null)
            {
                return NotFound();
            }

            return View(communicationQueue);
        }

        // GET: CommunicationQueues/Create
        public IActionResult Create()
        {
            ViewData["ContactMethodID"] = new SelectList(_context.Set<ContactMethod>(), "ID", "ID");
            return View();
        }

        // POST: CommunicationQueues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ContactMethodID,MessageData,Subject,MessageText,Attempts,ScheduledDate,NotificationRecipient,SysStartTime,SysEndTime,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] CommunicationQueue communicationQueue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(communicationQueue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactMethodID"] = new SelectList(_context.Set<ContactMethod>(), "ID", "ID", communicationQueue.ContactMethodID);
            return View(communicationQueue);
        }

        // GET: CommunicationQueues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communicationQueue = await _context.CommunicationQueue.FindAsync(id);
            if (communicationQueue == null)
            {
                return NotFound();
            }
            ViewData["ContactMethodID"] = new SelectList(_context.Set<ContactMethod>(), "ID", "ID", communicationQueue.ContactMethodID);
            return View(communicationQueue);
        }

        // POST: CommunicationQueues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ContactMethodID,MessageData,Subject,MessageText,Attempts,ScheduledDate,NotificationRecipient,SysStartTime,SysEndTime,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] CommunicationQueue communicationQueue)
        {
            if (id != communicationQueue.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(communicationQueue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunicationQueueExists(communicationQueue.ID))
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
            ViewData["ContactMethodID"] = new SelectList(_context.Set<ContactMethod>(), "ID", "ID", communicationQueue.ContactMethodID);
            return View(communicationQueue);
        }

        // GET: CommunicationQueues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communicationQueue = await _context.CommunicationQueue
                .Include(c => c.ContactMethod)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (communicationQueue == null)
            {
                return NotFound();
            }

            return View(communicationQueue);
        }

        // POST: CommunicationQueues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var communicationQueue = await _context.CommunicationQueue.FindAsync(id);
            if (communicationQueue != null)
            {
                _context.CommunicationQueue.Remove(communicationQueue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunicationQueueExists(int id)
        {
            return _context.CommunicationQueue.Any(e => e.ID == id);
        }
    }
}
