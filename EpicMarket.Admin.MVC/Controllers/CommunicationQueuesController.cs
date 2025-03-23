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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
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
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesView)]
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.CommunicationQueue.Include(c => c.ContactMethod);
            return View(await authDbContext.ToListAsync());
        }

        // GET: CommunicationQueues/Details/5
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesView)]
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
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CommunicationQueues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesAdd)]
        public async Task<IActionResult> Create([Bind("ID,Type,To,CC,BCC,Subject,Body,Status,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] CommunicationQueue communicationQueue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(communicationQueue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(communicationQueue);
        }

        // GET: CommunicationQueues/Delete/5
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesDelete)]
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
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesDelete)]
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
