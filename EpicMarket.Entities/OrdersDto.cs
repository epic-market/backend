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

        public string PaymentMode { get; set; }

        public string OrderedMode { get; set; }

        public string Status { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string OrderDetails { get; set; } // this List<OrderDetailsDto> convert to json to string
    }
}
