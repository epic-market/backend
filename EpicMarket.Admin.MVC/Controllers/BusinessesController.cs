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
using EpicMarket.Entities.CustomModels;
using System.Net.Mail;
using EpicMarket.Admin.MVC.Contracts;
using Microsoft.IdentityModel.Tokens;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN}")]
    public class BusinessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService attachmentService;
        private readonly IFileService fileService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public BusinessesController(
            ApplicationDbContext context, 
            IAttachmentService attachmentService, 
            IFileService fileService,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            this.attachmentService = attachmentService;
            this.fileService = fileService;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Business/GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] BusinessFilterViewModel filter)
        {
            try
            {
                var query = _context.Businesses
                    .Include(b => b.Address)
                    .Include(b => b.BusinessCategory)
                    .Include(b => b.Person)
                    .Include(b => b.Status)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.BusinessId))
                {
                    query = query.Where(b => b.ID.ToString().Contains(filter.BusinessId));
                }

                if (!string.IsNullOrWhiteSpace(filter.OwnerUsername))
                {
                    query = query.Where(b => b.Person.UserName.Contains(filter.OwnerUsername));
                }

                if (!string.IsNullOrWhiteSpace(filter.ContactNumber))
                {
                    query = query.Where(b => b.ContactNumber.ToString().Contains(filter.ContactNumber));
                }

                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = filter.SortColumn?.ToLower() switch
                {
                    "id" => filter.SortDirection == "asc" ? query.OrderBy(b => b.ID) : query.OrderByDescending(b => b.ID),
                    "name" => filter.SortDirection == "asc" ? query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name),
                    "contactnumber" => filter.SortDirection == "asc" ? query.OrderBy(b => b.ContactNumber) : query.OrderByDescending(b => b.ContactNumber),
                    _ => query.OrderBy(b => b.ID)
                };

                // Apply pagination and project to DTO
                var businesses = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(b => new BusinessDto
                    {
                        ID = b.ID,
                        Name = b.Name,
                        ContactNumber = b.ContactNumber,
                        ContactEmail = b.ContactEmail,
                        StatusName = b.Status.Status,
                        PersonUserName = b.Person.UserName,
                        PersonId = b.Person.Id,
                        BusinessCategoryName = b.BusinessCategory.Name,
                        City = b.Address.City
                    })
                    .ToListAsync();

                return Json(new { totalRecords, data = businesses });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
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


			var attachmentTypeID_LOGO = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.LOGO);
            var attachmentTypeID_PROOF = await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.PROOF);



             var attachments_logo = from attachment in _context.Attachments
                              join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                              join entity in _context.Entity on link.EntityID equals entity.ID
                              where entity.Name == EntityConstants.Business && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_LOGO.ID
                              select new
                              {
                                  ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                              };

            var attachments_proof = from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            where entity.Name == EntityConstants.Business && link.RecordID == id && link.AttachmentTypeID == attachmentTypeID_PROOF.ID
                            orderby attachment.CreateDate descending
                            select new
                            {
                                ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}",
                                Name = $"{attachment.DocumentFile}"
							};


            ViewBag.AttachmentLogo = attachments_logo.Select(a => a.ImagePath).FirstOrDefault();
            ViewBag.AttachmentProof = attachments_proof.ToList();
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
        public async Task<IActionResult> Create([Bind("ID,PersonID,BusinessCategoryID,Name,Description,Banner,Logo,ContactNumber,ContactEmail,AddressID,Rating,ReviewCount,IsOpen,Weight,StatusId,CreateDate,CreateBy,ModifiedDate,ModifiedBy,Address")] Business business, IFormFile[] Images, IFormFile[] Proofs)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            business.Address.CreateBy = userName;
            business.Address.CreateDate = DateTime.UtcNow;
            business.CreateBy = userName;
            business.CreateDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(business);
                await _context.SaveChangesAsync();

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddBusiness,
                    EntityName = EntityConstants.Business,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added business '{business.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(business),
                    RecordId = business.ID,
                    BusinessID = business.ID,
                    LoggedInUserName = User.Identity.Name
                });

                if (Images?.Length > 0) {
                    var attachmentLogo = new AttachmentModel()
                    {
                        Name = EntityConstants.Business,
                        Comment = EntityConstants.Business,
                        RecordID = business.ID,
                        Entity = EntityConstants.Business,
                        AttachmentType = AttachmentTypeConstants.LOGO,
                        Files = Images
                    };
                    await attachmentService.UploadAttachment(attachmentLogo);
                }

                if (Proofs?.Length > 0)
                {
                    var attachmentProofs = new AttachmentModel()
                    {
                        Name = EntityConstants.Business,
                        Comment = EntityConstants.Business,
                        RecordID = business.ID,
                        Entity = EntityConstants.Business,
                        AttachmentType = AttachmentTypeConstants.PROOF,
                        Files = Proofs
                    };
                    await attachmentService.UploadAttachment(attachmentProofs);
                }
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
            ViewBag.attachments = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.LOGO,
                Entity = EntityConstants.Business,
                RecordID = business.ID
            });

            ViewBag.Proofs = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.PROOF,
                Entity = EntityConstants.Business,
                RecordID = business.ID
            });


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
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,BusinessCategoryID,Name,Description,Banner,Logo,ContactNumber,ContactEmail,AddressID,Rating,ReviewCount,IsOpen,Weight,CreateDate,CreateBy,ModifiedDate,ModifiedBy,StatusId,Address,IsActive")] Business business, IFormFile[] newLogo, string? addedLogos, IFormFile[] newProofs, string? removedProofs, string? addedProofs)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;

            business.Address.ModifiedBy = userName;
            business.Address.ModifiedDate = DateTime.UtcNow;
            business.Address.Id = (int)business.AddressID;
            business.ModifiedBy = userName;
            business.ModifiedDate = DateTime.UtcNow;

            if (id != business.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalBusiness = await _context.Businesses
                        .AsNoTracking()
                        .Include(b => b.Address)
                        .FirstOrDefaultAsync(b => b.ID == id);

                    _context.Update(business);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditBusiness,
                        EntityName = EntityConstants.Business,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated business '{business.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = originalBusiness,
                            Updated = business
                        }),
                        RecordId = business.ID,
                        BusinessID = business.ID,
                        LoggedInUserName = User.Identity.Name
                    });

                    // Handle added images
                    if (addedLogos != null && newLogo?.Length > 0)
                    {
                        List<string> ExisitingLogo = await this.attachmentService.GetAttachmentLinks(new GetAttachmentLink()
                        {
                            AttachmentType = AttachmentTypeConstants.LOGO,
                            Entity = EntityConstants.Business,
                            RecordID = business.ID
                        });
                        if (ExisitingLogo?.Count > 0)
                        {
                            await this.fileService.DeleteImage(ExisitingLogo, userName);
                        }

                        var attachment = new AttachmentModel()
                        {
                            Name = EntityConstants.Business,
                            Comment = EntityConstants.Business,
                            RecordID = business.ID,
                            Entity = EntityConstants.Business,
                            AttachmentType = AttachmentTypeConstants.LOGO,
                            Files = newLogo
                        };
                        await attachmentService.UploadAttachment(attachment);
                    }

                    // Handle removed images
                    if (!string.IsNullOrEmpty(removedProofs))
                    {
                        var removedImageUrls = removedProofs.Split(',').ToList();
                        await this.fileService.DeleteImage(removedImageUrls, userName);
                    }

                    // Handle added images
                    if (addedProofs != null && newProofs?.Length > 0)
                    {
                        var attachmentProofs = new AttachmentModel()
                        {
                            Name = EntityConstants.Business,
                            Comment = EntityConstants.Business,
                            RecordID = business.ID,
                            Entity = EntityConstants.Business,
                            AttachmentType = AttachmentTypeConstants.PROOF,
                            Files = newProofs
                        };
                        await attachmentService.UploadAttachment(attachmentProofs);
                    }
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

                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteBusiness,
                    EntityName = EntityConstants.Business,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted business '{business.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(business),
                    RecordId = business.ID,
                    BusinessID = business.ID,
                    LoggedInUserName = User.Identity.Name
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessExists(int id)
        {
            return _context.Businesses.Any(e => e.ID == id);
        }
    }
}
