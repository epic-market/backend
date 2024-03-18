using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class OutletProduct
    {
        public int ID { get; set; }


        [ForeignKey("Outlet")]
        public int OutletID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public virtual Catalog Product { get; set; }

        public virtual Outlet Outlet { get; set; } 
    }
}
