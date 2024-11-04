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
    public class Catalog : BaseModel
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }
        public long? Barcode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Category { get; set; }
        public double Rate { get; set; }
        public bool IsRecommended { get; set; }
        public int? MaximumOrderPurchase { get; set; }
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public int? OrderCount { get; set; }
        public double PackedHeight { get; set; }
        public double PackedWidhth { get; set; }
        public double PackedDepth { get; set; }
        public double Weight { get; set; }
        public bool RequiresRefrigeration { get; set; }
        public double CostPrice { get; set; }


        [ForeignKey("StatusOptionSets")]
        public int StatusId { get; set; }

        // Navigation property
        public virtual Business Business { get; set; }

        public virtual StatusOptionSet StatusOptionSets { get; set; }

        public virtual ICollection<OutletProduct> OutletProducts { get; set; }
    }
}
