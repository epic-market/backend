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
    public class OrderTypesOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderTypesOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderTypesOptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderTypesOptions.ToListAsync());
        }

        // GET: OrderTypesOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTypesOptions = await _context.OrderTypesOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderTypesOptions == null)
            {
                return NotFound();
            }

            return View(orderTypesOptions);
        }

        // GET: OrderTypesOptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderTypesOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ordertype")] OrderTypesOptions orderTypesOptions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderTypesOptions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderTypesOptions);
        }

        // GET: OrderTypesOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTypesOptions = await _context.OrderTypesOptions.FindAsync(id);
            if (orderTypesOptions == null)
            {
                return NotFound();
            }
            return View(orderTypesOptions);
        }

        // POST: OrderTypesOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ordertype")] OrderTypesOptions orderTypesOptions)
        {
            if (id != orderTypesOptions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderTypesOptions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderTypesOptionsExists(orderTypesOptions.Id))
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
            return View(orderTypesOptions);
        }

        // GET: OrderTypesOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTypesOptions = await _context.OrderTypesOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderTypesOptions == null)
            {
                return NotFound();
            }

            return View(orderTypesOptions);
        }

        // POST: OrderTypesOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderTypesOptions = await _context.OrderTypesOptions.FindAsync(id);
            if (orderTypesOptions != null)
            {
                _context.OrderTypesOptions.Remove(orderTypesOptions);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderTypesOptionsExists(int id)
        {
            return _context.OrderTypesOptions.Any(e => e.Id == id);
        }
    }
}
