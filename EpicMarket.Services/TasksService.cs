using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
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
        public async Task<long> SaveTask(TasksDTO tasksDTO)
        {
            var currentTask = _context.Taskss.Where(row => row.ID == tasksDTO.ID).FirstOrDefault();
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
                _context.Taskss.Add(taskToSave);
                await _context.SaveChangesAsync();
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
                _context.Taskss.Update(taskToSave);
                await _context.SaveChangesAsync();

            }
            return (long)currentTask.ID;
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
            await _context.SaveChangesAsync();
            return commentSave.ID;
        }
    }
}