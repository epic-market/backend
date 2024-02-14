using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class OutletProduct
    {
        public int ID { get; set; }

        public int OutletID { get; set; }

        public int ProductID { get; set; }

        public virtual Catalog Product { get; set; }

        public virtual Outlet Outlet { get; set; } 
    }
}
