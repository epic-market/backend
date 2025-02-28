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
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using EpicMarket.Admin.MVC.Models;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;
using EpicMarket.Admin.MVC.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace EpicMarket.Admin.MVC.Controllers
{
    [Authorize(Roles = $"{ROLES.SUPPORT},{ROLES.ROOT},{ROLES.ADMIN}")]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttachmentService _attachmentService;
        private readonly IEventService _eventService;
        private readonly IUrlContextService _urlContextService;

        public TasksController(
            IAttachmentService attachmentService,
            ApplicationDbContext context,
            IEventService eventService,
            IUrlContextService urlContextService)
        {
            _context = context;
            _attachmentService = attachmentService;
            _eventService = eventService;
            _urlContextService = urlContextService;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            // Load statuses for filter dropdown
            ViewBag.Statuses = await _context.TaskStatusTypes.ToListAsync();
            ViewBag.TaskTypes = await _context.TaskTypes.ToListAsync();
            
            // Return the view with empty model - data will be loaded via AJAX
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredData([FromBody] TaskFilterModel filter)
        {
            try
            {
                // Start with the base query
                var query = _context.Tasks
                    .Include(t => t.TaskStatusType)
                    .Include(t => t.TaskTypes)
                    .Include(t => t.AppUser)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.TaskId) && int.TryParse(filter.TaskId, out int taskId))
                {
                    query = query.Where(t => t.ID == taskId);
                }

                if (!string.IsNullOrEmpty(filter.TaskName))
                {
                    query = query.Where(t => t.Name.Contains(filter.TaskName));
                }

                if (!string.IsNullOrEmpty(filter.AssignedTo))
                {
                    query = query.Where(t => t.AppUser.UserName.Contains(filter.AssignedTo));
                }

                if (filter.StatusId.HasValue)
                {
                    query = query.Where(t => t.TaskStatusID == filter.StatusId);
                }

                if (filter.TypeId.HasValue)
                {
                    query = query.Where(t => t.TaskTypeID == filter.TypeId);
                }

                // Get total count for pagination
                var totalRecords = await query.CountAsync();

                // Apply sorting
                query = ApplySorting(query, filter.SortColumn, filter.SortDirection);

                // Apply pagination and project to DTO to avoid circular references
                var tasks = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(t => new TaskViewModel
                    {
                        ID = t.ID,
                        Name = t.Name,
                        Description = t.Description,
                        TaskTypeName = t.TaskTypes.Name,
                        TaskStatusName = t.TaskStatusType.Status,
                        ParentID = t.ParentID,
                        TaskPriorityID = t.TaskPriorityID,
                        PriorityText = GetPriorityText(t.TaskPriorityID),
                        AssignedToName = t.AppUser != null ? t.AppUser.UserName : "Unassigned",
                        AssignedToId = t.PrimaryAssignedToPersonID,
                        DateAssigned = t.DateAssigned,
                        DateDue = t.DateDue,
                        SubmittedByPersonID = t.SubmittedByPersonID,
                        IsOverdue = t.DateDue.HasValue && t.DateDue.Value < DateTime.Now && t.TaskStatusID != 3 // Assuming 3 is Completed
                    })
                    .ToListAsync();

                // Get dashboard stats
                var stats = await GetTaskStats();

                // Return the response
                return Json(new
                {
                    data = tasks,
                    totalRecords,
                    stats
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private IQueryable<Tasks> ApplySorting(IQueryable<Tasks> query, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumn))
            {
                // Default sort
                return query.OrderBy(t => t.ID);
            }

            var isAscending = string.IsNullOrEmpty(sortDirection) || sortDirection.ToLower() == "asc";

            return sortColumn.ToLower() switch
            {
                "id" => isAscending ? query.OrderBy(t => t.ID) : query.OrderByDescending(t => t.ID),
                "name" => isAscending ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name),
                "status" => isAscending ? query.OrderBy(t => t.TaskStatusType.Status) : query.OrderByDescending(t => t.TaskStatusType.Status),
                "type" => isAscending ? query.OrderBy(t => t.TaskTypes.Name) : query.OrderByDescending(t => t.TaskTypes.Name),
                "priority" => isAscending ? query.OrderBy(t => t.TaskPriorityID) : query.OrderByDescending(t => t.TaskPriorityID),
                "duedate" => isAscending ? query.OrderBy(t => t.DateDue) : query.OrderByDescending(t => t.DateDue),
                "assigned" => isAscending ? query.OrderBy(t => t.AppUser.UserName) : query.OrderByDescending(t => t.AppUser.UserName),
                _ => isAscending ? query.OrderBy(t => t.ID) : query.OrderByDescending(t => t.ID)
            };
        }

        private async Task<object> GetTaskStats()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            
            return new
            {
                TotalTasks = await _context.Tasks.CountAsync(),
                OpenTasks = await _context.Tasks.CountAsync(t => t.TaskStatusID != 3), // Assuming 3 is Completed
                OverdueTasks = await _context.Tasks.CountAsync(t => t.DateDue.HasValue && t.DateDue.Value < now && t.TaskStatusID != 3),
                NewThisMonth = await _context.Tasks.CountAsync(t => t.CreateDate >= startOfMonth)
            };
        }

        private static string GetPriorityText(int? priorityId)
        {
            return priorityId switch
            {
                1 => "Critical",
                2 => "High",
                3 => "Normal",
                4 => "Low",
                _ => "Unknown"
            };
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }




            var taskDetailsResult = new TaskDetailsModel();

            var taskDetails = await _context.Tasks
                .Include(t => t.TaskStatusType)
                .Include(t => t.TaskTypes)
                .Include(t => t.AppUser)
                .Include(t => t.Entity)
                .FirstOrDefaultAsync(m => m.ID == id);


         


           var SubTasks = await _context.Tasks.Include(t => t.AppUser)
                                             .Include(t => t.TaskStatusType)
                                             .Where(c=> c.ParentID == id).ToListAsync();

            var EntityForTasks = await _context.Entity.FirstOrDefaultAsync(m => m.Name == EntityConstants.Tasks);

            var eventLogs = await _context.EventLog.Where(c => c.RecordID == id && c.EntityID == EntityForTasks.ID).ToListAsync();

			var comments = await _context.Comments.Where(c => c.RecordID == id && c.EntityID == EntityForTasks.ID).ToListAsync();


            ViewBag.attachments = await this._attachmentService.GetAttachmentLinks(new GetAttachmentLink()
            {
                AttachmentType = AttachmentTypeConstants.TASK,
                Entity = EntityConstants.Tasks,
                RecordID = taskDetails.ID
            });



            taskDetailsResult.Task = taskDetails;
			taskDetailsResult.SubTasks = SubTasks;
			taskDetailsResult.EventLogs = eventLogs;
			taskDetailsResult.Comments = comments;


			if (taskDetailsResult.Task == null)
            {
                return NotFound();
            }

            return View(taskDetailsResult);
        }

        // GET: Tasks/Create


        [HttpGet]
        public IActionResult Create(int? parentTaskId)
        {
            ViewData["TaskStatusID"] = new SelectList(_context.Set<TaskStatusType>(), "Id", "Status");
            ViewData["TaskTypeID"] = new SelectList(_context.Set<TaskType>(), "ID", "Name");
            ViewData["ParentTaskId"] = new SelectList(_context.Set<Tasks>().Select(t => new { ID = t.ID, Name = t.Name }), "ID", "ID", parentTaskId);

            List<SelectListItem> numbers = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Critical" },
                new SelectListItem { Value = "2", Text = "High" },
                new SelectListItem { Value = "3", Text = "Normal", Selected = true  },
                new SelectListItem { Value = "4", Text = "Low" },
            };

            ViewData["Priority"] = numbers;
            ViewBag.ExistingID = parentTaskId;

            var admins = from userrole in _context.UserRoles
                         join role in _context.Roles on userrole.RoleId equals role.Id
                         join user in _context.Users on userrole.UserId equals user.Id
                         where role.Name == ROLES.ADMIN
                         select new { Id = user.Id, UserName = user.UserName };

            ViewData["Admin"] = new SelectList(admins, "Id", "UserName");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromQuery] int? parentTaskId, [Bind("ID,Name,Description,TaskTypeID,ParentID,TaskStatusID,TaskPriorityID,PrimaryAssignedToPersonID,DateAssigned,DateDue,DateStarted,DateCompleted,SubmittedByPersonID,EffectiveDate,TaskData,ReceivedDate,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Tasks tasks)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var GetTaskType = _context.TaskTypes.Where(row => row.ID == tasks.TaskTypeID)
                                                 .Select(t => new { ID = t.ID, Name = t.Name, DefaultDueDateHours = t.DefaultDueDateHours })
                                                 .FirstOrDefault();
            
            tasks.CreateBy = userName;
            tasks.CreateDate = DateTime.UtcNow;
            tasks.DateDue = GetTaskType.DefaultDueDateHours.HasValue
                            ? DateTime.Now.AddHours((double)GetTaskType.DefaultDueDateHours)
                            : DateTime.Now.AddDays(7);
            tasks.SubmittedByPersonID = UserID;
            tasks.ParentID = parentTaskId;
            tasks.DateAssigned = DateTime.UtcNow;
            tasks.ReceivedDate = DateTime.UtcNow;
            
            if (ModelState.IsValid)
            {
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.AddTask,
                    EntityName = EntityConstants.Tasks,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Added task '{tasks.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(tasks, new System.Text.Json.JsonSerializerOptions
                    {
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                        MaxDepth = 64
                    }),
                    RecordId = tasks.ID,
                    LoggedInUserName = User.Identity.Name
                });
                
                return RedirectToAction(nameof(Index));
            }

            var admins = from userrole in _context.UserRoles
                         join role in _context.Roles on userrole.RoleId equals role.Id
                         join user in _context.Users on userrole.UserId equals user.Id
                         where role.Name == ROLES.ADMIN
                         select new { Id = user.Id, UserName = user.UserName };

            List<SelectListItem> numbers = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Critical" },
                new SelectListItem { Value = "2", Text = "High" },
                new SelectListItem { Value = "3", Text = "Normal", Selected = true  },
                new SelectListItem { Value = "4", Text = "Low" },
            };

            ViewData["ParentTaskId"] = new SelectList(_context.Set<Tasks>().Select(t => new { ID = t.ID, Name = t.Name }), "ID", "ID", tasks.ParentID);
            ViewData["Priority"] = new SelectList(numbers, tasks.TaskPriorityID); 
            ViewData["Admin"] = new SelectList(admins, "Id", "UserName", tasks.PrimaryAssignedToPersonID);
            ViewData["TaskStatusID"] = new SelectList(_context.TaskStatusTypes, "Id", "Status", tasks.TaskStatusID);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", tasks.TaskTypeID);
            
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }
            ViewData["TaskStatusID"] = new SelectList(_context.Set<TaskStatusType>(), "Id", "Status");
            ViewData["TaskTypeID"] = new SelectList(_context.Set<TaskType>(), "ID", "Name");
            ViewData["ParentTaskId"] = new SelectList(_context.Set<Tasks>(), "ID", "ID");

            List<SelectListItem> numbers = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Critical" },
                new SelectListItem { Value = "2", Text = "High" },
                new SelectListItem { Value = "3", Text = "Normal", Selected = true  },
                new SelectListItem { Value = "4", Text = "Low" },
            };

            ViewData["Priority"] = numbers;


            var admins = from userrole in _context.UserRoles
                         join role in _context.Roles on userrole.RoleId equals role.Id
                         join user in _context.Users on userrole.UserId equals user.Id
                         where role.Name == ROLES.ADMIN
                         select user;

            ViewData["Admin"] = new SelectList(admins, "Id", "UserName");
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,TaskTypeID,ParentID,TaskStatusID,TaskPriorityID,PrimaryAssignedToPersonID,DateAssigned,DateDue,DateStarted,DateCompleted,SubmittedByPersonID,TaskData,ReceivedDate,CreateDate,CreateBy,ModifiedDate,ModifiedBy,IsActive")] Tasks tasks)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            tasks.ModifiedBy = userName;
            tasks.ModifiedDate = DateTime.UtcNow;
            
            if (id != tasks.ID)
            {
                return NotFound();
            }

            // Get the original entity for comparison
            var originalTask = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ID == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasks);
                    
                    // Log the event
                    await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                    {
                        EventName = EventConstants.EditTask,
                        EntityName = EntityConstants.Tasks,
                        Source = _urlContextService.CurrentPageUrl,
                        Description = $"Updated task '{tasks.Name}'",
                        Data = System.Text.Json.JsonSerializer.Serialize(new { 
                            Original = originalTask, 
                            Updated = tasks 
                        }, new System.Text.Json.JsonSerializerOptions
                        {
                            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                            MaxDepth = 64
                        }),
                        RecordId = tasks.ID,
                        LoggedInUserName = User.Identity.Name
                    });
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.ID))
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

            ViewData["TaskStatusID"] = new SelectList(_context.TaskStatusTypes, "Id", "Status", tasks.TaskStatusID);
            ViewData["TaskTypeID"] = new SelectList(_context.TaskTypes, "ID", "Name", tasks.TaskTypeID);
            return View(tasks);
        }



		[HttpPost]
		public IActionResult AddComment(int taskId, string commentText)
		{
			if (!string.IsNullOrWhiteSpace(commentText))
			{

				var EntityForTasks =  _context.Entity.FirstOrDefault(m => m.Name == EntityConstants.Tasks).ID;

				// Create a new comment object
				var newComment = new Comment
				{
					CreateBy = User.Identity.Name,
					CreateDate = DateTime.Now,
					CommentText = commentText,
                    EntityID = EntityForTasks,
                    RecordID = taskId

				};

                _context.Comments.Add(newComment);
                _context.SaveChanges();
				return RedirectToAction("Details", new { id = taskId });
			}

			// Handle the case where the comment is empty or invalid
			ModelState.AddModelError("", "Comment cannot be empty.");
			return View(); // Return the view with the model to display the error
		}

		// GET: Tasks/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
                .Include(t => t.TaskStatusType)
                .Include(t => t.TaskTypes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks != null)
            {
                _context.Tasks.Remove(tasks);
                
                // Log the event
                await _eventService.LogEvent(new EVENT_LOG_SAVE_PARAMS
                {
                    EventName = EventConstants.DeleteTask,
                    EntityName = EntityConstants.Tasks,
                    Source = _urlContextService.CurrentPageUrl,
                    Description = $"Deleted task '{tasks.Name}'",
                    Data = System.Text.Json.JsonSerializer.Serialize(tasks),
                    RecordId = tasks.ID,
                    LoggedInUserName = User.Identity.Name
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




		private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
    }
}
