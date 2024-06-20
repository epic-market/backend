using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using EpicMarket.Admin.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;
using System.Security.Claims;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class FAQCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FAQCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FAQCategories
        //public async Task<IActionResult> Index(string search="",string orderBy="",int currentPage=1)
        //{

        //    var viewModel = new FAQCategoryModel();
        //    search = string.IsNullOrEmpty(search) ? "" : search.ToLower();

        //    var data =  _context.FAQCategories.Where(row => row.CategoryTitle.Contains(search));
        //    var totalRecords = data.Count();
        //    int pageSize = 1;
        //    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        //    var pagenateddata = await data.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        //    viewModel.CurrentPage = currentPage;
        //    viewModel.TotalPages = totalPages;
        //    viewModel.PageSize = pageSize;
        //    viewModel.Search = search;
        //    viewModel.FAQCategory = pagenateddata;
        //    return View(viewModel);
        //}

        public async Task<IActionResult> Index()
        {
            var faqDbContext = _context.FAQCategories;
            return View(await faqDbContext.ToListAsync());
        }

        // GET: FAQCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQCategory = await _context.FAQCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fAQCategory == null)
            {
                return NotFound();
            }

            return View(fAQCategory);
        }

        // GET: FAQCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FAQCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryTitle,TypeOfFAQ,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] FAQCategory fAQCategory)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            fAQCategory.CreateBy = userName;
            fAQCategory.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(fAQCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fAQCategory);
        }

        // GET: FAQCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var fAQCategory = await _context.FAQCategories.FindAsync(id);
            if (fAQCategory == null)
            {
                return NotFound();
            }
            return View(fAQCategory);
        }

        // POST: FAQCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryTitle,TypeOfFAQ,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] FAQCategory fAQCategory)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            fAQCategory.ModifiedBy = userName;
            fAQCategory.ModifiedDate = DateTime.UtcNow;
            if (id != fAQCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fAQCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQCategoryExists(fAQCategory.Id))
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
            return View(fAQCategory);
        }

        // GET: FAQCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQCategory = await _context.FAQCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fAQCategory == null)
            {
                return NotFound();
            }

            return View(fAQCategory);
        }

        // POST: FAQCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fAQCategory = await _context.FAQCategories.FindAsync(id);
            if (fAQCategory != null)
            {
                _context.FAQCategories.Remove(fAQCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FAQCategoryExists(int id)
        {
            return _context.FAQCategories.Any(e => e.Id == id);
        }
    }
}
