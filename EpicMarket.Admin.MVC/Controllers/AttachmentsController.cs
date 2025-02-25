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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService _attachmentService;
        private readonly IFileService _fileService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public AttachmentsController(
            IAttachmentService attachmentService,
            IFileService fileService,
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _attachmentService = attachmentService;
            _fileService = fileService;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Attachments
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Attachments;
            return View(await authDbContext.ToListAsync());
        }

        // GET: Attachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        public async Task<IActionResult> DownloadImage(string key)
        {
            try
            {
                var fileDto = await _fileService.GetFileByKeyAsync(key);

                // Set the content disposition to attachment to prompt download in the browser
                var result = new FileStreamResult(fileDto.fileStream, fileDto.contentType)
                {
                    FileDownloadName = key
                };

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while downloading the image.");
            }
        }

        // GET: Attachments/Create
        public IActionResult Create()
        {
            ViewData["AttachmentTypeID"] = new SelectList(_context.AttachmentTypes, "ID", "Name");
            return View();
        }

        // POST: Attachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AttachmentTypeID,Name,Comment,DocumentType,DocumentFileType,DocumentFolderPath,DocumentFile,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Attachment attachment)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            attachment.CreateBy = userName;
            attachment.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(attachment);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddAttachment,
                    EntityName = EntityConstants.Attachment,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added attachment '{attachment.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(attachment),
                    RecordId = attachment.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(attachment);
        }

        // GET: Attachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            return View(attachment);
        }

        // POST: Attachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AttachmentTypeID,Name,Comment,DocumentType,DocumentFileType,DocumentFolderPath,DocumentFile,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Attachment attachment)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            attachment.ModifiedBy = userName;
            attachment.ModifiedDate = DateTime.UtcNow;
            if (id != attachment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.Attachments.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(attachment);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAttachment,
                        EntityName = EntityConstants.Attachment,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated attachment '{attachment.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = attachment
                        }),
                        RecordId = attachment.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentExists(attachment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(attachment);
        }

        // GET: Attachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(m => m.ID == id);
            var key = attachment.DocumentFolderPath + attachment.DocumentFile;
            await _fileService.DeleteImage([key], userName);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteAttachment,
                    EntityName = EntityConstants.Attachment,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted attachment '{attachment.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(attachment),
                    RecordId = attachment.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentExists(int id)
        {
            return _context.Attachments.Any(e => e.ID == id);
        }
    }
}
