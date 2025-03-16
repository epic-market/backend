using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Order : BaseModel
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int OutletID { get; set; }

        [ForeignKey("OrderTypesOptions")]
        public int OrderTypeId { get; set; } //online or offline
        public double TotalPrice { get; set; }// total value of the order
        public int TotalItems { get; set; } //total quantity of the items
        public DateTime OrderAt { get; set; } //date time

        [ForeignKey("OrderStatusOptions")]
        public int StatusId { get; set; } // delivered, packing 
        public string PaymentMode { get; set; } // cash , online
        public int? AddressID { get; set; }
        public virtual AppUser? Person { get; set; }
        public virtual Outlet? Outlet { get; set; }

        [JsonIgnore]
        public virtual Address? Address { get; set; }

        public virtual OrderStatusOptions? OrderStatusOptions { get; set; }

        public virtual OrderTypesOptions? OrderTypesOptions { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class PlaceOrderViewModel
    {
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public int OutletId { get; set; }
        public double TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public string PaymentMode { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalPrice { get; set; }
    }
}
