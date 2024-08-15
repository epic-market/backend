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
    public class CommunicationQueueService : ICommunicationQueueService
    {
        private readonly ApplicationDbContext _context;
		private readonly IUnitOfWork unitOfWork;

		public CommunicationQueueService(ApplicationDbContext _context,IUnitOfWork unitOfWork)
        {
            this._context = _context;
			this.unitOfWork = unitOfWork;
		}
        public async Task InsertCommunicationQueue(CommunicationQueueDTO communicationQueueDTO)
        {

            var contactMethod = await _context.ContactMethod.Where(row => row.Name==communicationQueueDTO.ContactMethod.Trim()).FirstOrDefaultAsync();
            var communicationQueueModel = new CommunicationQueue();
            communicationQueueModel.MessageData = communicationQueueDTO.MessageData;
            communicationQueueModel.Subject = communicationQueueDTO.Subject;
            communicationQueueModel.MessageText = communicationQueueDTO.MessageText;
            communicationQueueModel.ScheduledDate = communicationQueueDTO.ScheduledDate;
            communicationQueueModel.NotificationRecipient = communicationQueueDTO.NotificationRecipient;
            communicationQueueModel.ContactMethodID = contactMethod.ID;
            communicationQueueModel.CreateBy = communicationQueueDTO.CreateBy;
            communicationQueueModel.CreateDate = DateTime.Now;
            await _context.CommunicationQueue.AddAsync(communicationQueueModel);
			await unitOfWork.Complete();
        }
    }
}
