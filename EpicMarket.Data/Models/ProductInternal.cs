using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class ProductInternal
    {
        public int ID { get; set; }

        public string BarCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Images { get; set; }
    }
}
