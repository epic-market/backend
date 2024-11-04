using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace EpicMarket.Data.Models
{
    public class OutletProduct
    {
        public int ID { get; set; }


        [ForeignKey("Outlet")]
        public int OutletID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required]
        [DefaultValue(0)]
        public int QuantityAvailable { get; set; }

        [Required]
        public int MinimumStockLevel { get; set; }

        [Required]
        public int MaximumStockLevel { get; set; }

        [Required]
        public int ReorderPoint { get; set; }

        [Required]
        public bool BackOrders { get; set; } //if true this will allow user to order if it is outofstock also

        public virtual Catalog Product { get; set; }

        public virtual Outlet Outlet { get; set; } 
    }
}
