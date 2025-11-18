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
    public class Inventory
    {
        public int ID { get; set; }


        [ForeignKey("Outlet")]
        public int OutletID { get; set; }

        [ForeignKey("ProductVariants")]
        public int ProductVariantID { get; set; }

        [Required]
        public bool TrackInventory { get; set; }  // If false, system won't auto-track inventory
        public bool IsInStock { get; set; }  // Manual override for stock status
        public int? QuantityAvailable { get; set; } // Current stock
        public int? MinimumStockLevel { get; set; }  // Don't let stock go below this
        public int? MaximumStockLevel { get; set; } // Don't order more than this
        public int? ReorderPoint { get; set; } // Place new order when stock hits this level

        [Required]
        public bool BackOrders { get; set; } // if true this will allow user to order if it is outofstock also

        public virtual CatalogVariants ProductVariants { get; set; }

        public virtual Outlet Outlet { get; set; } 
    }
}
