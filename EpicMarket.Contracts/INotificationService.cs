using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface INotificationService
    {
        Task ReadNotification(int NotifificationId);

        Task<List<NotificationResult>> GetAllUnReadNoticationForSpecificUser(int UserID);

        Task<int> AddNotification(NotificationDto notification, int userID);

    }
}
