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
    public class ContactMethodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactMethodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactMethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactMethod.ToListAsync());
        }

        // GET: ContactMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMethod = await _context.ContactMethod
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contactMethod == null)
            {
                return NotFound();
            }

            return View(contactMethod);
        }

        // GET: ContactMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ContactMethod contactMethod)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            contactMethod.CreateBy = userName;
            contactMethod.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(contactMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactMethod);
        }

        // GET: ContactMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMethod = await _context.ContactMethod.FindAsync(id);
            if (contactMethod == null)
            {
                return NotFound();
            }
            return View(contactMethod);
        }

        // POST: ContactMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] ContactMethod contactMethod)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            contactMethod.ModifiedBy = userName;
            contactMethod.ModifiedDate = DateTime.UtcNow;
            if (id != contactMethod.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactMethodExists(contactMethod.ID))
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
            return View(contactMethod);
        }

        // GET: ContactMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMethod = await _context.ContactMethod
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contactMethod == null)
            {
                return NotFound();
            }

            return View(contactMethod);
        }

        // POST: ContactMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactMethod = await _context.ContactMethod.FindAsync(id);
            if (contactMethod != null)
            {
                _context.ContactMethod.Remove(contactMethod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactMethodExists(int id)
        {
            return _context.ContactMethod.Any(e => e.ID == id);
        }
    }
}
