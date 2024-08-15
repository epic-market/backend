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

        public SupportController(ILogger<SupportController> logger, ITasksService tasksService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.tasksService = tasksService;
        }

        [HttpPost("AddEditTask")]
        public async Task<ActionResult<OperationResult<int>>> AddEditTask(TasksDTO tasksDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddEditTask()-> params {0}", JsonConvert.SerializeObject(new { Params = tasksDTO }));

            var results = await tasksService.SaveTask(tasksDTO);
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

        [HttpGet("GetSupportByPersonId")]
        public async Task<ActionResult<OperationResult<GetDataResult<List<TasksDTO>>>>> GetSupportByPersonId([FromQuery] int personId)
        {
            var response = new OperationResult<GetDataResult<List<TasksDTO>>>();

            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> params {0}", JsonConvert.SerializeObject(new { Params = personId }));

            var results = await tasksService.GetSupportByPersonId(personId);

            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        [HttpPost("AddSupportTask")]
        public ActionResult<OperationResult<long>> AddSupportTask(SupportDTO supportDTO)
        {
            var response = new OperationResult<long>();

            this.logger.LogInformation("Support Controller -> AddSupportTask()-> params {0}", JsonConvert.SerializeObject(new { Params = supportDTO }));

            var results = tasksService.AddSupportTask(supportDTO,this.AdminPersonID);
            this.logger.LogInformation("Support Controller -> AddSupportTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }
    }
}
