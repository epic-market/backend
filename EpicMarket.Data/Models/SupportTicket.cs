using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class SupportTicket
    {
        public int ID { get; set; }

        public int TicketTypeID { get; set; }

        public string Attachment { get; set; }

        public string  Description { get; set; }

        public int  PersonId { get; set; }

        public virtual AppUser Person { get; set; }

        public virtual SupportTicketType TicketType { get; set; }
    }
}
