using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{

    public class TasksService : ITasksService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public TasksService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public long SaveTask(TasksDTO tasksDTO)
        {
            var currentTask = _context.Tasks.Where(row => row.ID == tasksDTO.ID).FirstOrDefault();
            var newTaskStatus = _context.TaskStatusTypes.Where(row => row.Status == "New").FirstOrDefault();
            var updateTaskStatus = _context.TaskStatusTypes.Where(row => row.Status == tasksDTO.TaskStatus).FirstOrDefault();
            Tasks taskToSave;
            if (currentTask != null)
            {
                taskToSave = new Tasks
                {
                    Name = tasksDTO.Name,
                    Description = tasksDTO.Description,
                    TaskTypeID = tasksDTO.TaskTypeID,
                    ParentID = tasksDTO.ParentID,
                    TaskStatusID = newTaskStatus.Id,
                    TaskPriorityID = tasksDTO.TaskPriorityID,
                    PrimaryAssignedToPersonID = tasksDTO.PrimaryAssignedToPersonID,
                    DateAssigned = DateTime.Now,
                    DateDue = tasksDTO.DateDue,
                    DateStarted = tasksDTO.DateStarted,
                    DateCompleted = tasksDTO.DateCompleted,
                    SubmittedByPersonID = tasksDTO.SubmittedByPersonID,
                    TaskData = tasksDTO.TaskData,
                    ReceivedDate = tasksDTO.ReceivedDate,
                    CreateDate = DateTime.Now,
                    CreateBy = tasksDTO.CreateBy
                };
                _context.Tasks.Add(taskToSave);
                _context.SaveChanges();
                currentTask.ID = taskToSave.ID;
            }
            else
            {
                taskToSave = new Tasks
                {
                    Name = tasksDTO.Name,
                    Description = tasksDTO.Description,
                    TaskTypeID = tasksDTO.TaskTypeID,
                    TaskStatusID = updateTaskStatus.Id,
                    TaskPriorityID = tasksDTO.TaskPriorityID,
                    DateStarted = tasksDTO.DateStarted,
                    DateCompleted = tasksDTO.DateCompleted,
                    TaskData = tasksDTO.TaskData,
                    ReceivedDate = tasksDTO.ReceivedDate,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = tasksDTO.ModifiedBy
                };
                _context.Tasks.Update(taskToSave);
                _context.SaveChanges();

            }
            return (long)currentTask.ID;
        }

        public int SaveComments(CommentDTO commentDTO)
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
            _context.SaveChanges();
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

    }
}