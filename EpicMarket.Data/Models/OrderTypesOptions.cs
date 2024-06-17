using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class OrderTypesOptions
    {
        public int Id { get; set; }

        public string Ordertype { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
