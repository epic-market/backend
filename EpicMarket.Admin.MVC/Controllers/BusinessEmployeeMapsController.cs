using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Admin.MVC.Attributes;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{
    [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsView)]
    public class BusinessEmployeeMapsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public BusinessEmployeeMapsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: BusinessEmployeeMaps
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsView)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BusinessEmployeeMaps.Include(b => b.Bussiness).Include(b => b.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BusinessEmployeeMaps/Details/5
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsView)]
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
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsAdd)]
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
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsAdd)]
        public async Task<IActionResult> Create([Bind("ID,BussinessID,EmployeeID,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BusinessEmployeeMap businessEmployeeMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessEmployeeMap);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddBusinessEmployeeMap,
                    EntityName = EntityConstants.BusinessEmployeeMap,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added business employee map for Business ID: {businessEmployeeMap.BussinessID}, Employee ID: {businessEmployeeMap.EmployeeID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(businessEmployeeMap),
                    RecordId = businessEmployeeMap.ID,
                    BusinessID = businessEmployeeMap.BussinessID,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", businessEmployeeMap.BussinessID);
            ViewData["EmployeeID"] = new SelectList(_context.Users, "Id", "Id", businessEmployeeMap.EmployeeID);
            return View(businessEmployeeMap);
        }

        // GET: BusinessEmployeeMaps/Edit/5
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsEdit)]
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
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsEdit)]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BussinessID,EmployeeID,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] BusinessEmployeeMap businessEmployeeMap)
        {
            if (id != businessEmployeeMap.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.BusinessEmployeeMaps.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == id);

                    _context.Update(businessEmployeeMap);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditBusinessEmployeeMap,
                        EntityName = EntityConstants.BusinessEmployeeMap,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated business employee map for Business ID: {businessEmployeeMap.BussinessID}, Employee ID: {businessEmployeeMap.EmployeeID}",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = businessEmployeeMap
                        }),
                        RecordId = businessEmployeeMap.ID,
                        BusinessID = businessEmployeeMap.BussinessID,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
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
            }
            ViewData["BussinessID"] = new SelectList(_context.Businesses, "ID", "ID", businessEmployeeMap.BussinessID);
            ViewData["EmployeeID"] = new SelectList(_context.Users, "Id", "Id", businessEmployeeMap.EmployeeID);
            return View(businessEmployeeMap);
        }

        // GET: BusinessEmployeeMaps/Delete/5
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsDelete)]
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
        [SecurableAuthorize(SecurableConstants.BusinessEmployeeMapsDelete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessEmployeeMap = await _context.BusinessEmployeeMaps.FindAsync(id);
            if (businessEmployeeMap != null)
            {
                _context.BusinessEmployeeMaps.Remove(businessEmployeeMap);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteBusinessEmployeeMap,
                    EntityName = EntityConstants.BusinessEmployeeMap,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted business employee map for Business ID: {businessEmployeeMap.BussinessID}, Employee ID: {businessEmployeeMap.EmployeeID}",
                    Data = System.Text.Json.JsonSerializer.Serialize(businessEmployeeMap),
                    RecordId = businessEmployeeMap.ID,
                    BusinessID = businessEmployeeMap.BussinessID,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessEmployeeMapExists(int id)
        {
            return _context.BusinessEmployeeMaps.Any(e => e.ID == id);
        }
    }
}
