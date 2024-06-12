using EpicMarket.Business.API.Helpers;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace EpicMarket.Business.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
		protected readonly ApplicationDbContext dbContext;

		public BaseApiController(ApplicationDbContext dbContext )
        {
			this.dbContext = dbContext;
        }
        public int BusinessId
        {
            get {
                var usernameid = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (this.User.IsInRole(ROLES.BUSINESS_OWNER))
                {
                   return dbContext.Businesses.Where(c => c.PersonID == usernameid).Select(c => c.ID).FirstOrDefault(); ;
                }
                else if (this.User.IsInRole(ROLES.BUSINESS_EMPLOYEE))
                {
                    return dbContext.BusinessEmployeeMaps.Where(c => c.EmployeeID == usernameid).Select(c => c.BussinessID).FirstOrDefault();
                }
                else {
                    throw new("No Business is found");
                }
			}
		}


	}
}
