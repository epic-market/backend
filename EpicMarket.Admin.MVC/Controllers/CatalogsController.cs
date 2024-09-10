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

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class CatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogsController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("ID,BusinessID,Barcode,Name,Description,Images,Category,Rate,IsActive,InStock,IsRecommended,MaximumOrderPurchase,Rating,ReviewCount,OrderCount,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Catalog catalog)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,BusinessID,Barcode,Name,Description,Images,Category,Rate,IsActive,InStock,IsRecommended,MaximumOrderPurchase,Rating,ReviewCount,OrderCount,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Catalog catalog)
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
