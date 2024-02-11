using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int BusinessID { get; set; }
        public string OrderType { get; set; }
        public double TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public DateTime OrderAt { get; set; }
        public string Status { get; set; }
        public string PaymentMode { get; set; }
        public int AddressID { get; set; }

        // Navigation properties
        public virtual AppUser? Person { get; set; }
        public virtual Business? Business { get; set; }
        public virtual Address? Address { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
