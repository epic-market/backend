using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Data.ApplicationModels;
using System.Security.Claims;

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
