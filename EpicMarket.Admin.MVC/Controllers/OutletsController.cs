using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Models;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Operations;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using System.Text.Json;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class OutletsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public OutletsController(
            ApplicationDbContext context, 
            IAttachmentService attachmentService, 
            IFileService fileService,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            this.attachmentService = attachmentService;
            this.fileService = fileService;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Outlets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Outlets.Include(o => o.Address).Include(o => o.Bussiness).Include(o => o.StatusOptionSets);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Outlets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletDetails = new OutletsDetailsModel();

            var outlet = await _context.Outlets
                .Include(o => o.Address)
                .Include(o => o.Bussiness)
                .Include(o=> o.StatusOptionSets)
                .FirstOrDefaultAsync(m => m.ID == id);

            var orders = await _context.Orders.Where(c => c.OutletID == id).Include(o => o.Address).ToListAsync();
            var outletPersons = await _context.OutletPeople.Where(c => c.OutletId == id).Include(o => o.Person).ToListAsync();
            var outletProducts = await _context.Inventory.Where(c => c.OutletID == id).Include(o => o.ProductVariants).ToListAsync();
            
            outletDetails.Outlet = outlet;
            outletDetails.Orders = orders;
            outletDetails.Inventorys = outletProducts;
            outletDetails.OutletEmployees = outletPersons;

            var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_THUMBNAIL);
            var attachmentTypeID_Photos = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_PHOTOS);

            var thumbnails = from attachment in _context.Attachments
                          join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                          join entity in _context.Entity on link.EntityID equals entity.ID
                          where entity.Name == EntityConstants.Branch && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
                          orderby attachment.CreateDate descending
                          select new
                          {
                              ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                          };

            var photos = from attachment in _context.Attachments
                       join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                       join entity in _context.Entity on link.EntityID equals entity.ID
                       where entity.Name == EntityConstants.Branch && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_Photos.ID
                       select new
                       {
                           ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                       };

            ViewBag.thumbnail = thumbnails.Select(a => a.ImagePath).FirstOrDefault();
            ViewBag.attachments = photos.Select(a => a.ImagePath).ToList();

            if (outlet == null)
            {
                return NotFound();
            }

            return View(outletDetails);
        }

        // GET: Outlets/Create
        public IActionResult Create()
        {
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "Name");
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View();
        }

        // POST: Outlets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BussinessID,AddressID,Name,Description,ContactNumber,ContactEmail,Rating,ReviewCount,IsOpen,Weight,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Outlet outlet, IFormFile[] thumbnail, IFormFile[] OutletImages)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            outlet.Address.CreateBy = userName;
            outlet.Address.CreateDate = DateTime.UtcNow;
            outlet.CreateBy = userName;
            outlet.CreateDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(outlet);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CREATE,
                    EntityName = EntityConstants.Outlet,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created outlet: {outlet.Name}",
                    Data = JsonSerializer.Serialize(outlet),
                    RecordId = outlet.ID,
                    BusinessID = outlet.BussinessID,
                    LoggedInUserName = userName
                });
                
                // Upload thumbnail if provided
                if (thumbnail?.Length > 0)
                {
                    var attachmentThumbnail = new BusinessAttachmentModel()
                    {
                        Name = EntityConstants.Branch,
                        Comment = EntityConstants.Branch,
                        RecordID = outlet.ID,
                        Entity = EntityConstants.Branch,
                        AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                        Files = thumbnail,
                        BusinessID = outlet.BussinessID
                    };
                    await attachmentService.UploadBusinessAttachment(attachmentThumbnail);
                }

                // Upload outlet images if provided
                if (OutletImages?.Length > 0)
                {
                    var attachmentOutletImages = new BusinessAttachmentModel()
                    {
                        Name = EntityConstants.Branch,
                        Comment = EntityConstants.Branch,
                        RecordID = outlet.ID,
                        Entity = EntityConstants.Branch,
                        AttachmentType = AttachmentTypeConstants.BRANCH_PHOTOS,
                        Files = OutletImages,
                        BusinessID = outlet.BussinessID
                    };
                    await attachmentService.UploadBusinessAttachment(attachmentOutletImages);
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "Name", outlet.BussinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", outlet.StatusId);
            return View(outlet);
        }

        // GET: Outlets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outlet = await _context.Outlets
                .Where(b => b.ID == id)
                .Include(c => c.Address)
                .Include(c => c.StatusOptionSets)
                .FirstOrDefaultAsync();
                
            if (outlet == null)
            {
                return NotFound();
            }
            
            // Get thumbnail images
            ViewBag.thumbnails = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                Entity = EntityConstants.Branch,
                RecordID = outlet.ID
            });

            // Get outlet images
            ViewBag.BranchImages = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.BRANCH_PHOTOS,
                Entity = EntityConstants.Branch,
                RecordID = outlet.ID
            });
            
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "Name", outlet.BussinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", outlet.StatusId);
            return View(outlet);
        }

        // POST: Outlets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BussinessID,AddressID,Name,Description,ContactNumber,ContactEmail,Rating,ReviewCount,IsOpen,Weight,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address,IsActive")] Outlet outlet, IFormFile[] newThumbnail, string? addedThumbnail, IFormFile[] newOutletImages, string? removedOutletImages, string? addedOutletImages)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            
            // Get original entity for event logging
            var originalEntity = await _context.Outlets
                .AsNoTracking()
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.ID == id);

            if (id != outlet.ID)
            {
                return NotFound();
            }

            // Update modification info
            outlet.Address.ModifiedBy = userName;
            outlet.Address.ModifiedDate = DateTime.UtcNow;
            outlet.Address.Id = outlet.AddressID;
            outlet.ModifiedBy = userName;
            outlet.ModifiedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outlet);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.UPDATE,
                        EntityName = EntityConstants.Outlet,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated outlet: {outlet.Name}",
                        Data = JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = outlet
                        }),
                        RecordId = outlet.ID,
                        BusinessID = outlet.BussinessID,
                        LoggedInUserName = userName
                    });
                    
                    // Handle thumbnail update
                    if (addedThumbnail != null && newThumbnail?.Length > 0)
                    {
                        // Get existing thumbnails
                        List<string> existingThumbnails = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                            Entity = EntityConstants.Branch,
                            RecordID = outlet.ID
                        });
                        
                        // Delete existing thumbnails if any
                        if (existingThumbnails?.Count > 0)
                        {
                            await this.fileService.DeleteImage(existingThumbnails, userName);
                        }

                        // Upload new thumbnail
                        var attachmentThumbnail = new BusinessAttachmentModel()
                        {
                            Name = EntityConstants.Branch,
                            Comment = EntityConstants.Branch,
                            RecordID = outlet.ID,
                            Entity = EntityConstants.Branch,
                            AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                            Files = newThumbnail,
                            BusinessID = outlet.BussinessID
                        };
                        await attachmentService.UploadBusinessAttachment(attachmentThumbnail);
                    }

                    // Handle removed outlet images
                    if (!string.IsNullOrEmpty(removedOutletImages))
                    {
                        var removedImageUrls = removedOutletImages.Split(',').ToList();
                        await this.fileService.DeleteImage(removedImageUrls, userName);
                    }

                    // Handle added outlet images
                    if (addedOutletImages != null && newOutletImages?.Length > 0)
                    {
                        var attachmentOutletImages = new BusinessAttachmentModel()
                        {
                            Name = EntityConstants.Branch,
                            Comment = EntityConstants.Branch,
                            RecordID = outlet.ID,
                            Entity = EntityConstants.Branch,
                            AttachmentType = AttachmentTypeConstants.BRANCH_PHOTOS,
                            Files = newOutletImages,
                            BusinessID = outlet.BussinessID

                        };
                        await attachmentService.UploadBusinessAttachment(attachmentOutletImages);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutletExists(outlet.ID))
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
            
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "Name", outlet.BussinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", outlet.StatusId);
            return View(outlet);
        }

        // GET: Outlets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outlet = await _context.Outlets
                .Include(o => o.Address)
                .Include(o => o.Bussiness)
                .Include(o => o.StatusOptionSets)
                .FirstOrDefaultAsync(m => m.ID == id);
                
            if (outlet == null)
            {
                return NotFound();
            }

            return View(outlet);
        }

        // POST: Outlets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outlet = await _context.Outlets
                .Include(o => o.Bussiness)
                .FirstOrDefaultAsync(o => o.ID == id);
                
            if (outlet != null)
            {
                
                _context.Outlets.Remove(outlet);
                
                // Log event before saving changes
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DELETE,
                    EntityName = EntityConstants.Outlet,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted outlet: {outlet.Name}",
                    Data = JsonSerializer.Serialize(outlet),
                    RecordId = outlet.ID,
                    BusinessID = outlet.BussinessID,
                    LoggedInUserName = User.Identity.Name
                });
                
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OutletExists(int id)
        {
            return _context.Outlets.Any(e => e.ID == id);
        }

        [HttpPost]
        [Route("Outlet/GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] OutletFilterViewModel filter)
        {
            try
            {
                var query = _context.Outlets
                    .Include(o => o.Address)
                    .Include(o => o.Bussiness)
                    .Include(o => o.StatusOptionSets)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.OutletId))
                {
                    query = query.Where(o => o.ID.ToString().Contains(filter.OutletId));
                }

                if (!string.IsNullOrWhiteSpace(filter.OutletName))
                {
                    query = query.Where(o => o.Name.Contains(filter.OutletName));
                }

                if (!string.IsNullOrWhiteSpace(filter.BusinessName))
                {
                    query = query.Where(o => o.Bussiness.Name.Contains(filter.BusinessName));
                }

                // Add filter for Business ID
                if (!string.IsNullOrWhiteSpace(filter.BusinessId))
                {
                    if (int.TryParse(filter.BusinessId, out int businessId))
                    {
                        query = query.Where(o => o.BussinessID == businessId);
                    }
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(o => o.ID) : query.OrderByDescending(o => o.ID),
                    "name" => filter.SortDirection == "asc" ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name),
                    "businessname" => filter.SortDirection == "asc" ? query.OrderBy(o => o.Bussiness.Name) : query.OrderByDescending(o => o.Bussiness.Name),
                    "city" => filter.SortDirection == "asc" ? query.OrderBy(o => o.Address.City) : query.OrderByDescending(o => o.Address.City),
                    "status" => filter.SortDirection == "asc" ? query.OrderBy(o => o.StatusOptionSets.Status) : query.OrderByDescending(o => o.StatusOptionSets.Status),
                    _ => query.OrderBy(o => o.ID)
                };

                // Apply pagination and project to DTO
                var outlets = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(o => new OutletDto
                    {
                        ID = o.ID,
                        Name = o.Name,
                        BusinessName = o.Bussiness.Name,
                        BusinessID = o.BussinessID,
                        ContactNumber = o.ContactNumber,
                        ContactEmail = o.ContactEmail,
                        City = o.Address.City,
                        StatusName = o.StatusOptionSets.Status,
                        IsOpen = o.IsOpen
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = outlets });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
