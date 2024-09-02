using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Entities;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace EpicMarket.Business.API.Controllers
{
    public class ActivityController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ActivityController> logger;
        public ActivityController(
                                    IMapper mapper,
                                    ILogger<ActivityController> logger,
                                    ApplicationDbContext dbContext,
                                    IHttpContextAccessor httpContextAccessor
                                    ) : base(dbContext, httpContextAccessor)
        {
            _mapper = mapper;
            this.logger = logger;

        }

        [HttpGet]
        public async Task<ActionResult<OperationResult<GetDataResult<List<ActivityResult>>>>> GetAllActivity([FromQuery] ActivityParams activityParams)
        {
            var response = new OperationResult<GetDataResult<List<ActivityResult>>>
            {
                Data = new GetDataResult<List<ActivityResult>>()
            };
            this.logger.LogInformation("Activity Controller -> GetAllActivity()-> params {0}", JsonConvert.SerializeObject(new { Params = activityParams }));

            var entity =  dbContext.Entity.Where(c => c.Name == activityParams.Entity).FirstOrDefault();
            var sortedActivity = dbContext.EventLog.Where(row => row.RecordID == activityParams.RecordId && row.EntityID == entity.ID);
            int totalCount = sortedActivity.Count();
            var pagedActivity = sortedActivity
                .Skip((activityParams.PageIndex - 1) * activityParams.pageSize) 
                .Take(activityParams.pageSize);
            var results = await pagedActivity.Select(row => new ActivityResult()
            {
                ID = row.ID,
                Description = row.Description,
                Data=row.Data,
                EventName = row.Event.Name, 
                CreatedDate = row.CreateDate,
                CreatedBy = row.CreateBy 
            }).ToListAsync();

            this.logger.LogInformation("Activity Controller -> GetAllActivity()-> return {0}", JsonConvert.SerializeObject(new { Results = results }));

            response.Data.items = results;
            response.Data.Count = totalCount;

            return Ok(response);
        }
    }
}