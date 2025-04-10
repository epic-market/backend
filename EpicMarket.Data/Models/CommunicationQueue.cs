using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Data.Common;

namespace EpicMarket.Data.Models
{
    public class CommunicationQueue : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ContactMethodID { get; set; } //which type phone , email

        public string MessageData { get; set; } //body

        [StringLength(255)]
        public string Subject { get; set; }

        public string MessageText { get; set; } //phone 

        public int RetryCount { get; set; } = 0; // Tracking retries

        public DateTime? ScheduledDate { get; set; } //optional

        public string NotificationRecipient { get; set; } //to email

        public int? CommunicationStatusId { get; set; } // Reference to status
        
        public string TemplateName { get; set; } // Name of the template used
        
        public DateTime? SentDate { get; set; } // When the communication was successfully sent
        
        public string ErrorMessage { get; set; } // Store any error messages

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? SysStartTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? SysEndTime { get; set; }

        public ContactMethod ContactMethod { get; set; }
        
        [ForeignKey("CommunicationStatusId")]
        public virtual CommunicationStatus CommunicationStatus { get; set; }
    }
}
