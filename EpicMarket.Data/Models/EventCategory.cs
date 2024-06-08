using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class EventCategory //this table is to know which application is assced like bussinessowner, admin , customer app
    {
        [Key]
        public int ID { get; set; }

        [Required]
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int Sequence { get; set; }

        public virtual ICollection<Event>? Events { get; set; }

    }
}
