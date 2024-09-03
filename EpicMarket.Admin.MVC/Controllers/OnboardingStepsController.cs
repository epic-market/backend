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
    public class OnboardingStepsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OnboardingStepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OnboardingSteps
        public async Task<IActionResult> Index()
        {
            return View(await _context.OnboardingSteps.ToListAsync());
        }

        // GET: OnboardingSteps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingStep = await _context.OnboardingSteps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onboardingStep == null)
            {
                return NotFound();
            }

            return View(onboardingStep);
        }

        // GET: OnboardingSteps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OnboardingSteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StepName,StepDescription,NavigationURL,StepOrder,CreatedAt")] OnboardingStep onboardingStep)
        {
            onboardingStep.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(onboardingStep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(onboardingStep);
        }

        // GET: OnboardingSteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingStep = await _context.OnboardingSteps.FindAsync(id);
            if (onboardingStep == null)
            {
                return NotFound();
            }
            return View(onboardingStep);
        }

        // POST: OnboardingSteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StepName,StepDescription,NavigationURL,StepOrder,CreatedAt")] OnboardingStep onboardingStep)
        {
            if (id != onboardingStep.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onboardingStep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OnboardingStepExists(onboardingStep.Id))
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
            return View(onboardingStep);
        }

        // GET: OnboardingSteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingStep = await _context.OnboardingSteps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onboardingStep == null)
            {
                return NotFound();
            }

            return View(onboardingStep);
        }

        // POST: OnboardingSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var onboardingStep = await _context.OnboardingSteps.FindAsync(id);
            if (onboardingStep != null)
            {
                _context.OnboardingSteps.Remove(onboardingStep);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OnboardingStepExists(int id)
        {
            return _context.OnboardingSteps.Any(e => e.Id == id);
        }
    }
}
