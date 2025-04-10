using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Admin.MVC.Data;
using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class OnboardingStepsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public OnboardingStepsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: OnboardingSteps
        [SecurableAuthorize(SecurableConstants.OnboardingStepsView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.OnboardingSteps.Include(n => n.Page).ToListAsync());
        }

        // GET: OnboardingSteps/Details/5
        [SecurableAuthorize(SecurableConstants.OnboardingStepsView)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingStep = await _context.OnboardingSteps
                   .Include(n => n.Page)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onboardingStep == null)
            {
                return NotFound();
            }

            return View(onboardingStep);
        }

        // GET: OnboardingSteps/Create
        [SecurableAuthorize(SecurableConstants.OnboardingStepsAdd)]
        public IActionResult Create()
        {
            ViewData["PageId"] = new SelectList(_context.Pages, "ID", "Url");
            return View();
        }

        // POST: OnboardingSteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.OnboardingStepsAdd)]
        public async Task<IActionResult> Create([Bind("Id,StepName,StepDescription,StepOrder,PageId")] OnboardingStep onboardingStep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onboardingStep);
                await _context.SaveChangesAsync();
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.CreateOnboardingStep,
                    EntityName = EntityConstants.OnboardingStep,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Created onboarding step: {onboardingStep.StepName}",
                    Data = System.Text.Json.JsonSerializer.Serialize(onboardingStep),
                    RecordId = onboardingStep.Id,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageId"] = new SelectList(_context.Pages, "ID", "Url", onboardingStep.PageId);
            return View(onboardingStep);
        }

        // GET: OnboardingSteps/Edit/5
        [SecurableAuthorize(SecurableConstants.OnboardingStepsEdit)]
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
            ViewData["PageId"] = new SelectList(_context.Pages, "ID", "Url", onboardingStep.Page);
            return View(onboardingStep);
        }

        // POST: OnboardingSteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.OnboardingStepsEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StepName,StepDescription,StepOrder,PageId")] OnboardingStep onboardingStep)
        {
            if (id != onboardingStep.Id)
            {
                return NotFound();
            }
            
            // Get original entity for event logging
            var originalEntity = await _context.OnboardingSteps
                .AsNoTracking()
                .FirstOrDefaultAsync(os => os.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onboardingStep);
                    await _context.SaveChangesAsync();
                    
                    // Log event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditOnboardingStep,
                        EntityName = EntityConstants.OnboardingStep,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated onboarding step: {onboardingStep.StepName}",
                        Data = System.Text.Json.JsonSerializer.Serialize(onboardingStep),
                        RecordId = onboardingStep.Id,
                        LoggedInUserName = User.Identity.Name
                    });
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
            ViewData["PageId"] = new SelectList(_context.Pages, "ID", "Url", onboardingStep.Page);
            return View(onboardingStep);
        }

        // GET: OnboardingSteps/Delete/5
        [SecurableAuthorize(SecurableConstants.OnboardingStepsDelete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingStep = await _context.OnboardingSteps
                 .Include(n => n.Page)
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
        [SecurableAuthorize(SecurableConstants.OnboardingStepsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var onboardingStep = await _context.OnboardingSteps.FindAsync(id);
            if (onboardingStep != null)
            {
                _context.OnboardingSteps.Remove(onboardingStep);
                
                // Log event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteOnboardingStep,
                    EntityName = EntityConstants.OnboardingStep,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted onboarding step: {onboardingStep.StepName}",
                    Data = System.Text.Json.JsonSerializer.Serialize(onboardingStep),
                    RecordId = onboardingStep.Id,
                    LoggedInUserName = User.Identity.Name
                });
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
