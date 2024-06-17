using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Order : BaseModel
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int OutletID { get; set; }
        public int OrderTypeId { get; set; } //online or offline
        public double TotalPrice { get; set; }// total value of the order
        public int TotalItems { get; set; } //total quantity of the items
        public DateTime OrderAt { get; set; } //date time
        public int StatusId { get; set; } // delivered, packing 
        public string PaymentMode { get; set; } // cash , online
        public int? AddressID { get; set; }
        public virtual AppUser? Person { get; set; }
        public virtual Outlet? Outlet { get; set; }
        public virtual Address? Address { get; set; }
        [ForeignKey("StatusId")]
        public virtual OrderStatusOptions? OrderStatusOptions { get; set; }
        [ForeignKey("OrderTypeId")]
        public virtual OrderTypesOptions? OrderTypesOptions { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
