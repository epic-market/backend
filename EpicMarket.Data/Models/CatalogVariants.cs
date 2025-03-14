using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpicMarket.Data.Models
{
    public class CatalogVariants : BaseModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [ForeignKey("Catalog")]
        public int CatalogID { get; set; }
        [Required]
        [StringLength(50)]
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string Attributes { get; set; }//{"Size" : XL , "Color" : Red}
        [Required]
        public double CostPrice { get; set; } //cost of product for percharse
        [Required]
        public double SalePrice { get; set; } //what price he what to show to users
        public double? CompareAtPrice { get; set; }
        public string AdditionalHightlights { get; set; }
        public int? MaximumOrderQuantity { get; set; }
        public int? MinimumOrderQuantity { get; set; }
        public double? PackedHeight { get; set; }
        public double? PackedWidth { get; set; }
        public double? PackedDepth { get; set; }
        public string WeightUnit { get; set; }//kg,g,lbs,oz
        public double? Weight { get; set; } //value of weight
        public bool IsDefaultVariant { get; set; }
        public virtual Catalog Catalog { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
