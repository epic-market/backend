using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class ApplicationConfigurationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationConfigurationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationConfigurations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationConfigurations.ToListAsync());
        }

        // GET: ApplicationConfigurations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationConfiguration = await _context.ApplicationConfigurations
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationConfiguration == null)
            {
                return NotFound();
            }

            return View(applicationConfiguration);
        }

        // GET: ApplicationConfigurations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationConfigurations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Value,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationConfiguration applicationConfiguration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationConfiguration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationConfiguration);
        }

        // GET: ApplicationConfigurations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationConfiguration = await _context.ApplicationConfigurations.FindAsync(id);
            if (applicationConfiguration == null)
            {
                return NotFound();
            }
            return View(applicationConfiguration);
        }

        // POST: ApplicationConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Value,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ApplicationConfiguration applicationConfiguration)
        {
            if (id != applicationConfiguration.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationConfiguration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationConfigurationExists(applicationConfiguration.ID))
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
            return View(applicationConfiguration);
        }

        // GET: ApplicationConfigurations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationConfiguration = await _context.ApplicationConfigurations
                .FirstOrDefaultAsync(m => m.ID == id);
            if (applicationConfiguration == null)
            {
                return NotFound();
            }

            return View(applicationConfiguration);
        }

        // POST: ApplicationConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationConfiguration = await _context.ApplicationConfigurations.FindAsync(id);
            if (applicationConfiguration != null)
            {
                _context.ApplicationConfigurations.Remove(applicationConfiguration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationConfigurationExists(int id)
        {
            return _context.ApplicationConfigurations.Any(e => e.ID == id);
        }
    }
}
