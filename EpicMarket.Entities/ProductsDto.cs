using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class ProductsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category{ get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public bool InStock { get; set; }
        public bool IsRecomended { get; set; }
        public int MaximumPurchaceOrder  { get; set; }
        public string Images { get; set; }
        public int BusinessID { get; set; }
    }
    public class ProductsMapOptionResult 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public double Rate { get; set; }

        public int Selected { get; set; }
    }
}
