using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    public class BusinessEmployeeMapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessEmployeeMapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BusinessEmployeeMaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BusinessEmployeeMaps.Include(b => b.Bussiness).Include(b => b.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BusinessEmployeeMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessEmployeeMap = await _context.BusinessEmployeeMaps
                .Include(b => b.Bussiness)
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessEmployeeMap == null)
            {
                return NotFound();
            }

            return View(businessEmployeeMap);
        }

        // GET: BusinessEmployeeMaps/Create
        public IActionResult Create()
        {
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID");
            ViewData["EmployeeID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BusinessEmployeeMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BussinessID,EmployeeID")] BusinessEmployeeMap businessEmployeeMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessEmployeeMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", businessEmployeeMap.BussinessID);
            ViewData["EmployeeID"] = new SelectList(_context.Users, "Id", "Id", businessEmployeeMap.EmployeeID);
            return View(businessEmployeeMap);
        }

        // GET: BusinessEmployeeMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessEmployeeMap = await _context.BusinessEmployeeMaps.FindAsync(id);
            if (businessEmployeeMap == null)
            {
                return NotFound();
            }
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", businessEmployeeMap.BussinessID);
            ViewData["EmployeeID"] = new SelectList(_context.Users, "Id", "Id", businessEmployeeMap.EmployeeID);
            return View(businessEmployeeMap);
        }

        // POST: BusinessEmployeeMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BussinessID,EmployeeID")] BusinessEmployeeMap businessEmployeeMap)
        {
            if (id != businessEmployeeMap.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessEmployeeMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessEmployeeMapExists(businessEmployeeMap.ID))
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
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", businessEmployeeMap.BussinessID);
            ViewData["EmployeeID"] = new SelectList(_context.Users, "Id", "Id", businessEmployeeMap.EmployeeID);
            return View(businessEmployeeMap);
        }

        // GET: BusinessEmployeeMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessEmployeeMap = await _context.BusinessEmployeeMaps
                .Include(b => b.Bussiness)
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (businessEmployeeMap == null)
            {
                return NotFound();
            }

            return View(businessEmployeeMap);
        }

        // POST: BusinessEmployeeMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessEmployeeMap = await _context.BusinessEmployeeMaps.FindAsync(id);
            if (businessEmployeeMap != null)
            {
                _context.BusinessEmployeeMaps.Remove(businessEmployeeMap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessEmployeeMapExists(int id)
        {
            return _context.BusinessEmployeeMaps.Any(e => e.ID == id);
        }
    }
}
