using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Order : BaseModel
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int BusinessID { get; set; }
        public string OrderType { get; set; } //online or offline
        public double TotalPrice { get; set; }// total value of the order
        public int TotalItems { get; set; } //total quantity of the items
        public DateTime OrderAt { get; set; } //date time
        public string Status { get; set; } // delivered, packing 
        public string PaymentMode { get; set; } // cash , online
        public int? AddressID { get; set; }

        // Navigation properties
        public virtual AppUser? Person { get; set; }
        public virtual Business? Business { get; set; }
        public virtual Address? Address { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
