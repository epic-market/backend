using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using EpicMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EpicMarket.Business.API.Controllers
{

	/// <summary>
	/// Notification APIs for retrieving and updating user-specific alerts.
	/// Route prefix: api/notification
	/// Auth: Requires authenticated users.
	/// </summary>
	[Route("api/notification")]
	public class NotificationController : BaseApiController
    {
        private readonly ILogger<NotificationController> logger;
        private readonly INotificationService notificationService;

        public NotificationController(ILogger<NotificationController> logger , ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,INotificationService notificationService) : base(dbContext, httpContextAccessor)
        {
            this.logger = logger;
            this.notificationService = notificationService;
        }

		/// <summary>
		/// Retrieves unread notifications for the logged-in user.
		/// Route: GET api/notification
		/// Auth: Any authenticated user.
		/// </summary>
		/// <returns>List of unread notifications.</returns>
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetNotification()
		{
            var reponse = new OperationResult<List<NotificationResult>>();

            this.logger.LogInformation("Notification Controller -> GetStatusOptions()");

            var UserID = int.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var notifications = await this.notificationService.GetAllUnReadNoticationForSpecificUser(UserID);

            this.logger.LogInformation("Notification Controller-> GetStatusOptions()-> return {0}", JsonConvert.SerializeObject(new { ListofOptions = notifications }));

            reponse.Data = notifications;

            return Ok(reponse);
          
        }

		/// <summary>
		/// Marks the specified notification as read.
		/// Route: PUT api/notification/Read/{id}
		/// Auth: Any authenticated user.
		/// </summary>
		/// <param name="id">The notification identifier to mark as read.</param>
		/// <returns>Boolean response indicating success.</returns>
		[HttpPut("Read/{id}")]
		[Authorize]
		public async Task<IActionResult> ReadNotification(int id)
		{

            var reponse = new OperationResult<bool>();

            this.logger.LogInformation("Notification Controller-> ReadNotification()-> params {0}", JsonConvert.SerializeObject(new { param = id }));

            await this.notificationService.ReadNotification(id);


            reponse.Data = true;

            return Ok(reponse);

        }
    }
}
