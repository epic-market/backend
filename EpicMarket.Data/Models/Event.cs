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
    public class Event: BaseModel //orders ,products , employee,braches, business
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int EventCategoryID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public int? PriorityID { get; set; }

        public virtual EventCategory EventCategorys { get; set; }

        public virtual ICollection<EventLog>? EventLogs { get; set; }

    }
}
