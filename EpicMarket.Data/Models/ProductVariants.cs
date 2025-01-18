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
    public class ProductVariants : BaseModel
    {
        [Key]
        public int VariantID { get; set; }

        [Required]
        [ForeignKey("Catalog")]
        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        [Required]
        public string VariantName { get; set; }

        [Required]
        public double CostPrice { get; set; } //cost of product for percharse

        [Required]
         public double SalePrice { get; set; } //what price he what to show to users

        [Required]
        public string VariantAttributes { get; set; }

        public virtual Catalog Catalog { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }



    
    }
}
