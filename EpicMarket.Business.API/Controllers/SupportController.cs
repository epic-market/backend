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
    /// <summary>
    /// Manages support tickets, tasks, and customer service operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SupportController : BaseApiController
    {

        private readonly ILogger<SupportController> logger;
        private readonly ITasksService tasksService;

        public SupportController(ILogger<SupportController> logger, ITasksService tasksService, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.tasksService = tasksService;
        }

        /// <summary>
        /// Creates a new task or updates an existing task
        /// </summary>
        /// <param name="tasksDTO">Task information including title, description, priority, and assignee</param>
        /// <returns>The ID of the created or updated task</returns>
        /// <response code="200">Task successfully created or updated</response>
        /// <response code="400">Invalid task data provided</response>
        /// <response code="401">User is not authenticated</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/support/AddEditTask
        ///     {
        ///        "taskId": 0,  // 0 for new task, existing ID for update
        ///        "title": "Customer complaint about delivery",
        ///        "description": "Product was damaged during delivery",
        ///        "priority": "High",
        ///        "category": "Complaint",
        ///        "assignedTo": 123,
        ///        "dueDate": "2024-01-15T10:00:00",
        ///        "status": "Open"
        ///     }
        /// </remarks>
        [HttpPost("AddEditTask")]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OperationResult<int>>> AddEditTask(TasksDTO tasksDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddEditTask()-> params {0}", JsonConvert.SerializeObject(new { Params = tasksDTO }));

            var results = await tasksService.SaveTask(tasksDTO);
            this.logger.LogInformation("Support Controller -> AddEditTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }
        /// <summary>
        /// Adds a comment to an existing task
        /// </summary>
        /// <param name="commentDTO">Comment information including task ID and comment text</param>
        /// <returns>The ID of the created comment</returns>
        /// <response code="200">Comment successfully added</response>
        /// <response code="400">Invalid comment data</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="404">Task not found</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/support/AddTaskComment
        ///     {
        ///        "taskId": 123,
        ///        "comment": "Customer has been contacted and issue resolved",
        ///        "isInternal": false,
        ///        "attachments": []
        ///     }
        /// 
        /// Comments can be internal (visible only to staff) or external (visible to customers).
        /// </remarks>
        [HttpPost("AddTaskComment")]
        [ProducesResponseType(typeof(OperationResult<int>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public ActionResult<OperationResult<int>> AddTaskComment(CommentDTO commentDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddTaskComment()-> params {0}", JsonConvert.SerializeObject(new { Params = commentDTO }));

            var results = tasksService.SaveComments(commentDTO);
            this.logger.LogInformation("Support Controller -> AddTaskComment()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all comments for a specific task
        /// </summary>
        /// <param name="taskId">The ID of the task to get comments for</param>
        /// <returns>List of comments with pagination information</returns>
        /// <response code="200">Returns list of comments</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="404">Task not found</response>
        /// <remarks>
        /// Returns comments in chronological order with:
        /// - Comment text
        /// - Author information
        /// - Timestamp
        /// - Internal/external flag
        /// - Attachments
        /// </remarks>
        [HttpGet("GetAllComments")]
        [ProducesResponseType(typeof(OperationResult<GetDataResult<List<CommentDTO>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<GetDataResult<List<CommentDTO>>>>> GetAllComments([FromQuery] int taskId)
        {
            var response = new OperationResult<GetDataResult<List<CommentDTO>>>();

            this.logger.LogInformation("Support Controller -> GetAllComments()-> params {0}", JsonConvert.SerializeObject(new { Params = taskId }));

            var results = await tasksService.GetAllComments(taskId);

            this.logger.LogInformation("Support Controller -> GetAllComments()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves detailed information for a specific task
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve</param>
        /// <returns>Complete task details</returns>
        /// <response code="200">Returns task details</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="404">Task not found</response>
        /// <remarks>
        /// Returns comprehensive task information including:
        /// - Task metadata (title, description, priority)
        /// - Assignment and status information
        /// - Complete comment history
        /// - Related tickets or tasks
        /// - Attachments and documents
        /// - Activity timeline
        /// </remarks>
        [HttpGet("GettaskDetails")]
        [ProducesResponseType(typeof(OperationResult<TasksDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationResult<TasksDTO>>> GettaskDetails(int taskId)
        {
            var response = new OperationResult<TasksDTO>();

            this.logger.LogInformation("Support Controller -> GettaskDetails()-> params {0}", JsonConvert.SerializeObject(new { Params = taskId }));

            var results = await tasksService.GettaskDetails(taskId);

            this.logger.LogInformation("Support Controller -> GettaskDetails()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves all support tasks associated with a specific person
        /// </summary>
        /// <param name="personId">The ID of the person to get tasks for</param>
        /// <returns>List of tasks with pagination</returns>
        /// <response code="200">Returns list of tasks</response>
        /// <response code="401">User is not authenticated</response>
        /// <remarks>
        /// Returns all tasks where the person is:
        /// - The creator/requester
        /// - The assignee
        /// - A participant in comments
        /// 
        /// Tasks are sorted by priority and date.
        /// </remarks>
        [HttpGet("GetSupportByPersonId")]
        [ProducesResponseType(typeof(OperationResult<GetDataResult<List<TasksDTO>>>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OperationResult<GetDataResult<List<TasksDTO>>>>> GetSupportByPersonId([FromQuery] int personId)
        {
            var response = new OperationResult<GetDataResult<List<TasksDTO>>>();

            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> params {0}", JsonConvert.SerializeObject(new { Params = personId }));

            var results = await tasksService.GetSupportByPersonId(personId);

            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Creates a new support ticket/task
        /// </summary>
        /// <param name="supportDTO">Support ticket information</param>
        /// <returns>The ID of the created support ticket</returns>
        /// <response code="200">Support ticket successfully created</response>
        /// <response code="400">Invalid support ticket data</response>
        /// <response code="401">User is not authenticated</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/support/AddSupportTask
        ///     {
        ///        "personId": 456,
        ///        "personType": "Customer",
        ///        "category": "Technical Issue",
        ///        "subject": "Cannot login to account",
        ///        "description": "Getting error message when trying to login",
        ///        "priority": "Medium",
        ///        "contactEmail": "customer@example.com",
        ///        "contactPhone": "+1234567890"
        ///     }
        /// 
        /// This creates a ticket that will be assigned to the appropriate support team.
        /// </remarks>
        [HttpPost("AddSupportTask")]
        [ProducesResponseType(typeof(OperationResult<long>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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
