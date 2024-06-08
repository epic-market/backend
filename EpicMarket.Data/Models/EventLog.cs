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
    public class EventLog:BaseModel
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public int EventID { get; set; }

        [MaxLength(88)]
        public string SessionID { get; set; }

        [MaxLength(255)]
        public string Source { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public string Data { get; set; }

        public DateTime? NotificationQueueDate { get; set; }
        public int? EventTypeID { get; set; }

        public virtual Event Event { get; set; }

        public virtual EventType EventType { get; set; }
    }
}
