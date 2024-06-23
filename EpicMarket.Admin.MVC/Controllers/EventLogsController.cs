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
    public class EventLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventLogs
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.EventLog.Include(e => e.Entity).Include(e => e.Event);
            return View(await authDbContext.ToListAsync());
        }

        // GET: EventLogs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLog = await _context.EventLog
                .Include(e => e.Entity)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventLog == null)
            {
                return NotFound();
            }

            return View(eventLog);
        }

        private bool EventLogExists(long id)
        {
            return _context.EventLog.Any(e => e.ID == id);
        }
    }
}
