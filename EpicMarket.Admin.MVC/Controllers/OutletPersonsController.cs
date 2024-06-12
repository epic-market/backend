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

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class OutletPersonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OutletPersonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OutletPersons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OutletPeople.Include(o => o.Outlet).Include(o => o.Outlet.Bussiness).Include(o => o.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OutletPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletPerson = await _context.OutletPeople
                .Include(o => o.Outlet)
                .Include(o => o.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outletPerson == null)
            {
                return NotFound();
            }

            return View(outletPerson);
        }

        // GET: OutletPersons/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID");
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: OutletPersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PersonId,OutletId")] OutletPerson outletPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outletPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID", outletPerson.PersonId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", outletPerson.PersonId);
            return View(outletPerson);
        }

        // GET: OutletPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletPerson = await _context.OutletPeople.FindAsync(id);
            if (outletPerson == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID", outletPerson.PersonId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", outletPerson.PersonId);
            return View(outletPerson);
        }

        // POST: OutletPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonId,OutletId")] OutletPerson outletPerson)
        {
            if (id != outletPerson.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outletPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutletPersonExists(outletPerson.ID))
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
            ViewData["PersonId"] = new SelectList(_context.Outlets, "ID", "ID", outletPerson.PersonId);
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", outletPerson.PersonId);
            return View(outletPerson);
        }

        // GET: OutletPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outletPerson = await _context.OutletPeople
                .Include(o => o.Outlet)
                .Include(o => o.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (outletPerson == null)
            {
                return NotFound();
            }

            return View(outletPerson);
        }

        // POST: OutletPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outletPerson = await _context.OutletPeople.FindAsync(id);
            if (outletPerson != null)
            {
                _context.OutletPeople.Remove(outletPerson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutletPersonExists(int id)
        {
            return _context.OutletPeople.Any(e => e.ID == id);
        }
    }
}
