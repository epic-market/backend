using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class EventCategory
    {
        [Key]
        public int ID { get; set; }

        [Required]
        
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int Sequence { get; set; }
        public string EventCategoryIcon { get; set; }

        public string AlertIcons { get; set; }

        public bool IsShownInAlerts { get; set; }
    }
}
