using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
 public class PopularProductChart
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class GMVChart
    {
        public string Month { get; set; }
        public decimal Value { get; set; }
    }

    public class ActiveUserChart
    {
        public string X { get; set; }
        public int Customer { get; set; }
    }
}