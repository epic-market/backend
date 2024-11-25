using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class SusbcriptionStatus
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
