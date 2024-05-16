using EpicMarket.Business.API.Helpers;
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
				var usernameid =  int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
				var business = 	dbContext.Businesses.Where(c => c.PersonID == usernameid).FirstOrDefault();
				return business == null? throw new ("No Business is found"):business.ID;	
			}
		}


	}
}
