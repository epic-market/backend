using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class OrderStatusOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderStatusOptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderStatusOptions.ToListAsync());
        }

        // GET: OrderStatusOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderStatusOptions = await _context.OrderStatusOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderStatusOptions == null)
            {
                return NotFound();
            }

            return View(orderStatusOptions);
        }

        // GET: OrderStatusOptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderStatusOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderStatus")] OrderStatusOptions orderStatusOptions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderStatusOptions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderStatusOptions);
        }

        // GET: OrderStatusOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderStatusOptions = await _context.OrderStatusOptions.FindAsync(id);
            if (orderStatusOptions == null)
            {
                return NotFound();
            }
            return View(orderStatusOptions);
        }

        // POST: OrderStatusOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderStatus")] OrderStatusOptions orderStatusOptions)
        {
            if (id != orderStatusOptions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderStatusOptions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderStatusOptionsExists(orderStatusOptions.Id))
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
            return View(orderStatusOptions);
        }

        // GET: OrderStatusOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderStatusOptions = await _context.OrderStatusOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderStatusOptions == null)
            {
                return NotFound();
            }

            return View(orderStatusOptions);
        }

        // POST: OrderStatusOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderStatusOptions = await _context.OrderStatusOptions.FindAsync(id);
            if (orderStatusOptions != null)
            {
                _context.OrderStatusOptions.Remove(orderStatusOptions);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderStatusOptionsExists(int id)
        {
            return _context.OrderStatusOptions.Any(e => e.Id == id);
        }
    }
}
