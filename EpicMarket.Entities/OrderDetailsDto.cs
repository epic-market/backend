using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class OrderDetailsDto
    {
        public int VariantID { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDetails
    {
        public int CatalogID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalPrice { get; set; }

        public string Thumbnail { get; set; }
    }


}
