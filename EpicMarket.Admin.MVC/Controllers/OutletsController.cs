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
using EpicMarket.Entities.CustomModels;
using Microsoft.CodeAnalysis.Operations;
using EpicMarket.Admin.MVC.Contracts;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class OutletsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;

        public OutletsController(ApplicationDbContext context, IAttachmentService attachmentService, IFileService fileService)
        {
            _context = context;
            this.attachmentService = attachmentService;
            this.fileService = fileService;
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
			var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.BRANCH_PHOTOS);



			var attachments = from attachment in _context.Attachments
							  join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
							  join entity in _context.Entity on link.EntityID equals entity.ID
							  where entity.Name == EntityConstants.Branch && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_Product.ID
							  select new
							  {
								  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
							  };

			var thumbnail = from attachment in _context.Attachments
							join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
							join entity in _context.Entity on link.EntityID equals entity.ID
							where entity.Name == EntityConstants.Branch && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
							orderby attachment.CreateDate descending
							select new
							{
								ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
							};


			ViewBag.thumbnail = thumbnail.Select(a => a.ImagePath).FirstOrDefault();
			ViewBag.attachments = attachments.Select(a => a.ImagePath).ToList();


			if (outlet == null)
            {
                return NotFound();
            }

            return View(outletDetails);
        }

        // GET: Outlets/Create
        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID");
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View();
        }

        // POST: Outlets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                if (thumbnail?.Length > 0)
                {
                    var attachmentThumbnail = new AttachmentModel()
                    {
                        Name = EntityConstants.Branch,
                        Comment = EntityConstants.Branch,
                        RecordID = outlet.ID,
                        Entity = EntityConstants.Branch,
                        AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                        FolderPathConstant = FilePathConstants.BranchThumbnail,
                        Files = thumbnail
                    };
                    await attachmentService.InsertAttachment(attachmentThumbnail);
                }

                if (OutletImages?.Length > 0)
                {
                    var attachmentOutletImages = new AttachmentModel()
                    {
                        Name = EntityConstants.Branch,
                        Comment = EntityConstants.Branch,
                        RecordID = outlet.ID,
                        Entity = EntityConstants.Branch,
                        AttachmentType = AttachmentTypeConstants.BRANCH_PHOTOS,
                        FolderPathConstant = FilePathConstants.BranchesPhotos,
                        Files = OutletImages
                    };
                    await attachmentService.InsertAttachment(attachmentOutletImages);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", outlet.AddressID);
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", outlet.BussinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status",outlet.StatusId);
            return View(outlet);
        }

        // GET: Outlets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var outlet = await _context.Outlets.Where(b => b.ID == id).Include(c => c.Address).Include(c => c.StatusOptionSets).FirstOrDefaultAsync();
            ViewBag.thumbnails = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                Entity = EntityConstants.Branch,
                RecordID = outlet.ID
            });

            ViewBag.BranchImages = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.BRANCH_PHOTOS,
                Entity = EntityConstants.Branch,
                RecordID = outlet.ID
            });
            if (outlet == null)
            {
                return NotFound();
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", outlet.AddressID);
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", outlet.BussinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", outlet.StatusId);
            return View(outlet);
        }

        // POST: Outlets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BussinessID,AddressID,Name,Description,ContactNumber,ContactEmail,Rating,ReviewCount,IsOpen,Weight,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address,IsActive")] Outlet outlet, IFormFile[] newThumbnail, string? addedThumbnail, IFormFile[] newOutletImages, string? removedOutletImages, string? addedOutletImages)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;


            outlet.Address.ModifiedBy = userName;
            outlet.Address.ModifiedDate = DateTime.UtcNow;
            outlet.Address.Id = outlet.AddressID;
            outlet.ModifiedBy = userName;
            outlet.ModifiedDate = DateTime.UtcNow;

            if (id != outlet.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outlet);
                    await _context.SaveChangesAsync();
                    if (addedThumbnail != null && addedThumbnail?.Length > 0)
                    {
                        List<string> ExisitingThumbnail = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                            Entity = EntityConstants.Branch,
                            RecordID = outlet.ID
                        });
                        if (ExisitingThumbnail?.Count > 0)
                        {
                            await this.fileService.DeleteImage(ExisitingThumbnail, userName);
                        }

                        var attachmentThumbnail = new AttachmentModel()
                        {
                            Name = EntityConstants.Branch,
                            Comment = EntityConstants.Branch,
                            RecordID = outlet.ID,
                            Entity = EntityConstants.Branch,
                            AttachmentType = AttachmentTypeConstants.BRANCH_THUMBNAIL,
                            FolderPathConstant = FilePathConstants.BranchThumbnail,
                            Files = newThumbnail
                        };
                        await attachmentService.InsertAttachment(attachmentThumbnail);

                    }

                    // Handle removed images
                    if (!string.IsNullOrEmpty(removedOutletImages))
                    {
                        var removedImageUrls = removedOutletImages.Split(',').ToList();
                        await this.fileService.DeleteImage(removedImageUrls, userName);
                    }

                    // Handle added images
                    if (addedOutletImages != null && newOutletImages?.Length > 0)
                    {

                        var attachmentOutletImages = new AttachmentModel()
                        {
                            Name = EntityConstants.Branch,
                            Comment = EntityConstants.Branch,
                            RecordID = outlet.ID,
                            Entity = EntityConstants.Branch,
                            AttachmentType = AttachmentTypeConstants.BRANCH_PHOTOS,
                            FolderPathConstant = FilePathConstants.BranchesPhotos,
                            Files = newOutletImages
                        };
                        await attachmentService.InsertAttachment(attachmentOutletImages);

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
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", outlet.AddressID);
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", outlet.BussinessID);
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
            var outlet = await _context.Outlets.FindAsync(id);
            if (outlet != null)
            {
                _context.Outlets.Remove(outlet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutletExists(int id)
        {
            return _context.Outlets.Any(e => e.ID == id);
        }
    }
}
