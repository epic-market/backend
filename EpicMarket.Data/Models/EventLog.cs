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
    public class EventLog : BaseModel
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public int EventID { get; set; } // updated, Deleted , Created


        [Required]
        public int EntityID { get; set; } // Order, Product , Business

        [Required]
        public int? RecordID { get; set; }

        [MaxLength(255)]
        public string Source { get; set; } //which api call has been called this event

        [MaxLength(2000)]
        public string Description { get; set; } //type event desciption

        public string Data { get; set; } // its the data 

        public virtual Event Event { get; set; }

        public virtual Entity Entity { get; set; }
    }
}
