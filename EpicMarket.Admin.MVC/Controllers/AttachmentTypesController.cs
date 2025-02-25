using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class AttachmentTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public AttachmentTypesController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: AttachmentTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AttachmentTypes.ToListAsync());
        }

        // GET: AttachmentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachmentType = await _context.AttachmentTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachmentType == null)
            {
                return NotFound();
            }

            return View(attachmentType);
        }

        // GET: AttachmentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AttachmentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] AttachmentType attachmentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attachmentType);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddAttachmentType,
                    EntityName = EntityConstants.AttachmentType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added attachment type '{attachmentType.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(attachmentType),
                    RecordId = attachmentType.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(attachmentType);
        }

        // GET: AttachmentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachmentType = await _context.AttachmentTypes.FindAsync(id);
            if (attachmentType == null)
            {
                return NotFound();
            }
            return View(attachmentType);
        }

        // POST: AttachmentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] AttachmentType attachmentType)
        {
            if (id != attachmentType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.AttachmentTypes.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(attachmentType);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAttachmentType,
                        EntityName = EntityConstants.AttachmentType,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated attachment type '{attachmentType.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = attachmentType
                        }),
                        RecordId = attachmentType.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentTypeExists(attachmentType.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(attachmentType);
        }

        // GET: AttachmentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachmentType = await _context.AttachmentTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachmentType == null)
            {
                return NotFound();
            }

            return View(attachmentType);
        }

        // POST: AttachmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attachmentType = await _context.AttachmentTypes.FindAsync(id);
            if (attachmentType != null)
            {
                _context.AttachmentTypes.Remove(attachmentType);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteAttachmentType,
                    EntityName = EntityConstants.AttachmentType,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted attachment type '{attachmentType.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(attachmentType),
                    RecordId = attachmentType.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentTypeExists(int id)
        {
            return _context.AttachmentTypes.Any(e => e.ID == id);
        }
    }
}
