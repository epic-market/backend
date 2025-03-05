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
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.ADMIN},{ROLES.ROOT}")]
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService _attachmentService;
        private readonly IFileService _fileService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public AttachmentsController(
            IAttachmentService attachmentService,
            IFileService fileService,
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _attachmentService = attachmentService;
            _fileService = fileService;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Attachments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attachments.ToListAsync());
        }

        // GET: Attachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        public async Task<IActionResult> DownloadImage(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("File key is required");
            }

            try
            {
                // Normalize the key by ensuring it doesn't have double slashes
                key = key.Replace("//", "/").TrimStart('/');
                
                var fileDto = await _fileService.GetFileByKeyAsync(key);
                if (fileDto == null || fileDto.fileStream == null)
                {
                    return NotFound("File not found");
                }
                
                // Extract the original filename from the key
                string fileName = Path.GetFileName(key);
                
                // Try to find the attachment by matching the file path and name
                var attachment = await _context.Attachments
                    .FirstOrDefaultAsync(a => (a.DocumentFolderPath + a.DocumentFile) == key);
                
                if (attachment != null)
                {
                    // Use the attachment name with the original extension
                    string extension = Path.GetExtension(fileName);
                    fileName = $"{attachment.Name}{extension}";
                }

                // Set the content disposition to attachment to prompt download in the browser
                var result = new FileStreamResult(fileDto.fileStream, fileDto.contentType)
                {
                    FileDownloadName = fileName
                };

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred while downloading the file: {ex.Message}");
            }
        }

        // GET: Attachments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attachments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Comment,DocumentType,EntityId,DocumentFolderPath")] Attachment attachment, IFormFile fileUpload)
        {
            if (fileUpload == null || fileUpload.Length == 0)
            {
                ModelState.AddModelError("fileUpload", "Please select a file to upload.");
                return View(attachment);
            }

            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            attachment.CreateBy = userName;
            attachment.CreateDate = DateTime.UtcNow;
            attachment.ModifiedBy = userName;
            attachment.ModifiedDate = DateTime.UtcNow;
            attachment.IsActive = true;

            if (ModelState.IsValid)
            {
                try
                {
                    // Get file extension and set document type
                    var fileExtension = Path.GetExtension(fileUpload.FileName).ToLowerInvariant();
                    attachment.DocumentFileType = fileExtension;
                    
                    // If DocumentType is not provided, set it based on file extension
                    if (string.IsNullOrEmpty(attachment.DocumentType))
                    {
                        // Set document type based on file extension
                        if (new[] { ".pdf" }.Contains(fileExtension))
                        {
                            attachment.DocumentType = "PDF";
                        }
                        else if (new[] { ".doc", ".docx" }.Contains(fileExtension))
                        {
                            attachment.DocumentType = "Word";
                        }
                        else if (new[] { ".xls", ".xlsx" }.Contains(fileExtension))
                        {
                            attachment.DocumentType = "Excel";
                        }
                        else if (new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(fileExtension))
                        {
                            attachment.DocumentType = "Image";
                        }
                        else
                        {
                            attachment.DocumentType = "Other";
                        }
                    }

                    string entityName = EntityConstants.Attachment;
                    
                    // Use custom folder path if provided, otherwise use default
                    string prefix;
                    if (!string.IsNullOrEmpty(attachment.DocumentFolderPath))
                    {
                        // Ensure the path doesn't have leading/trailing slashes
                        prefix = attachment.DocumentFolderPath.Trim('/');
                    }
                    else
                    {
                        // Default path
                        prefix = $"{entityName}/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}";
                    }
                    
                    string fileNameKey = $"{System.Guid.NewGuid()}{fileExtension}";
                    
                    // Upload to S3 and get the path
                    string s3Path = await _fileService.UploadFileAsync(fileUpload, prefix, fileNameKey);
                    
                    // Set the document path and file name
                    var pathParts = s3Path.Split('/');
                    attachment.DocumentFile = pathParts[pathParts.Length - 1];
                    attachment.DocumentFolderPath = s3Path.Substring(0, s3Path.Length - attachment.DocumentFile.Length);

                    // Save to database
                    _context.Add(attachment);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.AddAttachment,
                        EntityName = EntityConstants.Attachment,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Added attachment '{attachment.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            AttachmentID = attachment.ID,
                            Name = attachment.Name,
                            Type = attachment.DocumentType,
                            FileType = attachment.DocumentFileType
                        }),
                        RecordId = attachment.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                    return View(attachment);
                }
            }
            
            return View(attachment);
        }

        // GET: Attachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            
            return View(attachment);
        }

        // POST: Attachments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Comment,DocumentType,DocumentFileType,DocumentFolderPath,DocumentFile,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive,EntityId")] Attachment attachment, IFormFile fileUpload)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            attachment.ModifiedBy = userName;
            attachment.ModifiedDate = DateTime.UtcNow;
            
            if (id != attachment.ID)
            {
                return NotFound();
            }

            // Get the original attachment to preserve values that shouldn't be changed
            var originalAttachment = await _context.Attachments.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == id);

            if (originalAttachment == null)
            {
                return NotFound();
            }

            // Preserve original values that shouldn't be changed
            attachment.Name = originalAttachment.Name;
            attachment.DocumentType = originalAttachment.DocumentType;
            attachment.EntityId = originalAttachment.EntityId;
            attachment.CreateDate = originalAttachment.CreateDate;
            attachment.CreateBy = originalAttachment.CreateBy;
            attachment.IsActive = originalAttachment.IsActive;

            if (ModelState.IsValid)
            {
                try
                {
                    // If a new file is uploaded, update the file information
                    if (fileUpload != null && fileUpload.Length > 0)
                    {
                        // Get file extension
                        var fileExtension = Path.GetExtension(fileUpload.FileName).ToLowerInvariant();
                        attachment.DocumentFileType = fileExtension;
                        
                        // Delete the old file from S3 if it exists
                        if (!string.IsNullOrEmpty(originalAttachment.DocumentFolderPath) && !string.IsNullOrEmpty(originalAttachment.DocumentFile))
                        {
                            string oldKey = originalAttachment.DocumentFolderPath + originalAttachment.DocumentFile;
                            await _fileService.DeleteFileAsync(oldKey);
                        }

                        // Use the existing folder path
                        string prefix = originalAttachment.DocumentFolderPath;
                        if (string.IsNullOrEmpty(prefix))
                        {
                            prefix = $"{EntityConstants.Attachment}/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}";
                        }
                        else
                        {
                            prefix = prefix.TrimEnd('/');
                        }
                        
                        string fileNameKey = $"{System.Guid.NewGuid()}{fileExtension}";
                        
                        // Upload to S3 to the same location
                        string entityName = EntityConstants.Attachment;
                        string s3Path = await _fileService.UploadFileAsync(fileUpload, prefix, fileNameKey);
                        
                        // Set the document path and file name
                        var pathParts = s3Path.Split('/');
                        attachment.DocumentFile = pathParts[pathParts.Length - 1];
                        attachment.DocumentFolderPath = s3Path.Substring(0, s3Path.Length - attachment.DocumentFile.Length);
                    }
                    else
                    {
                        // Keep the original file information if no new file is uploaded
                        attachment.DocumentFile = originalAttachment.DocumentFile;
                        attachment.DocumentFolderPath = originalAttachment.DocumentFolderPath;
                        attachment.DocumentFileType = originalAttachment.DocumentFileType;
                    }

                    _context.Update(attachment);
                    await _context.SaveChangesAsync();

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditAttachment,
                        EntityName = EntityConstants.Attachment,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated attachment '{attachment.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Original = new {
                                Comment = originalAttachment.Comment,
                                DocumentFile = originalAttachment.DocumentFile
                            },
                            Updated = new {
                                Comment = attachment.Comment,
                                DocumentFile = attachment.DocumentFile
                            }
                        }),
                        RecordId = attachment.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentExists(attachment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating attachment: {ex.Message}");
                    return View(attachment);
                }
            }
            
            return View(attachment);
        }

        // GET: Attachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attachment == null)
            {
                return NotFound();
            }
            
            return View(attachment);
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment != null)
            {
                try
                {
                    // Delete the file from S3 if it exists
                    if (!string.IsNullOrEmpty(attachment.DocumentFolderPath) && !string.IsNullOrEmpty(attachment.DocumentFile))
                    {
                        string key = attachment.DocumentFolderPath + attachment.DocumentFile;
                        await _fileService.DeleteFileAsync(key);
                    }
                    
                    // Remove from database
                    _context.Attachments.Remove(attachment);

                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.DeleteAttachment,
                        EntityName = EntityConstants.Attachment,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Deleted attachment '{attachment.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new {
                            AttachmentID = attachment.ID,
                            Name = attachment.Name,
                            Type = attachment.DocumentType,
                            FileType = attachment.DocumentFileType
                        }),
                        RecordId = attachment.ID,
                        BusinessID = 0,
                        LoggedInUserName = User.Identity.Name
                    });

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the error and show a friendly message
                    // You might want to log this to your error logging system
                    return RedirectToAction(nameof(Index), new { error = "Failed to delete attachment. Please try again later." });
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentExists(int id)
        {
            return _context.Attachments.Any(e => e.ID == id);
        }
    }
}
