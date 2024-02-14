using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class SupportTicketType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SupportTicket> SupportTickets { get; set; }
    }
}
