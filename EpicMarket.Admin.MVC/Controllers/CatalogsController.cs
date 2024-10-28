using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EpicMarket.Entities.CustomModels;
using Microsoft.CodeAnalysis;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Admin.MVC.Contracts;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class CatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;

        public CatalogsController(ApplicationDbContext context, IAttachmentService attachmentService, IFileService fileService)
        {
            _context = context;
            this.attachmentService = attachmentService;
            this.fileService = fileService;
        }

        // GET: Catalogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Catalogs.Include(c => c.Business).Include(c=>c.StatusOptionSets);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Catalogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Business)
               .Include(c=>c.StatusOptionSets)
                .FirstOrDefaultAsync(m => m.ID == id);



			var attachmentTypeID_Thumbnail = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.THUMBNAIL);
			var attachmentTypeID_Product = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PRODUCTIMAGES);



			var attachments = from attachment in _context.Attachments
							  join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
							  join entity in _context.Entity on link.EntityID equals entity.ID
							  where entity.Name == EntityConstants.Catelog && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_Product.ID
							  select new
							  {
								  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
							  };

			var thumbnail = from attachment in _context.Attachments
							join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
							join entity in _context.Entity on link.EntityID equals entity.ID
							where entity.Name == EntityConstants.Catelog && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_Thumbnail.ID
							orderby attachment.CreateDate descending
							select new
							{
								ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
							};



			ViewBag.thumbnail = thumbnail.Select(a => a.ImagePath).FirstOrDefault();
			ViewBag.attachments = attachments.Select(a => a.ImagePath).ToList();


			if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // GET: Catalogs/Create
        public IActionResult Create()
        {
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID");
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View();
        }

        // POST: Catalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BusinessID,Barcode,Name,Description,Images,Category,Rate,InStock,IsRecommended,MaximumOrderPurchase,Rating,ReviewCount,OrderCount,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Catalog catalog, IFormFile[] thumbnail, IFormFile[] ProductImages)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            catalog.CreateBy = userName;
            catalog.CreateDate = DateTime.UtcNow;
            if (ModelState.ContainsKey("CreateDate"))
            {
                ModelState.Remove("CreateDate");
            }

            if (ModelState.IsValid)
            {
                _context.Add(catalog);
                await _context.SaveChangesAsync();

                if (thumbnail?.Length > 0)
                {
                    var attachmentLogo = new AttachmentModel()
                    {
                        Name = EntityConstants.Catelog,
                        Comment = EntityConstants.Catelog,
                        RecordID = catalog.ID,
                        Entity = EntityConstants.Catelog,
                        AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                        FolderPathConstant = FilePathConstants.THUMBNAILPATH,
                        Files = thumbnail
                    };
                    await attachmentService.InsertAttachment(attachmentLogo);
                }

                if (ProductImages?.Length > 0)
                {
                    var attachmentProofs = new AttachmentModel()
                    {
                        Name = EntityConstants.Catelog,
                        Comment = EntityConstants.Catelog,
                        RecordID = catalog.ID,
                        Entity = EntityConstants.Catelog,
                        AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                        FolderPathConstant = FilePathConstants.PRODUCTPATH,
                        Files = ProductImages
                    };
                    await attachmentService.InsertAttachment(attachmentProofs);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        // GET: Catalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs.FindAsync(id);
            ViewBag.thumbnails = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                Entity = EntityConstants.Catelog,
                RecordID = catalog.ID
            });

            ViewBag.ProductImages = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                Entity = EntityConstants.Catelog,
                RecordID = catalog.ID
            });
            if (catalog == null)
            {
                return NotFound();
            }
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        // POST: Catalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BusinessID,Barcode,Name,Description,Images,Category,Rate,IsActive,InStock,IsRecommended,MaximumOrderPurchase,Rating,ReviewCount,OrderCount,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Catalog catalog, IFormFile[] newThumbnail, string? addedThumbnail, IFormFile[] newProductImages, string? removedProductImages, string? addedProductImages)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            catalog.ModifiedBy = userName;
            catalog.ModifiedDate = DateTime.UtcNow;

            if (id != catalog.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalog);
                    await _context.SaveChangesAsync();

                    // Handle added images
                    if (addedThumbnail != null && addedThumbnail?.Length > 0)
                    {
                        List<string> ExisitingThumbnail = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                            Entity = EntityConstants.Catelog,
                            RecordID = catalog.ID
                        });
                        if (ExisitingThumbnail?.Count > 0)
                        {
                            await this.fileService.DeleteImage(ExisitingThumbnail, userName);
                        }

                        var attachmentThumbnail = new AttachmentModel()
                        {
                            Name = EntityConstants.Catelog,
                            Comment = EntityConstants.Catelog,
                            RecordID = catalog.ID,
                            Entity = EntityConstants.Catelog,
                            AttachmentType = AttachmentTypeConstants.THUMBNAIL,
                            FolderPathConstant = FilePathConstants.THUMBNAILPATH,
                            Files = newThumbnail
                        };
                        await attachmentService.InsertAttachment(attachmentThumbnail);

                    }

                    // Handle removed images
                    if (!string.IsNullOrEmpty(removedProductImages))
                    {
                        var removedImageUrls = removedProductImages.Split(',').ToList();
                        await this.fileService.DeleteImage(removedImageUrls, userName);
                    }

                    // Handle added images
                    if (addedProductImages != null && newProductImages?.Length > 0)
                    {

                        var attachmentProductImages = new AttachmentModel()
                        {
                            Name = EntityConstants.Catelog,
                            Comment = EntityConstants.Catelog,
                            RecordID = catalog.ID,
                            Entity = EntityConstants.Catelog,
                            AttachmentType = AttachmentTypeConstants.PRODUCTIMAGES,
                            FolderPathConstant = FilePathConstants.PRODUCTPATH,
                            Files = newProductImages
                        };
                        await attachmentService.InsertAttachment(attachmentProductImages);

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogExists(catalog.ID))
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
            ViewData["BusinessID"] = new SelectList(_context.Businesses, "ID", "ID", catalog.BusinessID);
            ViewData["StatusId"] = new SelectList(_context.StatusOptionSets, "Id", "Status", catalog.StatusId);
            return View(catalog);
        }

        // GET: Catalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Business)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalog = await _context.Catalogs.FindAsync(id);
            if (catalog != null)
            {
                _context.Catalogs.Remove(catalog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalogs.Any(e => e.ID == id);
        }
    }
}
