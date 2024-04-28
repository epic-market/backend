using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class OrdersDto
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int BusinessID { get; set; }

        public string PaymentMode { get; set; }

        public string OrderedMode { get; set; }

        public string Status { get; set; }
        
        public double TotalPrice { get; set; }
        
        public int TotalItems { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string OrderDetails { get; set; } // this List<OrderDetailsDto> convert to json to string
    }

    public class OrderParams
    {
        public int BusinessId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public bool ascending { get; set; } = true;
        public string searchTerm { get; set; } = string.Empty;

    }

    public class OrderResult
    {
        public int ID { get; set; }

        public string CustomerName { get; set; }

        public string Status { get; set; }

        public double TotalPrice { get; set; }

        public int TotalItems { get; set; }

        public string OrderType { get; set; }

        public int Count { get; set; }
    }

}
