using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis.Elfie.Model;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Models;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class BusinessesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Businesses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Businesses.Include(b => b.Address).Include(b => b.BusinessCategory).Include(b => b.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Businesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var businessDetailModel = new BusinessDetailModel();

			var business = await _context.Businesses
                .Include(b => b.Address)
                .Include(b => b.BusinessCategory)
                .Include(b => b.Person)
                .Include(b=> b.Status)
                .FirstOrDefaultAsync(m => m.ID == id);



			var braches = await _context.Outlets
                         .Include(b => b.Address)        
				        .Where(m => m.BussinessID == id)
                        .ToListAsync();

			var employees = await _context.BusinessEmployeeMaps
		                    .Include(b => b.Employee)
		                    .Where(m => m.BussinessID == id)
		                    .ToListAsync();



			var products = await _context.Catalogs
							.Where(m => m.BusinessID == id)
							.ToListAsync();


			businessDetailModel.Business = business;
			businessDetailModel.Outlets = braches;
			businessDetailModel.employees = employees;
			businessDetailModel.Catalogs = products;

			if (business == null)
            {
                return NotFound();
            }

            return View(businessDetailModel);
        }

        // GET: Businesses/Create
        public IActionResult Create()
        {
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "Name");
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["StatusID"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View();
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PersonID,BusinessCategoryID,Name,Description,Banner,Logo,ContactNumber,ContactEmail,AddressID,Rating,ReviewCount,IsOpen,Weight,Status,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Business business)
        {

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;


            business.Address.CreateBy = userName;
            business.Address.CreateDate = DateTime.Today;
            business.CreateBy = userName;
            business.CreateDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                _context.Add(business);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "Name");
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["StatusID"] = new SelectList(_context.StatusOptionSets, "Id", "Status");
            return View(business);
        }

        // GET: Businesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses.Where(b=>b.ID == id).Include(c=>c.Address).FirstOrDefaultAsync();
            if (business == null)
            {
                return NotFound();
            }
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "Name", business.BusinessCategoryID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "UserName", business.PersonID);
			ViewData["StatusID"] = new SelectList(_context.StatusOptionSets, "Id", "Status", business.StatusId);
			return View(business);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,BusinessCategoryID,Name,Description,Banner,Logo,ContactNumber,ContactEmail,AddressID,Rating,ReviewCount,IsOpen,Weight,CreateDate,CreateBy,ModifiedDate,ModifiedBy,StatusId,Address")] Business business)
        {


            var userName = this.User.FindFirst(ClaimTypes.Name).Value;


            business.Address.ModifiedBy = userName;
            business.Address.ModifiedDate = DateTime.Today;
            business.Address.Id = business.AddressID;
            business.ModifiedBy = userName;
            business.ModifiedDate = DateTime.Today;

            if (id != business.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(business);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessExists(business.ID))
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
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Id", business.AddressID);
            ViewData["BusinessCategoryID"] = new SelectList(_context.BusinessCategories, "ID", "ID", business.BusinessCategoryID);
            ViewData["PersonID"] = new SelectList(_context.Users, "Id", "Id", business.PersonID);
            return View(business);
        }

        // GET: Businesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses
                .Include(b => b.Address)
                .Include(b => b.BusinessCategory)
                .Include(b => b.Person)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var business = await _context.Businesses.FindAsync(id);
            if (business != null)
            {
                _context.Businesses.Remove(business);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(e => e.ID == id);
        }
    }
}
