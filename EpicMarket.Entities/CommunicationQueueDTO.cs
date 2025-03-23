using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class CommunicationQueueDTO
    {
        public int ID { get; set; }
        
        public string ContactMethod { get; set; }

        public string MessageData { get; set; }

        public string Subject { get; set; }

        public string MessageText { get; set; }
        
        public int RetryCount { get; set; } = 0;

        public DateTime? ScheduledDate { get; set; }

        public string NotificationRecipient { get; set; }
        
        public string ToEmail { get; set; } // Explicit property for email recipient
        
        public string Body { get; set; } // Explicit body content after template rendering
        
        public int? CommunicationStatusId { get; set; }
        
        public string CommunicationStatusName { get; set; }
        
        public string TemplateName { get; set; }
        
        public DateTime? SentDate { get; set; }
        
        public string ErrorMessage { get; set; }

        public long? EventLogID { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string CreatedBy { get; set; }
    }
}
