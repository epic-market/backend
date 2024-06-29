using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{//this will be used for both events and tasks
    public class EventCategory : BaseModel //this table is to know which application is assced like bussinessowner, admin , customer app 
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Sequence { get; set; }

        public virtual ICollection<Event>? Events { get; set; }

        public virtual ICollection<TaskType>? TaskTypes { get; set; }
    }
}
