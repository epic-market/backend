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
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Admin.MVC.Controllers
{

    [Authorize(Roles = $"{ROLES.SUPPORT},{ROLES.ROOT},{ROLES.ADMIN}")]
    public class PromotionalLeadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public PromotionalLeadsController(
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: PromotionalLeads
        public async Task<IActionResult> Index()
        {
            return View(await _context.PromotionalLeads.ToListAsync());
        }

        // GET: PromotionalLeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionalLeads = await _context.PromotionalLeads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotionalLeads == null)
            {
                return NotFound();
            }

            return View(promotionalLeads);
        }

        // GET: PromotionalLeads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PromotionalLeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Gmail,CreateDate,Time,WhichApplication")] PromotionalLeads promotionalLeads)
        {
            promotionalLeads.CreateDate = DateTime.UtcNow;
            promotionalLeads.Time = DateTime.Now.TimeOfDay;

            if (ModelState.IsValid)
            {
                _context.Add(promotionalLeads);
                await _context.SaveChangesAsync();
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddPromotionalLead,
                    EntityName = EntityConstants.PromotionalLead,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added promotional lead for '{promotionalLeads.Gmail}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(promotionalLeads),
                    RecordId = promotionalLeads.Id,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }
            return View(promotionalLeads);
        }

        // GET: PromotionalLeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionalLeads = await _context.PromotionalLeads.FindAsync(id);
            if (promotionalLeads == null)
            {
                return NotFound();
            }
            return View(promotionalLeads);
        }

        // POST: PromotionalLeads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Gmail,CreateDate,Time,WhichApplication")] PromotionalLeads promotionalLeads)
        {
            if (id != promotionalLeads.Id)
            {
                return NotFound();
            }

            // Get the original entity for comparison
            var originalPromotionalLead = await _context.PromotionalLeads
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotionalLeads);
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditPromotionalLead,
                        EntityName = EntityConstants.PromotionalLead,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated promotional lead for '{promotionalLeads.Gmail}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            Original = originalPromotionalLead, 
                            Updated = promotionalLeads 
                        }),
                        RecordId = promotionalLeads.Id,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionalLeadsExists(promotionalLeads.Id))
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
            return View(promotionalLeads);
        }

        // GET: PromotionalLeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionalLeads = await _context.PromotionalLeads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotionalLeads == null)
            {
                return NotFound();
            }

            return View(promotionalLeads);
        }

        // POST: PromotionalLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotionalLeads = await _context.PromotionalLeads.FindAsync(id);
            if (promotionalLeads != null)
            {
                _context.PromotionalLeads.Remove(promotionalLeads);
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeletePromotionalLead,
                    EntityName = EntityConstants.PromotionalLead,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted promotional lead for '{promotionalLeads.Gmail}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(promotionalLeads),
                    RecordId = promotionalLeads.Id,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PromotionalLeads/Upload
        public IActionResult Upload()
        {
            return View();
        }
        
        // POST: PromotionalLeads/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload.");
                return View();
            }
            
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".csv")
            {
                ModelState.AddModelError("File", "Please upload a CSV file.");
                return View();
            }
            
            try
            {
                var leads = new List<PromotionalLeads>();
                
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    // Skip header row
                    await reader.ReadLineAsync();
                    
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(',');
                        
                        if (values.Length >= 2) // At minimum, we need Gmail and WhichApplication
                        {
                            var lead = new PromotionalLeads
                            {
                                Gmail = values[0].Trim(),
                                WhichApplication = values[1].Trim(),
                                CreateDate = DateTime.UtcNow,
                                Time = DateTime.Now.TimeOfDay
                            };
                            
                            leads.Add(lead);
                        }
                    }
                }
                
                if (leads.Count > 0)
                {
                    await _context.PromotionalLeads.AddRangeAsync(leads);
                    await _context.SaveChangesAsync();
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.AddPromotionalLead,
                        EntityName = EntityConstants.PromotionalLead,
                        Source = _urlContextService.CurrentPageUrl,
                        RecordId = 0,
                        Description = $"Uploaded {leads.Count} promotional leads from CSV file",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { FileName = file.FileName, LeadCount = leads.Count }),
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    TempData["SuccessMessage"] = $"Successfully imported {leads.Count} promotional leads.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("File", "No valid data found in the uploaded file.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", $"Error processing file: {ex.Message}");
                return View();
            }
        }

        // GET: PromotionalLeads/DownloadSample
        public IActionResult DownloadSample()
        {
            // Create a sample CSV content
            var csvContent = "Gmail,WhichApplication\n" +
                             "john.doe@gmail.com,Mobile App\n" +
                             "jane.smith@gmail.com,Website\n" +
                             "user.example@gmail.com,Desktop App";
            
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            
            return File(bytes, "text/csv", "promotional_leads_sample.csv");
        }

        private bool PromotionalLeadsExists(int id)
        {
            return _context.PromotionalLeads.Any(e => e.Id == id);
        }
    }
}
