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
        public IActionResult Index()
        {
            // Load dropdown data for filters
            ViewBag.ContactMethods = _context.ContactMethod
                .Select(c => new SelectListItem { Value = c.ID.ToString(), Text = c.Name })
                .ToList();

            ViewBag.CommunicationStatuses = _context.CommunicationStatus
                .Select(s => new SelectListItem { Value = s.ID.ToString(), Text = s.Name })
                .ToList();

            return View();
        }

        // GET: Server-side rendering endpoint for DataTables
        [HttpPost]
        [Route("CommunicationQueues/GetFilteredData")]
        [SecurableAuthorize(SecurableConstants.CommunicationQueuesView)]
        public async Task<IActionResult> GetFilteredData([FromBody] CommunicationQueueFilterViewModel filter)
        {
            try
            {
                // Start with base query
                var query = _context.CommunicationQueue
                    .Include(c => c.ContactMethod)
                    .Include(c => c.CommunicationStatus)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.Subject))
                {
                    query = query.Where(c => c.Subject.Contains(filter.Subject));
                }

                if (!string.IsNullOrWhiteSpace(filter.Recipient))
                {
                    query = query.Where(c => c.NotificationRecipient.Contains(filter.Recipient));
                }

                if (filter.ContactMethodId.HasValue)
                {
                    query = query.Where(c => c.ContactMethodID == filter.ContactMethodId);
                }

                if (filter.CommunicationStatusId.HasValue)
                {
                    query = query.Where(c => c.CommunicationStatusId == filter.CommunicationStatusId);
                }

                if (filter.ScheduledDateFrom.HasValue)
                {
                    query = query.Where(c => c.ScheduledDate >= filter.ScheduledDateFrom);
                }

                if (filter.ScheduledDateTo.HasValue)
                {
                    // Add one day to include the entire end date
                    var endDate = filter.ScheduledDateTo.Value.AddDays(1);
                    query = query.Where(c => c.ScheduledDate < endDate);
                }

                // Get total count for pagination
                var totalCount = await query.CountAsync();

                // Apply sorting
                if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
                {
                    if (filter.SortColumn == "subject")
                    {
                        query = filter.SortDirection.ToLower() == "asc" 
                            ? query.OrderBy(c => c.Subject) 
                            : query.OrderByDescending(c => c.Subject);
                    }
                    else if (filter.SortColumn == "recipient")
                    {
                        query = filter.SortDirection.ToLower() == "asc" 
                            ? query.OrderBy(c => c.NotificationRecipient) 
                            : query.OrderByDescending(c => c.NotificationRecipient);
                    }
                    else if (filter.SortColumn == "retryCount")
                    {
                        query = filter.SortDirection.ToLower() == "asc" 
                            ? query.OrderBy(c => c.RetryCount) 
                            : query.OrderByDescending(c => c.RetryCount);
                    }
                    else if (filter.SortColumn == "scheduledDate")
                    {
                        query = filter.SortDirection.ToLower() == "asc" 
                            ? query.OrderBy(c => c.ScheduledDate) 
                            : query.OrderByDescending(c => c.ScheduledDate);
                    }
                    else if (filter.SortColumn == "sentDate")
                    {
                        query = filter.SortDirection.ToLower() == "asc" 
                            ? query.OrderBy(c => c.SentDate) 
                            : query.OrderByDescending(c => c.SentDate);
                    }
                    else if (filter.SortColumn == "createDate")
                    {
                        query = filter.SortDirection.ToLower() == "asc" 
                            ? query.OrderBy(c => c.CreateDate) 
                            : query.OrderByDescending(c => c.CreateDate);
                    }
                    else
                    {
                        // Default sort
                        query = query.OrderByDescending(c => c.ScheduledDate ?? c.CreateDate);
                    }
                }
                else
                {
                    // Default sort
                    query = query.OrderByDescending(c => c.ScheduledDate ?? c.CreateDate);
                }

                // Apply pagination
                var pagedData = await query
                    .Skip(filter.Start)
                    .Take(filter.Length)
                    .ToListAsync();

                // Format the data for DataTables
                var result = new
                {
                    draw = filter.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = pagedData.Select(c => new
                    {
                        id = c.ID,
                        subject = c.Subject ?? "N/A",
                        recipient = c.NotificationRecipient ?? "N/A",
                        contactMethod = c.ContactMethod != null ? c.ContactMethod.Name : "N/A",
                        status = c.CommunicationStatus != null ? c.CommunicationStatus.Name : "Pending",
                        statusClass = GetStatusBadgeColor(c.CommunicationStatus?.Name),
                        retryCount = c.RetryCount,
                        scheduledDate = c.ScheduledDate.HasValue ? c.ScheduledDate.Value.ToString("dd MMM yyyy HH:mm") : "N/A",
                        sentDate = c.SentDate.HasValue ? c.SentDate.Value.ToString("dd MMM yyyy HH:mm") : "N/A",
                        createDate = c.CreateDate.ToString("dd MMM yyyy HH:mm")
                    })
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new
                {
                    draw = filter.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = ex.Message
                });
            }
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
                .Include(c => c.CommunicationStatus)
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
                .Include(c => c.CommunicationStatus)
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

        private string GetStatusBadgeColor(string status)
        {
            if (string.IsNullOrEmpty(status))
                return "secondary";
                
            return status.ToLower() switch
            {
                "sent" => "success",
                "delivered" => "success",
                "failed" => "danger",
                "pending" => "warning",
                "processing" => "info",
                "retry" => "primary",
                "cancelled" => "dark",
                _ => "secondary"
            };
        }
    }

    // View model for filtering communication queues
    public class CommunicationQueueFilterViewModel
    {
        // DataTables parameters
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

        // Custom filter parameters
        public string Subject { get; set; }
        public string Recipient { get; set; }
        public int? ContactMethodId { get; set; }
        public int? CommunicationStatusId { get; set; }
        public DateTime? ScheduledDateFrom { get; set; }
        public DateTime? ScheduledDateTo { get; set; }
    }
}
