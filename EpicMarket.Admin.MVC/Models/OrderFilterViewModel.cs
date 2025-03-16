using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Models
{
    public class OrderFilterViewModel
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string OutletName { get; set; }
        public string OrderStatus { get; set; }
        public string OrderType { get; set; }
        public string PaymentMode { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "orderat";
        public string SortDirection { get; set; } = "desc";
    }

    public class OrderDto
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string OutletName { get; set; }
        public int OutletId { get; set; }
        public double TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public DateTime OrderAt { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string OrderTypeName { get; set; }
        public int OrderTypeId { get; set; }
        public string PaymentMode { get; set; }
    }
} 