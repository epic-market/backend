using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class AccessTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AccessTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccessTypes.ToListAsync());
        }

        // GET: AccessTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessType = await _context.AccessTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accessType == null)
            {
                return NotFound();
            }

            return View(accessType);
        }

        // GET: AccessTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccessTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Priority")] AccessType accessType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accessType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accessType);
        }

        // GET: AccessTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessType = await _context.AccessTypes.FindAsync(id);
            if (accessType == null)
            {
                return NotFound();
            }
            return View(accessType);
        }

        // POST: AccessTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Priority")] AccessType accessType)
        {
            if (id != accessType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accessType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessTypeExists(accessType.Id))
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
            return View(accessType);
        }

        // GET: AccessTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessType = await _context.AccessTypes
				.FirstOrDefaultAsync(m => m.Id == id);
            if (accessType == null)
            {
                return NotFound();
            }

            return View(accessType);
        }

        // POST: AccessTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessType = await _context.AccessTypes.FindAsync(id);
            if (accessType != null)
            {
                _context.AccessTypes.Remove(accessType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessTypeExists(int id)
        {
            return _context.AccessTypes.Any(e => e.Id == id);
        }
    }
}
