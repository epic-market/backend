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

namespace EpicMarket.Admin.MVC.Controllers
{
    public class SupportQuerysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportQuerysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupportQuerys
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.SupportQuerys.Include(s => s.PersonType).Include(s => s.TaskTypes);
            return View(await authDbContext.ToListAsync());
        }

        // GET: SupportQuerys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportQuerys = await _context.SupportQuerys
                .Include(s => s.PersonType)
                .Include(s => s.TaskTypes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supportQuerys == null)
            {
                return NotFound();
            }

            return View(supportQuerys);
        }

        // GET: SupportQuerys/Create
        public IActionResult Create()
        {
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type");
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name");
            return View();
        }

        // POST: SupportQuerys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Query,TaskTypeID,TypeofPersonid,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] SupportQuerys supportQuerys)
        {

			var userName = this.User.FindFirst(ClaimTypes.Name).Value;
			supportQuerys.CreateBy = userName;
			supportQuerys.CreateDate = DateTime.UtcNow;
			if (ModelState.IsValid)
            {
                _context.Add(supportQuerys);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type", supportQuerys.TypeofPersonid);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", supportQuerys.TaskTypeID);
            return View(supportQuerys);
        }

        // GET: SupportQuerys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportQuerys = await _context.SupportQuerys.FindAsync(id);
            if (supportQuerys == null)
            {
                return NotFound();
            }
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type", supportQuerys.TypeofPersonid);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", supportQuerys.TaskTypeID);
            return View(supportQuerys);
        }

        // POST: SupportQuerys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Query,TaskTypeID,TypeofPersonid,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] SupportQuerys supportQuerys)
        {

			var userName = this.User.FindFirst(ClaimTypes.Name).Value;
			supportQuerys.ModifiedBy = userName;
			supportQuerys.ModifiedDate = DateTime.UtcNow;
			if (id != supportQuerys.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportQuerys);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportQuerysExists(supportQuerys.ID))
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
            ViewData["TypeofPersonid"] = new SelectList(_context.PersonTypes, "ID", "Type", supportQuerys.TypeofPersonid);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", supportQuerys.TaskTypeID);
            return View(supportQuerys);
        }

        // GET: SupportQuerys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportQuerys = await _context.SupportQuerys
                .Include(s => s.PersonType)
                .Include(s => s.TaskTypes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supportQuerys == null)
            {
                return NotFound();
            }

            return View(supportQuerys);
        }

        // POST: SupportQuerys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportQuerys = await _context.SupportQuerys.FindAsync(id);
            if (supportQuerys != null)
            {
                _context.SupportQuerys.Remove(supportQuerys);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportQuerysExists(int id)
        {
            return _context.SupportQuerys.Any(e => e.ID == id);
        }
    }
}
