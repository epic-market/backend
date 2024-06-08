using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class CommunicationQueueService : ICommunicationQueueService
    {
        private readonly ApplicationDbContext _context;

        public CommunicationQueueService(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async void InsertCommunicationQueue(CommunicationQueueDTO communicationQueueDTO)
        {

            var contactMethod = _context.ContactMethod.Where(row => row.Name==communicationQueueDTO.ContactMethod.Trim());
            var communicationQueueModel = new CommunicationQueue();
            communicationQueueModel.MessageData = communicationQueueDTO.MessageData;
            communicationQueueModel.Subject = communicationQueueDTO.Subject;
            communicationQueueModel.MessageText = communicationQueueDTO.MessageText;
            communicationQueueModel.ScheduledDate = communicationQueueDTO.ScheduledDate;
            communicationQueueModel.NotificationRecipient = communicationQueueDTO.NotificationRecipient;
            communicationQueueModel.EventLogID = communicationQueueDTO.EventLogID;
            communicationQueueModel.ContactMethodID = contactMethod.FirstOrDefault().ID;
            communicationQueueModel.CreateBy = communicationQueueDTO.CreateBy;
            communicationQueueModel.CreateDate = DateTime.Now;
            _context.CommunicationQueue.Add(communicationQueueModel);
            await _context.SaveChangesAsync();
        }
    }
}
