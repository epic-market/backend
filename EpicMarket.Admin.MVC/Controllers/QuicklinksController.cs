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
    public class QuicklinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuicklinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quicklinks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quicklink.ToListAsync());
        }

        // GET: Quicklinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quicklink = await _context.Quicklink
				.FirstOrDefaultAsync(m => m.Id == id);
            if (quicklink == null)
            {
                return NotFound();
            }

            return View(quicklink);
        }

        // GET: Quicklinks/Create
        public IActionResult Create(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Quicklinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Url,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Quicklink quicklink, string returnUrl = null)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            quicklink.CreateBy = userName;
            quicklink.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(quicklink);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
               
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(quicklink);
        }

        // GET: Quicklinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quicklink = await _context.Quicklink.FindAsync(id);
            if (quicklink == null)
            {
                return NotFound();
            }
            return View(quicklink);
        }

        // POST: Quicklinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Quicklink quicklink)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            quicklink.ModifiedBy = userName;
            quicklink.ModifiedDate = DateTime.UtcNow;
            if (id != quicklink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quicklink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuicklinkExists(quicklink.Id))
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
            return View(quicklink);
        }

        // GET: Quicklinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quicklink = await _context.Quicklink
				.FirstOrDefaultAsync(m => m.Id == id);
            if (quicklink == null)
            {
                return NotFound();
            }

            return View(quicklink);
        }

        // POST: Quicklinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quicklink = await _context.Quicklink.FindAsync(id);
            if (quicklink != null)
            {
                _context.Quicklink.Remove(quicklink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuicklinkExists(int id)
        {
            return _context.Quicklink.Any(e => e.Id == id);
        }
    }
}
