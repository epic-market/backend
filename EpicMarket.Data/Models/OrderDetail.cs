using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int CatalogID { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalPrice { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Catalog Catalog { get; set; }
    }
}
