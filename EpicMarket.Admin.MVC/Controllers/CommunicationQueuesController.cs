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
using EpicMarket.Entities.CustomModels;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class CommunicationQueuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public CommunicationQueuesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
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

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteCommunicationQueue,
                    EntityName = EntityConstants.CommunicationQueue,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted communication queue ID: {communicationQueue.ID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(communicationQueue),
                    RecordId = communicationQueue.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CommunicationQueueExists(int id)
        {
            return _context.CommunicationQueue.Any(e => e.ID == id);
        }
    }
}
