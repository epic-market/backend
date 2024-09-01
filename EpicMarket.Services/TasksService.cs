using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
        public async Task<long> SaveTask(TasksDTO tasksDTO, int AdminPersonID, string LoggedInUserName)
        {
            var taskStatusTypes = _context.TaskStatusTypes.Where(row => row.Status == TaskStatusTypesConstants.NEW).FirstOrDefault();
            var GetTaskType =  _context.TaskTypes.Where(row => row.ID == tasksDTO.TaskTypeID).FirstOrDefault();
            var taskToSave = new Tasks
            {
                Name = tasksDTO.Name,
                Description = tasksDTO.Description,
                TaskTypeID = tasksDTO.TaskTypeID,
                TaskStatusID = taskStatusTypes.Id,
                TaskPriorityID = 1,//TODO hardcoded
                PrimaryAssignedToPersonID = AdminPersonID,
                DateAssigned = DateTime.Now,
                DateDue = DateTime.Now.AddHours((double)GetTaskType.DefaultDueDateHours),
                DateStarted = null,
                DateCompleted = null,
                SubmittedByPersonID = tasksDTO.SubmittedByPersonID,
                TaskData = tasksDTO.TaskData,
                ReceivedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreateBy = LoggedInUserName
            };
            await _context.Tasks.AddAsync(taskToSave);
			await unitOfWork.Complete();
			return (long)taskToSave.ID;
     
        }

        public int SaveComments(CommentDTO commentDTO)
        {
            var EntityForTasks =  _context.Entity.FirstOrDefault(m => m.Name == EntityConstants.Tasks);
            Comment commentSave;
            commentSave = new Comment
            {
                CommentText = commentDTO.CommentText,
                Status = commentDTO.Status,
                RecordID = commentDTO.RecordID,
                CreateDate = DateTime.Now,
                CreateBy = commentDTO.CreateBy,
                EntityID= EntityForTasks.ID,
            };
            _context.Comments.Add(commentSave);
			unitOfWork.Complete();
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
        public async Task<TaskDeatilDTO> GettaskDetails(int taskId) //get attachments if any
        {
            var attachmentTypeID= await _context.AttachmentTypes.FirstOrDefaultAsync(c => c.Name == AttachmentTypeConstants.TASK);

            var uploadFilePaths = from attachment in _context.Attachments
                            join link in _context.AttachmentLinks on attachment.ID equals link.AttachmentID
                            join entity in _context.Entity on link.EntityID equals entity.ID
                            where entity.Name == EntityConstants.Branch && link.RecordID == taskId && link.AttachmentTypeID == attachmentTypeID.ID
                            orderby attachment.CreateDate descending
                            select new
                            {
                                ImagePath = $"{attachment.DocumentFolderPath}{attachment.DocumentFile}"
                            };

            return await _context.Tasks.Where(c => c.ID == taskId).Select(c => new TaskDeatilDTO
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                TaskTypeID = c.TaskTypeID,
                TaskStatus = c.TaskStatusType.Status,
                TaskData = c.TaskData,
                CreateDate = c.CreateDate,
                UploadFiles = uploadFilePaths.Select(a => a.ImagePath).ToList(),
                CommentsList = _context.Comments
                          .Where(comment => comment.RecordID == c.ID)
                          .Select(comment => new CommentListDTO
                          {
                              ID = comment.ID,
                              CommentText = comment.CommentText,
                              Status = comment.Status,
                              CreateBy = comment.CreateBy,
                              CreateDate = comment.CreateDate
                          }).ToList()
            }).FirstOrDefaultAsync();
        }
        public async Task<GetDataResult<List<TasksListDTO>>> GetSupportByPersonId(int personId)
        {
            var tasksDTOs = await _context.Tasks
                                        .Where(c => c.SubmittedByPersonID == personId)
                                        .Select(c => new TasksListDTO
                                        {
                                            ID = c.ID,
                                            Name = c.Name,
                                            TaskTypeID = c.TaskTypeID,
                                            TaskStatus = c.TaskStatusType.Status,
                                            TaskData = c.TaskData,
                                            CreateDate = c.CreateDate,
                                        })
                                        .ToListAsync();

            return new GetDataResult<List<TasksListDTO>>
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
                TaskData = supportDTO.Comment,
            }, AdminPersonID, supportDTO.Email);

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