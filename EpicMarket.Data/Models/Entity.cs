using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Entity: BaseModel
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } //order, products , Outlet , Staff

        [MaxLength(255)]
        public string Description { get; set; }

        public virtual ICollection<EventLog> EventLogs { get; set; } 
        public virtual ICollection<Tasks> Taskss { get; set; }
        public virtual ICollection<AttachmentLink> AttachmentLinks { get; set; }

		public virtual ICollection<Comment> Comments { get; set; }
	}
}
