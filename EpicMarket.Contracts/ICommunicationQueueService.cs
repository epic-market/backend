using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface    ICommunicationQueueService
    {
        public Task InsertCommunicationQueue(CommunicationQueueDTO communicationQueueDTO);
        
        public Task UpdateCommunicationQueueStatus(int id, int statusId);
        
        public Task UpdateCommunicationQueueStatusAndRetry(int id, int statusId, int retryCount, string errorMessage = null);
        
        public Task<int> GetCommunicationQueueRetryCount(int id);
        
        public Task<CommunicationQueueDTO> GetCommunicationQueueById(int id);
    }
}
