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
    public class CommunicationQueue:BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ContactMethodID { get; set; }

        public string MessageData { get; set; }

        [StringLength(255)]
        public string Subject { get; set; }

        public string MessageText { get; set; }

        public int? Attempts { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public string NotificationRecipient { get; set; }

        public long? EventLogID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2")]
        public DateTime SysStartTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2")]
        public DateTime SysEndTime { get; set; }
        public ContactMethod ContactMethod { get; set; }
    }
}
