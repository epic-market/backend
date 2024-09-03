using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class NotificationService : INotificationService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private static readonly object _lock = new object();
        public NotificationService(
                                ApplicationDbContext context,
                                IMapper mapper,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }


        public async Task ReadNotification(int NotifificationId)
        {
           var notification =  _context.Notifications.Find(NotifificationId);
            if (notification != null) { 
               notification.IsRead = true;
               _context.Notifications.Update(notification);
                await unitOfWork.Complete();
            }
        }



        public async Task<int> AddNotification(NotificationDto notification, int userID)
        {
            var newNotification = new Notification()
            {
                DateCreated = DateTime.Now,
                IsRead = false,
                Message = notification.Message,
                UserId = userID,
                Link = notification.Link,
            };
            await _context.Notifications.AddAsync(newNotification);
            await unitOfWork.Complete();

            return newNotification.Id;
        }

        public async Task<List<NotificationDto>> GetAllUnReadNoticationForSpecificUser(int UserID)
        {

          return await  _context.Notifications.Where(c => c.IsRead == false && c.UserId == UserID).Select(c => new NotificationDto()
            {
                Id = c.Id,
                Message = c.Message,
                Link = c.Link,
                CreateDate = c.DateCreated
            }).ToListAsync();
        }
    }
}
