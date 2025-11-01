using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using EpicMarket.Entities.Constants;

namespace EpicMarket.Business.API.Controllers
{
    /// <summary>
    /// Handles support task management including ticket creation, retrieval, and commenting.
    /// Provides secure endpoints for both authenticated users and public support submissions.
    /// </summary>
    [Route("api/support")]
    public class SupportController : BaseApiController
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<SupportController> logger;
        private readonly ITasksService tasksService;
        private readonly IFileService fileStoreService;
        private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly IAttachmentService attachmentService;

        public SupportController(  ApplicationDbContext context,
                                    ILogger<SupportController> logger,
                                    ITasksService tasksService,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IFileService fileStoreService,
                                    IApplicationConfigurationService applicationConfigurationService,
                                    IAttachmentService attachmentService
                                ) : base(dbContext, httpContextAccessor)
        {
            this.context = context;
            this.logger = logger;
            this.tasksService = tasksService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
            this.attachmentService = attachmentService;
        }

        /// <summary>
        /// Retrieves support tasks for the authenticated user with optional filters.
        /// Route: GET api/support
        /// Auth: Authorize
        /// </summary>
        /// <param name="tasksListParams">Filtering and paging options for the task list.</param>
        /// <returns>Paged list of support tasks.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OperationResult<GetDataResult<List<TasksListDTO>>>>> GetSupportByPersonId([FromQuery] TasksListParams tasksListParams)
        {
            var response = new OperationResult<GetDataResult<List<TasksListDTO>>>();

            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> params {0}", JsonConvert.SerializeObject(new { Params = UserID }));

            var results = await tasksService.GetSupportByPersonId(UserID,tasksListParams);

            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Retrieves detailed information for a specific support task.
        /// Route: GET api/support/{taskId}
        /// Auth: Authorize
        /// </summary>
        /// <param name="taskId">Identifier of the task to retrieve.</param>
        /// <returns>Detailed task information including history.</returns>
        [HttpGet("{taskId}")]
        [Authorize]
        public async Task<ActionResult<OperationResult<TaskDeatilDTO>>> GettaskDetails(int taskId)
        {
            var response = new OperationResult<TaskDeatilDTO>();

            this.logger.LogInformation("Support Controller -> GettaskDetails()-> params {0}", JsonConvert.SerializeObject(new { Params = taskId }));

            var results = await tasksService.GettaskDetails(taskId);

            this.logger.LogInformation("Support Controller -> GettaskDetails()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Creates a new support task and optionally links uploaded attachments.
        /// Route: POST api/support
        /// Auth: Authorize
        /// </summary>
        /// <param name="tasksDTO">Details of the task to create.</param>
        /// <returns>Identifier of the created task.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> AddTask([FromBody] TasksDTO tasksDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddTask()-> params {0}", JsonConvert.SerializeObject(new { Params = tasksDTO }));
            var GetTaskType = context.TaskTypes.Where(row => row.ID == tasksDTO.TaskTypeID).FirstOrDefault();
            var taskParams = new TasksParams()
            {
                Name = tasksDTO.Name,
                Description = tasksDTO.Description,
                TaskData = null,
                TaskEntity = null,
                TaskPriorityID = 1,
                TaskType = GetTaskType.Name
            };
            var results = await tasksService.SaveTask(taskParams, this.AdminPersonID,this.LoggedInUserName);
            if (tasksDTO.UploadFiles?.Length > 0)
            {
                foreach (var proof in tasksDTO.UploadFiles)
                {
                    var attachmentId = await this.attachmentService.GetAttachmentId(proof);
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.TASK,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Tasks,
                        RecordID = ((int)results)
                    }, this.BusinessId);
                }
            }
            this.logger.LogInformation("Support Controller -> AddTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all comments associated with support tasks for the authenticated user.
        /// Route: GET api/support/Comments
        /// Auth: Authorize
        /// </summary>
        /// <param name="commentDTO">Filtering and paging options for comments.</param>
        /// <returns>Paged list of support task comments.</returns>
        [HttpGet("Comments")]
        [Authorize]
        public async Task<ActionResult<OperationResult<GetDataResult<List<CommentListDTO>>>>> GetAllComments([FromQuery] CommentListParams commentDTO)
        {
            var response = new OperationResult<GetDataResult<List<CommentListDTO>>>();

            this.logger.LogInformation("Support Controller -> GetAllComments()-> params {0}", JsonConvert.SerializeObject(new { Params = commentDTO }));

            var results = await tasksService.GetAllComments(commentDTO);

            this.logger.LogInformation("Support Controller -> GetAllComments()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        /// <summary>
        /// Adds a comment to a support task and associates uploaded attachments.
        /// Route: POST api/support/CommentsAttachments
        /// Auth: Authorize
        /// </summary>
        /// <param name="commentDTO">Comment details including attachments.</param>
        /// <returns>Identifier of the created comment.</returns>
        [HttpPost("CommentsAttachments")]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> AddTaskCommentAndAttachment([FromBody] CommentDTO commentDTO)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Support Controller -> AddTaskComment()-> params {0}", JsonConvert.SerializeObject(new { Params = commentDTO }));
             response.Data = await tasksService.SaveComments(commentDTO,this.LoggedInUserName);

            this.logger.LogInformation("Support Controller -> Attachment()-> params {0}", JsonConvert.SerializeObject(new { Params = commentDTO }));
            if (commentDTO.UploadFiles?.Length > 0 && commentDTO.TaskId.Value > 0)
            {
                foreach (var proof in commentDTO.UploadFiles)
                {
                    var attachmentId = await this.attachmentService.GetAttachmentId(proof);
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.TASK,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Tasks,
                        RecordID = commentDTO.TaskId.Value
                    }, this.BusinessId);
                }
            }
            this.logger.LogInformation("Support Controller -> AddTaskComment()-> return {0}", JsonConvert.SerializeObject(new { Results = response.Data }));
            return Ok(response);
        }


        /// <summary>
        /// Creates a support task from public channels without requiring authentication.
        /// Route: POST api/support/AddSupportTask
        /// Auth: AllowAnonymous
        /// </summary>
        /// <param name="supportDTO">Support request details submitted by a user.</param>
        /// <returns>Identifier of the newly created support task.</returns>
        [HttpPost("AddSupportTask")]
        [AllowAnonymous]
        public async Task<ActionResult<OperationResult<long>>> AddSupportTask(SupportDTO supportDTO)
        {
            var response = new OperationResult<long>();

            this.logger.LogInformation("Support Controller -> AddSupportTask()-> params {0}", JsonConvert.SerializeObject(new { Params = supportDTO }));

            var results =await tasksService.AddSupportTask(supportDTO,this.AdminPersonID);
            this.logger.LogInformation("Support Controller -> AddSupportTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }
    }
}
