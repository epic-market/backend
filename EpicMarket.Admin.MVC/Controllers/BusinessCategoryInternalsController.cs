using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Data.ApplicationModels;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Http;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class BusinessCategoryInternalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;
        private readonly IFileService _fileService;
        private readonly IAttachmentService _attachmentService;

        public BusinessCategoryInternalsController(
            ApplicationDbContext context,
            IEventService eventService,
            IFileService fileService,
            IUrlContextService urlContextService,
            IAttachmentService attachmentService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
            _attachmentService = attachmentService;
            _fileService = fileService;
        }

        // GET: BusinessCategoryInternals
        [SecurableAuthorize(SecurableConstants.BusinessCategoryInternalsView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessCategories.ToListAsync());
        }

        // GET: BusinessCategoryInternals/Details/5
        [SecurableAuthorize(SecurableConstants.BusinessCategoryInternalsView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories.Include(c=>c.Businesses)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            // Get attached images
            var images = await _attachmentService.GetAttachmentLinks(new GetAttachmentLink
            {
                AttachmentType = AttachmentTypeConstants.BUSINESS_CATEGORY,
                Entity = EntityConstants.BusinessCategory,
                RecordID = businessCategoryInternal.ID
            });

            ViewBag.CategoryImages = images;

            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Create
        [SecurableAuthorize(SecurableConstants.BusinessCategoryInternalsAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusinessCategoryInternals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Type,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BusinessCategoryInternal businessCategoryInternal, IFormFile[] files)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            businessCategoryInternal.CreateBy = userName;
            businessCategoryInternal.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(businessCategoryInternal);
                await _context.SaveChangesAsync();

                // Handle file upload if files are present
                if (files != null && files.Length > 0)
                {
                    var attachmentModel = new AttachmentModel
                    {
                        Name = businessCategoryInternal.Name,
                        Comment = "Business Category Image",
                        RecordID = businessCategoryInternal.ID,
                        Entity = EntityConstants.BusinessCategory,
                        AttachmentType = AttachmentTypeConstants.BUSINESS_CATEGORY, // Set appropriate BusinessID if needed
                        Files = files
                    };

                    await _attachmentService.UploadAttachment(attachmentModel);
                }

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddBusinessCategory,
                    EntityName = EntityConstants.BusinessCategory,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added business category '{businessCategoryInternal.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(businessCategoryInternal),
                    RecordId = businessCategoryInternal.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Edit/5
        [SecurableAuthorize(SecurableConstants.BusinessCategoryInternalsEdit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories.FindAsync(id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            // Get attached images
            var images = await _attachmentService.GetAttachmentLinks(new GetAttachmentLink
            {
                AttachmentType = AttachmentTypeConstants.BUSINESS_CATEGORY,
                Entity = EntityConstants.BusinessCategory,
                RecordID = businessCategoryInternal.ID
            });

            ViewBag.CategoryImages = images;

            return View(businessCategoryInternal);
        }

        // POST: BusinessCategoryInternals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Type,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] BusinessCategoryInternal businessCategoryInternal, IFormFile[] files, string removedImages)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            businessCategoryInternal.ModifiedBy = userName;
            businessCategoryInternal.ModifiedDate = DateTime.UtcNow;
            if (id != businessCategoryInternal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.BusinessCategories.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(businessCategoryInternal);
                    await _context.SaveChangesAsync();

                    // Handle file upload if files are present
                    if (files != null && files.Length > 0)
                    {
                        var attachmentModel = new AttachmentModel
                        {
                            Name = businessCategoryInternal.Name,
                            Comment = "Business Category Image",
                            RecordID = businessCategoryInternal.ID,
                            Entity = EntityConstants.BusinessCategory,
                            AttachmentType = AttachmentTypeConstants.BUSINESS_CATEGORY,
                            Files = files
                        };

                        await _attachmentService.UploadAttachment(attachmentModel);
                    }

                    // Handle image removal if any images are selected for removal
                    if (!string.IsNullOrEmpty(removedImages))
                    {
                        var removedImageUrls = removedImages.Split(',').ToList();
                        await _fileService.DeleteImage(removedImageUrls, userName);
                    }

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditBusinessCategory,
                        EntityName = EntityConstants.BusinessCategory,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated business category '{businessCategoryInternal.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = businessCategoryInternal
                        }),
                        RecordId = businessCategoryInternal.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessCategoryInternalExists(businessCategoryInternal.ID))
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

            // If we got this far, something failed, redisplay form with images
            var images = await _attachmentService.GetAttachmentLinks(new GetAttachmentLink
            {
                AttachmentType = AttachmentTypeConstants.BUSINESS_CATEGORY,
                Entity = EntityConstants.BusinessCategory,
                RecordID = businessCategoryInternal.ID
            });

            ViewBag.CategoryImages = images;
            
            return View(businessCategoryInternal);
        }

        // GET: BusinessCategoryInternals/Delete/5
        [SecurableAuthorize(SecurableConstants.BusinessCategoryInternalsDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessCategoryInternal = await _context.BusinessCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessCategoryInternal == null)
            {
                return NotFound();
            }

            return View(businessCategoryInternal);
        }

        // POST: BusinessCategoryInternals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.BusinessCategoryInternalsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessCategoryInternal = await _context.BusinessCategories.FindAsync(id);
            if (businessCategoryInternal != null)
            {
                _context.BusinessCategories.Remove(businessCategoryInternal);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteBusinessCategory,
                    EntityName = EntityConstants.BusinessCategory,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted business category '{businessCategoryInternal.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(businessCategoryInternal),
                    RecordId = businessCategoryInternal.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessCategoryInternalExists(int id)
        {
            return _context.BusinessCategories.Any(e => e.ID == id);
        }
    }
}
