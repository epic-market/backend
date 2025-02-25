using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Authorization;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Data.ApplicationModels;
using System.Security.Claims;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;


namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public BlogsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            ViewData["BlogCategoryID"] = new SelectList(_context.BlogCategory, "Id", "Name");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageUrl,InnerHtml,Authour,BlogCategoryID,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Blog blog)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            blog.CreateBy = userName;
            blog.CreateDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddBlog,
                    EntityName = EntityConstants.Blog,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added blog '{blog.Title}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(blog),
                    RecordId = blog.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            ViewData["BlogCategoryID"] = new SelectList(_context.BlogCategory, "Id", "Name");
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageUrl,InnerHtml,Authour,BlogCategoryID,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Blog blog)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            blog.ModifiedBy = userName;
            blog.ModifiedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEntity = await _context.Blogs.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    _context.Update(blog);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditBlog,
                        EntityName = EntityConstants.Blog,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated blog '{blog.Title}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalEntity,
                            Updated = blog
                        }),
                        RecordId = blog.Id,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteBlog,
                    EntityName = EntityConstants.Blog,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted blog '{blog.Title}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(blog),
                    RecordId = blog.Id,
                    BusinessID = 0,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
