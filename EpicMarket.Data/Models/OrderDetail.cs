using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class OrderDetail : BaseModel
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        
        [Required]
        [ForeignKey("ProductVariants")]
        public int VariantID { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public double Rate { get; set; }
        
        [Required]
        public double TotalPrice { get; set; }

        // Navigation properties
        public virtual Order? Order { get; set; }
        public virtual CatalogVariants? ProductVariants { get; set; }
    }
}
