using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class SupportTicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportTicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupportTickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SupportTickets.Include(s => s.Person).Include(s => s.TicketType);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost, ActionName("GetData")]
        public async Task<IActionResult> GetData()
        {
            var applicationDbContext = _context.SupportTickets.Include(s => s.Person).Include(s => s.TicketType);
            return View(await applicationDbContext.ToListAsync());
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoadData()
        {

            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var ticketData = (from tempTicket in _context.SupportTickets.Include(s => s.Person).Include(s => s.TicketType)
                                  select tempTicket);

                if (!string.IsNullOrEmpty(searchValue))
                {
                    ticketData = ticketData.Where(m => m.Description.Contains(searchValue)
                                                        || m.Person.UserName.Contains(searchValue)
                                                        || m.TicketType.Name.Contains(searchValue));
                }

                recordsTotal = ticketData.Count();
                var data = await ticketData.Skip(skip).Take(pageSize).ToListAsync();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                var json_data = new JsonResult(jsonData);

                return json_data;
            } catch (Exception)
            {
                throw new Exception("error");
            }
           
        }











        // GET: SupportTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportTicket = await _context.SupportTickets
                .Include(s => s.Person)
                .Include(s => s.TicketType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supportTicket == null)
            {
                return NotFound();
            }

            return View(supportTicket);
        }

        // GET: SupportTickets/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TicketTypeID"] = new SelectList(_context.SupportTicketTypes, "ID", "ID");
            return View();
        }

        // POST: SupportTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TicketTypeID,Attachment,Description,PersonId")] SupportTicket supportTicket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportTicket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", supportTicket.PersonId);
            ViewData["TicketTypeID"] = new SelectList(_context.SupportTicketTypes, "ID", "ID", supportTicket.TicketTypeID);
            return View(supportTicket);
        }

        // GET: SupportTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportTicket = await _context.SupportTickets.FindAsync(id);
            if (supportTicket == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", supportTicket.PersonId);
            ViewData["TicketTypeID"] = new SelectList(_context.SupportTicketTypes, "ID", "ID", supportTicket.TicketTypeID);
            return View(supportTicket);
        }

        // POST: SupportTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TicketTypeID,Attachment,Description,PersonId")] SupportTicket supportTicket)
        {
            if (id != supportTicket.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportTicketExists(supportTicket.ID))
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
            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", supportTicket.PersonId);
            ViewData["TicketTypeID"] = new SelectList(_context.SupportTicketTypes, "ID", "ID", supportTicket.TicketTypeID);
            return View(supportTicket);
        }

        // GET: SupportTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportTicket = await _context.SupportTickets
                .Include(s => s.Person)
                .Include(s => s.TicketType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supportTicket == null)
            {
                return NotFound();
            }

            return View(supportTicket);
        }

        // POST: SupportTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportTicket = await _context.SupportTickets.FindAsync(id);
            if (supportTicket != null)
            {
                _context.SupportTickets.Remove(supportTicket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportTicketExists(int id)
        {
            return _context.SupportTickets.Any(e => e.ID == id);
        }
    }
}
