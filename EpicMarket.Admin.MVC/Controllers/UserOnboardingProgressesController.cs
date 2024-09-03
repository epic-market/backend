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
    public class UserOnboardingProgressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserOnboardingProgressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserOnboardingProgresses
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.UserOnboardingProgresses.Include(u => u.Step).Include(u => u.User);
            return View(await authDbContext.ToListAsync());
        }

        // GET: UserOnboardingProgresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnboardingProgress = await _context.UserOnboardingProgresses
                .Include(u => u.Step)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOnboardingProgress == null)
            {
                return NotFound();
            }

            return View(userOnboardingProgress);
        }

        // GET: UserOnboardingProgresses/Create
        public IActionResult Create()
        {
            ViewData["StepID"] = new SelectList(_context.OnboardingSteps, "Id", "StepName");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserOnboardingProgresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,StepID,CompletedAt,IsCompleted")] UserOnboardingProgress userOnboardingProgress)
        {


            if (userOnboardingProgress.IsCompleted)
            { 
                userOnboardingProgress.CompletedAt = DateTime.Now;
            }


            if (ModelState.IsValid)
            {
                _context.Add(userOnboardingProgress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StepID"] = new SelectList(_context.OnboardingSteps, "Id", "StepName", userOnboardingProgress.StepID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", userOnboardingProgress.UserID);
            return View(userOnboardingProgress);
        }

        // GET: UserOnboardingProgresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnboardingProgress = await _context.UserOnboardingProgresses.FindAsync(id);
            if (userOnboardingProgress == null)
            {
                return NotFound();
            }
            ViewData["StepID"] = new SelectList(_context.OnboardingSteps, "Id", "StepName", userOnboardingProgress.StepID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", userOnboardingProgress.UserID);
            return View(userOnboardingProgress);
        }

        // POST: UserOnboardingProgresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,StepID,CompletedAt,IsCompleted")] UserOnboardingProgress userOnboardingProgress)
        {


            if (userOnboardingProgress.IsCompleted)
            {
                userOnboardingProgress.CompletedAt = DateTime.Now;
            }

            if (id != userOnboardingProgress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOnboardingProgress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOnboardingProgressExists(userOnboardingProgress.Id))
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
            ViewData["StepID"] = new SelectList(_context.OnboardingSteps, "Id", "StepName", userOnboardingProgress.StepID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", userOnboardingProgress.UserID);
            return View(userOnboardingProgress);
        }

        // GET: UserOnboardingProgresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnboardingProgress = await _context.UserOnboardingProgresses
                .Include(u => u.Step)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOnboardingProgress == null)
            {
                return NotFound();
            }

            return View(userOnboardingProgress);
        }

        // POST: UserOnboardingProgresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userOnboardingProgress = await _context.UserOnboardingProgresses.FindAsync(id);
            if (userOnboardingProgress != null)
            {
                _context.UserOnboardingProgresses.Remove(userOnboardingProgress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOnboardingProgressExists(int id)
        {
            return _context.UserOnboardingProgresses.Any(e => e.Id == id);
        }
    }
}
