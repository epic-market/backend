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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string Attributes { get; set; }

        public virtual Catalog Catalog { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }



    
    }
}
