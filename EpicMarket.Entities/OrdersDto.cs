using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class OrdersDto
    {
        public int OutletId { get; set; }
        public string PaymentMode { get; set; }
        public int StatusId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public  List<OrderDetailsDto> orderDetailsDtos { get; set; } // this List<OrderDetailsDto> convert to json to string
    }


    public class OrdersDetailsResult
    {
        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string PaymentMode { get; set; }

        public string OrderMode { get; set; }

        public string Status { get; set; }

        public double TotalPrice { get; set; }

        public int TotalItems { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } 
    }




    public class OrderParams
    {
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
