using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class AttachmentLinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttachmentLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AttachmentLinks
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.AttachmentLinks.Include(a => a.AttachmentType).Include(a => a.Attachments).Include(a => a.Entity);
            return View(await authDbContext.ToListAsync());
        }

        // GET: AttachmentLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachmentLink = await _context.AttachmentLinks
                .Include(a => a.AttachmentType)
                .Include(a => a.Attachments)
                .Include(a => a.Entity)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachmentLink == null)
            {
                return NotFound();
            }

            return View(attachmentLink);
        }

        // GET: AttachmentLinks/Create
        public IActionResult Create()
        {
            ViewData["AttachmentTypeID"] = new SelectList(_context.AttachmentTypes, "ID", "Name");
            ViewData["AttachmentID"] = new SelectList(_context.Attachments, "ID", "Name");
            ViewData["EntityID"] = new SelectList(_context.Entity, "ID", "ID");
            return View();
        }

        // POST: AttachmentLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AttachmentID,EntityID,AttachmentTypeID,RecordID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] AttachmentLink attachmentLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attachmentLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttachmentTypeID"] = new SelectList(_context.AttachmentTypes, "ID", "Name", attachmentLink.AttachmentTypeID);
            ViewData["AttachmentID"] = new SelectList(_context.Attachments, "ID", "Name", attachmentLink.AttachmentID);
            ViewData["EntityID"] = new SelectList(_context.Entity, "ID", "ID", attachmentLink.EntityID);
            return View(attachmentLink);
        }

        // GET: AttachmentLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachmentLink = await _context.AttachmentLinks.FindAsync(id);
            if (attachmentLink == null)
            {
                return NotFound();
            }
            ViewData["AttachmentTypeID"] = new SelectList(_context.AttachmentTypes, "ID", "Name", attachmentLink.AttachmentTypeID);
            ViewData["AttachmentID"] = new SelectList(_context.Attachments, "ID", "Name", attachmentLink.AttachmentID);
            ViewData["EntityID"] = new SelectList(_context.Entity, "ID", "ID", attachmentLink.EntityID);
            return View(attachmentLink);
        }

        // POST: AttachmentLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AttachmentID,EntityID,AttachmentTypeID,RecordID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] AttachmentLink attachmentLink)
        {
            if (id != attachmentLink.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attachmentLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentLinkExists(attachmentLink.ID))
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
            ViewData["AttachmentTypeID"] = new SelectList(_context.AttachmentTypes, "ID", "Name", attachmentLink.AttachmentTypeID);
            ViewData["AttachmentID"] = new SelectList(_context.Attachments, "ID", "Name", attachmentLink.AttachmentID);
            ViewData["EntityID"] = new SelectList(_context.Entity, "ID", "ID", attachmentLink.EntityID);
            return View(attachmentLink);
        }

        // GET: AttachmentLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachmentLink = await _context.AttachmentLinks
                .Include(a => a.AttachmentType)
                .Include(a => a.Attachments)
                .Include(a => a.Entity)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachmentLink == null)
            {
                return NotFound();
            }

            return View(attachmentLink);
        }

        // POST: AttachmentLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attachmentLink = await _context.AttachmentLinks.FindAsync(id);
            if (attachmentLink != null)
            {
                _context.AttachmentLinks.Remove(attachmentLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentLinkExists(int id)
        {
            return _context.AttachmentLinks.Any(e => e.ID == id);
        }
    }
}
