using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{

    public class TasksService : ITasksService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
		private readonly IUnitOfWork unitOfWork;

		public TasksService(ApplicationDbContext context, IMapper mapper,IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
			this.unitOfWork = unitOfWork;
		}
        public async Task<long> SaveTask(TasksDTO tasksDTO)
        {
            var currentTask = _context.Tasks.Where(row => row.ID == tasksDTO.ID).FirstOrDefault();
            var newTaskStatus = _context.TaskStatusTypes.Where(row => row.Status == "New").FirstOrDefault();
            var updateTaskStatus = _context.TaskStatusTypes.Where(row => row.Status == tasksDTO.TaskStatus).FirstOrDefault();
            var GetTaskType =  _context.TaskTypes.Where(row => row.ID == tasksDTO.TaskTypeID).FirstOrDefault();
            var taskToSave = new Tasks
            {
                Name = tasksDTO.Name,
                Description = tasksDTO.Description,
                TaskTypeID = tasksDTO.TaskTypeID,
                ParentID = tasksDTO.ParentID,
                TaskStatusID = newTaskStatus.Id,
                TaskPriorityID = tasksDTO.TaskPriorityID,
                PrimaryAssignedToPersonID = tasksDTO.PrimaryAssignedToPersonID,
                DateAssigned = DateTime.Now,
                DateDue = DateTime.Now.AddHours((double)GetTaskType.DefaultDueDateHours),
                DateStarted = tasksDTO.DateStarted,
                DateCompleted = tasksDTO.DateCompleted,
                SubmittedByPersonID = tasksDTO.SubmittedByPersonID,
                TaskData = tasksDTO.TaskData,
                ReceivedDate = tasksDTO.ReceivedDate,
                CreateDate = DateTime.Now,
                CreateBy = tasksDTO.CreateBy
            };
            await _context.Tasks.AddAsync(taskToSave);
			await unitOfWork.Complete();
			return (long)taskToSave.ID;
           
     
        }

        public async Task<int> SaveComments(CommentDTO commentDTO)
        {
            Comment commentSave;
            commentSave = new Comment
            {
                CommentText = commentDTO.CommentText,
                Status = commentDTO.Status,
                RecordID = commentDTO.RecordID,
                CreateDate = DateTime.Now,
                CreateBy = commentDTO.CreateBy
            };
            _context.Comments.Add(commentSave);
			await unitOfWork.Complete();
			return commentSave.ID;
        }

        public async Task<GetDataResult<List<CommentDTO>>> GetAllComments(int taskId)
        {
            var comments = await _context.Comments
                                        .Where(c => c.RecordID == taskId)
                                        .Select(c => new CommentDTO
                                        {
                                            RecordID = c.RecordID,
                                            CommentText = c.CommentText,
                                            Status = c.Status,
                                            CreateBy = c.CreateBy,
                                            CreateDate = c.CreateDate
                                        })
                                        .ToListAsync();

            return new GetDataResult<List<CommentDTO>>
            {
                items = comments,
            };
        }
        public async Task<TasksDTO> GettaskDetails(int taskId)
        {
            return await _context.Tasks.Where(c => c.ID == taskId).Select(c => new TasksDTO
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                TaskTypeID = c.TaskTypeID,
                TaskStatus = c.TaskStatusType.Status,
                TaskPriorityID = c.TaskPriorityID,
                DateStarted = c.DateStarted,
                DateCompleted = c.DateCompleted,
                TaskData = c.TaskData,
                ReceivedDate = c.ReceivedDate,
                ModifiedDate = c.ModifiedDate,
                ModifiedBy = c.ModifiedBy,
                CreateDate = c.CreateDate,
                CreateBy = c.CreateBy
            }

            ).FirstOrDefaultAsync();
        }
        public async Task<GetDataResult<List<TasksDTO>>> GetSupportByPersonId(int personId)
        {
            var tasksDTOs = await _context.Tasks
                                        .Where(c => c.SubmittedByPersonID == personId)
                                        .Select(c => new TasksDTO
                                        {
                                            ID = c.ID,
                                            Name = c.Name,
                                            Description = c.Description,
                                            TaskTypeID = c.TaskTypeID,
                                            TaskStatus = c.TaskStatusType.Status,
                                            TaskPriorityID = c.TaskPriorityID,
                                            DateStarted = c.DateStarted,
                                            DateCompleted = c.DateCompleted,
                                            TaskData = c.TaskData,
                                            ReceivedDate = c.ReceivedDate,
                                            ModifiedDate = c.ModifiedDate,
                                            ModifiedBy = c.ModifiedBy,
                                            CreateDate = c.CreateDate,
                                            CreateBy = c.CreateBy
                                        })
                                        .ToListAsync();

            return new GetDataResult<List<TasksDTO>>
            {
                items = tasksDTOs,
            };
        }
        public async Task<long> AddSupportTask(SupportDTO supportDTO, int AdminPersonID)
        {
            var supportQuery = _context.SupportQuerys.Where(row => row.ID == supportDTO.QueryId).FirstOrDefault();
            var taskID = await this.SaveTask(new TasksDTO
            {
                Name= "Support Query",
                Description= supportQuery.Query,
                TaskTypeID=supportQuery.TaskTypeID,
                PrimaryAssignedToPersonID= AdminPersonID,
                TaskData = supportDTO.Comment,
                CreateBy = supportDTO.Email,
                ReceivedDate = DateTime.Now
            });

            SupportTicket supportTicket;
            supportTicket = new SupportTicket
            {
                Email = supportDTO.Email,
                Phonenumber = supportDTO.Phonenumber,
                TypeofPersonid = supportDTO.TypeofPersonid,
                Fullname = supportDTO.Fullname,
                Taskid= (int)taskID
            };
            _context.SupportTickets.Add(supportTicket);
			await unitOfWork.Complete();
			return (long)supportTicket.ID;
        }

    }
}