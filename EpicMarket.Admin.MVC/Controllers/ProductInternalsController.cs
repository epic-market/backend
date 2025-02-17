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
using System.Security.Claims;
using EpicMarket.Admin.MVC.Models;
using EpicMarket.Admin.MVC.Contracts;


namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class ProductInternalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;

        public ProductInternalsController(ApplicationDbContext context,IAttachmentService attachmentService,IFileService fileService)
        {
            _context = context;
            this.attachmentService = attachmentService;
            this.fileService = fileService;
        }



        // GET: ProductInternals
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductInternals.ToListAsync());
        }

        // GET: ProductInternals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInternal = await _context.ProductInternals
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewBag.attachments = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.ProductInternal,
                Entity = EntityConstants.ProductInternal,
                RecordID = productInternal.ID
            });
           

            if (productInternal == null)
            {
                return NotFound();
            }

            return View(productInternal);
        }

        // GET: ProductInternals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductInternals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BarCode,Name,Description")] ProductInternalModel productInternalModel, IFormFile[] images)
        {

            var productInternal = new ProductInternal();

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            productInternal.CreateBy = userName;
            productInternal.CreateDate = DateTime.UtcNow;
            productInternal.BarCode = productInternalModel.BarCode;
            productInternal.Name = productInternalModel.Name;
            productInternal.Description = productInternalModel.Description;
      
                _context.Add(productInternal);
                await _context.SaveChangesAsync();
                var attachment = new AttachmentModel()
                {
                    Name = EntityConstants.ProductInternal,
                    Comment = EntityConstants.ProductInternal,
                    RecordID = productInternal.ID,
                    Entity = EntityConstants.ProductInternal,
                    AttachmentType = AttachmentTypeConstants.ProductInternal,
                    Files = images
                };
                await attachmentService.UploadAttachment(attachment);
                return RedirectToAction(nameof(Index));

        }

        // GET: ProductInternals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productInternal = await _context.ProductInternals.FindAsync(id);

            ViewBag.attachments = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.ProductInternal,
                Entity = EntityConstants.ProductInternal,
                RecordID = productInternal.ID
            });


            if (productInternal == null)
            {
                return NotFound();
            }
            return View(productInternal);
        }

        // POST: ProductInternals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BarCode,Name,Description,Images,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ProductInternal productInternal, IFormFile[] newImages , string? removedImages, string? addedImages)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            productInternal.ModifiedBy = userName;
            productInternal.ModifiedDate = DateTime.UtcNow;

            if (id != productInternal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInternal);
                    await _context.SaveChangesAsync();
                    // Handle removed images
                    if (!string.IsNullOrEmpty(removedImages))
                    {
                        var removedImageUrls = removedImages.Split(',').ToList();
                        await  this.fileService.DeleteImage(removedImageUrls, userName);
                    }

                    // Handle added images
                    if (addedImages != null && newImages?.Length > 0 )
                    {

                            var attachment = new AttachmentModel()
                            {
                                Name = EntityConstants.ProductInternal,
                                Comment = EntityConstants.ProductInternal,
                                RecordID = productInternal.ID,
                                Entity = EntityConstants.ProductInternal,
                                AttachmentType = AttachmentTypeConstants.ProductInternal,
                                Files = newImages
                            };
                            await attachmentService.UploadAttachment(attachment);
      
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInternalExists(productInternal.ID))
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
            return View(productInternal);
        }

        // GET: ProductInternals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInternal = await _context.ProductInternals
                .FirstOrDefaultAsync(m => m.ID == id);
            if (productInternal == null)
            {
                return NotFound();
            }

            return View(productInternal);
        }

        // POST: ProductInternals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInternal = await _context.ProductInternals.FindAsync(id);
            if (productInternal != null)
            {
                _context.ProductInternals.Remove(productInternal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInternalExists(int id)
        {
            return _context.ProductInternals.Any(e => e.ID == id);
        }
    }
}
