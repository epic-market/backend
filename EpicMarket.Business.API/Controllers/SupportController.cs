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

namespace EpicMarket.Business.API.Controllers
{
    public class SupportController : BaseApiController
    {

        private readonly ILogger<SupportController> logger;
        private readonly ITasksService tasksService;
        private readonly IFileService fileStoreService;
        private readonly IApplicationConfigurationService applicationConfigurationService;
        private readonly IAttachmentService attachmentService;

        public SupportController(
                                    ILogger<SupportController> logger,
                                    ITasksService tasksService,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor,
                                    IFileService fileStoreService,
                                    IApplicationConfigurationService applicationConfigurationService,
                                    IAttachmentService attachmentService
                                ) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.tasksService = tasksService;
            this.fileStoreService = fileStoreService;
            this.applicationConfigurationService = applicationConfigurationService;
            this.attachmentService = attachmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OperationResult<GetDataResult<List<TasksListDTO>>>>> GetSupportByPersonId()
        {
            var response = new OperationResult<GetDataResult<List<TasksListDTO>>>();

            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> params {0}", JsonConvert.SerializeObject(new { Params = UserID }));

            var results = await tasksService.GetSupportByPersonId(UserID);

            this.logger.LogInformation("Support Controller -> GetSupportByPersonId()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> AddTask([FromForm] TasksDTO tasksDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> AddTask()-> params {0}", JsonConvert.SerializeObject(new { Params = tasksDTO }));
            var results = await tasksService.SaveTask(tasksDTO,this.AdminPersonID,this.LoggedInUserName);
            if (tasksDTO.UploadFiles.Length > 0)
            {
                foreach (var proof in tasksDTO.UploadFiles)
                {
                    var filinsertOutput = await this.SaveFileGlobalAsync(proof, FilePathConstants.TASKPATH, this.fileStoreService, this.applicationConfigurationService, ((int)results));
                    var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                    {

                        Name = AttachmentTypeConstants.TASK,
                        Comment = null,
                        DocumentType = DocumentTypeConstants.FILE,
                        DocumentFileType = proof.ContentType,
                        DocumentFolderPath = filinsertOutput.FullPathLocation,
                        DocumentFile = filinsertOutput.FileName,
                    });
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.TASK,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Tasks,
                        RecordID = ((int)results)
                    });
                }
            }
            this.logger.LogInformation("Support Controller -> AddTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }

        [HttpGet("Comments")]
        [Authorize]
        public async Task<ActionResult<OperationResult<GetDataResult<List<CommentDTO>>>>> GetAllComments([FromQuery] int taskId)
        {
            var response = new OperationResult<GetDataResult<List<CommentDTO>>>();

            this.logger.LogInformation("Support Controller -> GetAllComments()-> params {0}", JsonConvert.SerializeObject(new { Params = taskId }));

            var results = await tasksService.GetAllComments(taskId);

            this.logger.LogInformation("Support Controller -> GetAllComments()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data = results;

            return Ok(response);
        }

        [HttpPost("Comments")]
        [Authorize]
        public ActionResult<OperationResult<int>> AddTaskComment(CommentDTO commentDTO)
        {
            var response = new OperationResult<int>();
            this.logger.LogInformation("Support Controller -> AddTaskComment()-> params {0}", JsonConvert.SerializeObject(new { Params = commentDTO }));
             response.Data = tasksService.SaveComments(commentDTO,this.LoggedInUserName);
            this.logger.LogInformation("Support Controller -> AddTaskComment()-> return {0}", JsonConvert.SerializeObject(new { Results = response.Data }));
            return Ok(response);
        }


        //This related to non-login and query screen of advertise URL
        [HttpPost("AddSupportTask")]
        [AllowAnonymous]
        public ActionResult<OperationResult<long>> AddSupportTask(SupportDTO supportDTO)
        {
            var response = new OperationResult<long>();

            this.logger.LogInformation("Support Controller -> AddSupportTask()-> params {0}", JsonConvert.SerializeObject(new { Params = supportDTO }));

            var results = tasksService.AddSupportTask(supportDTO,this.AdminPersonID);
            this.logger.LogInformation("Support Controller -> AddSupportTask()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));
            return Ok(response);
        }

        [HttpPost("attachment")]
        [Authorize]
        public async Task<ActionResult<OperationResult<int>>> Attachment([FromForm] AttachmentTASKDTO tasksDTO)
        {
            var response = new OperationResult<int>();

            this.logger.LogInformation("Support Controller -> Attachment()-> params {0}", JsonConvert.SerializeObject(new { Params = tasksDTO }));
            if (tasksDTO.UploadFiles.Length > 0 && tasksDTO.TaskID>0)
            {
                foreach (var proof in tasksDTO.UploadFiles)
                {
                    var filinsertOutput = await this.SaveFileGlobalAsync(proof, FilePathConstants.TASKPATH, this.fileStoreService, this.applicationConfigurationService, tasksDTO.TaskID);
                    var attachmentId = await this.attachmentService.InsertOrUpdateAttachment(new AttachmentDTO
                    {

                        Name = AttachmentTypeConstants.TASK,
                        Comment = null,
                        DocumentType = DocumentTypeConstants.FILE,
                        DocumentFileType = proof.ContentType,
                        DocumentFolderPath = filinsertOutput.FullPathLocation,
                        DocumentFile = filinsertOutput.FileName,
                    });
                    await this.attachmentService.InsertAttachmentLink(new AttachmentLinkDTO()
                    {
                        AttachmentTypeName = AttachmentTypeConstants.TASK,
                        AttachmentID = attachmentId,
                        Entity = EntityConstants.Tasks,
                        RecordID = tasksDTO.TaskID
                    });
                }
            }
            this.logger.LogInformation("Support Controller -> Attachment()-> return {0}");
            return Ok(response);
        }
    }
}
