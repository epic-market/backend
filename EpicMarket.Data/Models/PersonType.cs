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
    public class PersonType
    {
        [Key]
        public int ID { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SupportTicket> SupportTickets { get; set; }
        public virtual ICollection<SupportQueries> SupportQuerys { get; set; }
    }
}
