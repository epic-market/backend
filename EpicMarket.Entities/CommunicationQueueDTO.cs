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
        public string ContactMethod { get; set; }

        public string MessageData { get; set; }

        public string Subject { get; set; }

        public string MessageText { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public string NotificationRecipient { get; set; }

        public long? EventLogID { get; set; }
        public DateTime CreateDate {  get; set; }
        public string CreateBy { get; set; }
    }
}
