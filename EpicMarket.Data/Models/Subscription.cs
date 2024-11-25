using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OutletId { get; set; }
        public DateTime SubscribedDate { get; set; }
        public DateTime? UnsubscribedDate { get; set; }
        public int StatusID{ get; set; }

        // Navigation Properties
        public AppUser Customer { get; set; }
        public Outlet Outlet { get; set; }

        public SusbcriptionStatus Status { get; set; }

    }


}
