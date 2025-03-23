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

        public CommunicationQueueService(ApplicationDbContext _context, IUnitOfWork unitOfWork)
        {
            this._context = _context;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CommunicationQueueDTO> GetCommunicationQueueById(int id)
        {
            var communicationQueue = await _context.CommunicationQueue
                .Include(cq => cq.CommunicationStatus)
                .Include(cq => cq.ContactMethod)
                .FirstOrDefaultAsync(cq => cq.ID == id);

            if (communicationQueue == null)
                return null;

            return new CommunicationQueueDTO
            {
                ID = communicationQueue.ID,
                ContactMethod = communicationQueue.ContactMethod?.Name,
                MessageData = communicationQueue.MessageData,
                Subject = communicationQueue.Subject,
                MessageText = communicationQueue.MessageText,
                RetryCount = communicationQueue.RetryCount,
                ScheduledDate = communicationQueue.ScheduledDate,
                NotificationRecipient = communicationQueue.NotificationRecipient,
                ToEmail = communicationQueue.NotificationRecipient, // Assuming NotificationRecipient contains the email
                Body = communicationQueue.MessageData, // Assuming MessageData contains the email body
                CommunicationStatusId = communicationQueue.CommunicationStatusId,
                CommunicationStatusName = communicationQueue.CommunicationStatus?.Name,
                TemplateName = communicationQueue.TemplateName,
                SentDate = communicationQueue.SentDate,
                ErrorMessage = communicationQueue.ErrorMessage,
                CreatedBy = communicationQueue.CreateBy,
                CreatedDate = communicationQueue.CreateDate
            };
        }

        public async Task<int> GetCommunicationQueueRetryCount(int id)
        {
            var communicationQueue = await _context.CommunicationQueue
                .Where(cq => cq.ID == id)
                .Select(cq => new { cq.RetryCount })
                .FirstOrDefaultAsync();

            if (communicationQueue == null)
                throw new KeyNotFoundException($"Communication queue with ID {id} not found");

            return communicationQueue.RetryCount;
        }

        public async Task InsertCommunicationQueue(CommunicationQueueDTO communicationQueueDTO)
        {
            var contactMethod = await _context.ContactMethod
                .Where(row => row.Name == communicationQueueDTO.ContactMethod)
                .FirstOrDefaultAsync();

            if (contactMethod == null)
                throw new InvalidOperationException($"Contact method '{communicationQueueDTO.ContactMethod}' not found");

            var communicationQueueModel = new CommunicationQueue
            {
                MessageData = communicationQueueDTO.MessageData ?? communicationQueueDTO.Body,
                Subject = communicationQueueDTO.Subject,
                MessageText = communicationQueueDTO.MessageText,
                ScheduledDate = communicationQueueDTO.ScheduledDate,
                NotificationRecipient = communicationQueueDTO.NotificationRecipient ?? communicationQueueDTO.ToEmail,
                ContactMethodID = contactMethod.ID,
                RetryCount = communicationQueueDTO.RetryCount,
                CommunicationStatusId = communicationQueueDTO.CommunicationStatusId ?? _context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Queued).ID,
                TemplateName = communicationQueueDTO.TemplateName,
                CreateBy = communicationQueueDTO.CreatedBy,
                CreateDate = DateTime.Now
            };

            await _context.CommunicationQueue.AddAsync(communicationQueueModel);
            await unitOfWork.Complete();
            
            // Update the DTO with the generated ID
            communicationQueueDTO.ID = communicationQueueModel.ID;
        }

        public async Task UpdateCommunicationQueueRetryCount(int id, int retryCount)
        {
            var communicationQueue = await _context.CommunicationQueue
                .FindAsync(id);

            if (communicationQueue == null)
                throw new KeyNotFoundException($"Communication queue with ID {id} not found");

            communicationQueue.RetryCount = retryCount;
            communicationQueue.ModifiedBy = "System";
            communicationQueue.ModifiedDate = DateTime.Now;

            _context.CommunicationQueue.Update(communicationQueue);
            await unitOfWork.Complete();
        }

        public async Task UpdateCommunicationQueueStatus(int id, int statusId)
        {
            var communicationQueue = await _context.CommunicationQueue
                .FindAsync(id);

            if (communicationQueue == null)
                throw new KeyNotFoundException($"Communication queue with ID {id} not found");

            communicationQueue.CommunicationStatusId = statusId;
            
            // If status is "Sent", update the SentDate
            if (statusId == _context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Sent).ID)
            {
                communicationQueue.SentDate = DateTime.Now;
            }
            
            communicationQueue.ModifiedBy = "System";
            communicationQueue.ModifiedDate = DateTime.Now;

            _context.CommunicationQueue.Update(communicationQueue);
            await unitOfWork.Complete();
        }
        
        // Additional helper method to update both status and retry count in one transaction
        public async Task UpdateCommunicationQueueStatusAndRetry(int id, int statusId, int retryCount, string errorMessage = null)
        {
            var communicationQueue = await _context.CommunicationQueue
                .FindAsync(id);

            if (communicationQueue == null)
                throw new KeyNotFoundException($"Communication queue with ID {id} not found");

            communicationQueue.CommunicationStatusId = statusId;
            communicationQueue.RetryCount = retryCount;
            
            if (errorMessage != null)
            {
                communicationQueue.ErrorMessage = errorMessage;
            }
            
            // If status is "Sent", update the SentDate
            if (statusId == _context.CommunicationStatus.FirstOrDefault(cs => cs.Name == CommunicationStatusConstants.Sent).ID)
            {
                communicationQueue.SentDate = DateTime.Now;
            }
            
            communicationQueue.ModifiedBy = "System";
            communicationQueue.ModifiedDate = DateTime.Now;

            _context.CommunicationQueue.Update(communicationQueue);
            await unitOfWork.Complete();
        }
    }
}
