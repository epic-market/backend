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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ROOT}")]
    public class ContactMethodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public ContactMethodsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: ContactMethods
        [SecurableAuthorize(SecurableConstants.ContactMethodsView)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactMethod.ToListAsync());
        }

        // GET: ContactMethods/Details/5
        [SecurableAuthorize(SecurableConstants.ContactMethodsView)]
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
        [SecurableAuthorize(SecurableConstants.ContactMethodsAdd)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecurableAuthorize(SecurableConstants.ContactMethodsAdd)]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] ContactMethod contactMethod)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            contactMethod.CreateBy = userName;
            contactMethod.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(contactMethod);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddContactMethod,
                    EntityName = EntityConstants.ContactMethod,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added contact method '{contactMethod.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(contactMethod),
                    RecordId = contactMethod.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(contactMethod);
        }

        // GET: ContactMethods/Edit/5
        [SecurableAuthorize(SecurableConstants.ContactMethodsEdit)]
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
        [SecurableAuthorize(SecurableConstants.ContactMethodsEdit)]
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
                    var originalEntity = await _context.ContactMethod.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(contactMethod);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditContactMethod,
                        EntityName = EntityConstants.ContactMethod,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated contact method '{contactMethod.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = contactMethod
                        }),
                        RecordId = contactMethod.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
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
            }
            return View(contactMethod);
        }

        // GET: ContactMethods/Delete/5
        [SecurableAuthorize(SecurableConstants.ContactMethodsDelete)]
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
        [SecurableAuthorize(SecurableConstants.ContactMethodsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactMethod = await _context.ContactMethod.FindAsync(id);
            if (contactMethod != null)
            {
                _context.ContactMethod.Remove(contactMethod);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteContactMethod,
                    EntityName = EntityConstants.ContactMethod,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted contact method '{contactMethod.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(contactMethod),
                    RecordId = contactMethod.ID,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContactMethodExists(int id)
        {
            return _context.ContactMethod.Any(e => e.ID == id);
        }
    }
}
