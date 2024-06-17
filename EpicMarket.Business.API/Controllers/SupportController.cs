using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EpicMarket.Business.API.Controllers
{
    public class SupportController : BaseApiController
    {

        private readonly ILogger<SupportController> logger;
        private readonly ITasksService tasksService;

        public SupportController(ILogger<SupportController> logger, ITasksService tasksService, ApplicationDbContext dbContext) : base(dbContext)
        {
            this.logger = logger;
            this.tasksService = tasksService;
        }

        [HttpPost("AddEditTask")]
        public  ActionResult<OperationResult<int>> AddEditTask(TasksDTO tasksDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddEditTask()-> params {0}", JsonConvert.SerializeObject(new { Params = tasksDTO }));

            var results = tasksService.SaveTask(tasksDTO);
            this.logger.LogInformation("Support Controller -> AddEditTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }
        [HttpPost("AddTaskComment")]
        public ActionResult<OperationResult<int>> AddTaskComment(CommentDTO commentDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddTaskComment()-> params {0}", JsonConvert.SerializeObject(new { Params = commentDTO }));

            var results = tasksService.SaveComments(commentDTO);
            this.logger.LogInformation("Support Controller -> AddTaskComment()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }

        [HttpGet("GetAllComments")]
        public async Task<ActionResult<OperationResult<GetDataResult<List<CommentDTO>>>>> GetAllComments([FromQuery] int taskId)
        {
            var response = new OperationResult<GetDataResult<List<CommentDTO>>>();

            this.logger.LogInformation("Support Controller -> GetAllComments()-> params {0}", JsonConvert.SerializeObject(new { Params = taskId }));

            var results = await tasksService.GetAllComments(taskId);

            this.logger.LogInformation("Support Controller -> GetAllComments()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }
        [HttpGet("GettaskDetails")]
        public async Task<ActionResult<OperationResult<TasksDTO>>> GettaskDetails(int taskId)
        {
            var response = new OperationResult<TasksDTO>();

            this.logger.LogInformation("Support Controller -> GettaskDetails()-> params {0}", JsonConvert.SerializeObject(new { Params = taskId }));

            var results = await tasksService.GettaskDetails(taskId);

            this.logger.LogInformation("Support Controller -> GettaskDetails()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }
    }
}
