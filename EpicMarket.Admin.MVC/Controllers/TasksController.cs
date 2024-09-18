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

namespace EpicMarket.Admin.MVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Tasks.Include(t => t.TaskStatusType).Include(t => t.TaskTypes);
            return View(await authDbContext.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.ID == id);



            var SubTasks = await _context.Tasks.Where(c=> c.ParentID == id).ToListAsync();

            var EntityForTasks = await _context.Entity.FirstOrDefaultAsync(m => m.Name == EntityConstants.Tasks);

            var eventLogs = await _context.EventLog.Where(c => c.RecordID == id && c.EntityID == EntityForTasks.ID).ToListAsync();

			var comments = await _context.Comments.Where(c => c.RecordID == id && c.EntityID == EntityForTasks.ID).ToListAsync();


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
            ViewData["ParentTaskId"] = new SelectList(_context.Set<Tasks>(), "ID", "ID", parentTaskId);

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
                         join role in _context.Roles on  userrole.RoleId equals role.Id
                         join user in _context.Users on userrole.UserId equals user.Id
                         where role.Name == ROLES.ADMIN
                         select user;

            ViewData["Admin"] = new SelectList(admins, "Id", "UserName");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromQuery] int? parentTaskId,[Bind("ID,Name,Description,TaskTypeID,ParentID,TaskStatusID,TaskPriorityID,PrimaryAssignedToPersonID,DateAssigned,DateDue,DateStarted,DateCompleted,SubmittedByPersonID,EffectiveDate,TaskData,ReceivedDate,CreateDate,CreateBy,ModifiedDate,ModifiedBy")] Tasks tasks)
        {
            var userName = this.User.FindFirst(ClaimTypes.Name).Value;
            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var GetTaskType = _context.TaskTypes.Where(row => row.ID == tasks.TaskTypeID).FirstOrDefault();
            tasks.CreateBy = userName;
            tasks.CreateDate = DateTime.UtcNow;
            tasks.DateDue = DateTime.Now.AddHours((double)GetTaskType.DefaultDueDateHours);
            tasks.SubmittedByPersonID = UserID;
            tasks.ParentID = parentTaskId;
            tasks.DateAssigned = DateTime.UtcNow;
            tasks.ReceivedDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            var admins = from userrole in _context.UserRoles
                         join role in _context.Roles on userrole.RoleId equals role.Id
                         join user in _context.Users on userrole.UserId equals user.Id
                         where role.Name == ROLES.ADMIN
                         select user;


            List<SelectListItem> numbers = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Critical" },
                new SelectListItem { Value = "2", Text = "High" },
                new SelectListItem { Value = "3", Text = "Normal", Selected = true  },
                new SelectListItem { Value = "4", Text = "Low" },
            };


            ViewData["ParentTaskId"] = new SelectList(_context.Set<Tasks>(), "ID", "ID",tasks.ParentID);
            ViewData["Priority"] = new SelectList(numbers, tasks.TaskPriorityID); 
            ViewData["Admin"] = new SelectList(admins, "Id", "UserName",tasks.PrimaryAssignedToPersonID);
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
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
            {
                return NotFound();
            }

			var changes = new List<string>();

			// Detach the existing entity
			_context.Entry(existingTask).State = EntityState.Detached;

            // Update properties as needed
            if (existingTask.PrimaryAssignedToPersonID != tasks.PrimaryAssignedToPersonID)
            {
                tasks.DateAssigned = DateTime.UtcNow;
                changes.Add("assignment");
            }

            if (existingTask.TaskTypeID != tasks.TaskTypeID)
            {
                var GetTaskType = _context.TaskTypes.FirstOrDefault(row => row.ID == tasks.TaskTypeID);
                changes.Add("task type");

				if (GetTaskType != null)
                {
                    tasks.DateDue = DateTime.Now.AddHours((double)GetTaskType.DefaultDueDateHours);
                }
            }

			if (existingTask.TaskStatusID != tasks.TaskStatusID)
			{
                changes.Add("status");
			}

			if (existingTask.Name != tasks.Name)
			{
				changes.Add("Name");
			}

			if (existingTask.Description != tasks.Description)
			{
				changes.Add("Decription");
			}

			if (existingTask.TaskPriorityID != tasks.TaskPriorityID)
			{
				changes.Add("Priority");
			}

			if (existingTask.TaskData != tasks.TaskData)
			{
				changes.Add("Task Details");
			}



			var NewstatusID = _context.TaskStatusTypes.FirstOrDefault(row => row.Status == "New")?.Id;

            if (existingTask.TaskStatusID == NewstatusID && existingTask.TaskStatusID != tasks.TaskStatusID)
            {
                tasks.DateStarted = DateTime.UtcNow;
            }

            var ClosedstatusID = _context.TaskStatusTypes.FirstOrDefault(row => row.Status == "Closed")?.Id;

            if (tasks.TaskStatusID == ClosedstatusID)
            {
                tasks.DateCompleted = DateTime.UtcNow;
            }
			string description = changes.Count > 0 ? $"{string.Join(" and ", changes)} changed" : "No significant changes";

			string outletModelJson = JsonConvert.SerializeObject(tasks, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
			var userName = this.User.FindFirst(ClaimTypes.Name)?.Value;
            tasks.ModifiedBy = userName;
            tasks.ModifiedDate = DateTime.UtcNow;

			var EntityForTasks = _context.Entity.FirstOrDefault(m => m.Name == EntityConstants.Tasks).ID;
			var eventModel = await _context.Event.Where(row => row.Name == EventConstants.EditEmployees).FirstOrDefaultAsync();
			var eventLogRecord = new EventLog
			{
                EventID = eventModel.ID,
				EntityID = EntityForTasks,
				RecordID = tasks.ID,
				Source = "Admin",
				Description = description,
				Data = outletModelJson,
				CreateDate = DateTime.Now,
				CreateBy = userName
			};

			if (id != tasks.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Attach the updated entity and mark it as modified
                    _context.Tasks.Attach(tasks);
                    _context.Entry(tasks).State = EntityState.Modified;
                    _context.EventLog.Add(eventLogRecord);

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
