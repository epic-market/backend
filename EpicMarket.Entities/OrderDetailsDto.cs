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
        public int CatalogID { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalPrice { get; set; }
    }
}
