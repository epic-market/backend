using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Data.ApplicationModels;
using System.Security.Claims;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class EntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Entities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Entity.ToListAsync());
        }

        // GET: Entities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Entities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Entity entity)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            entity.CreateBy = userName;
            entity.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        // GET: Entities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Entity entity)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            entity.ModifiedBy = userName;
            entity.ModifiedDate = DateTime.UtcNow;
            if (id != entity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.ID))
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
            return View(entity);
        }

        // GET: Entities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.Entity.FindAsync(id);
            if (entity != null)
            {
                _context.Entity.Remove(entity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntityExists(int id)
        {
            return _context.Entity.Any(e => e.ID == id);
        }
    }
}
