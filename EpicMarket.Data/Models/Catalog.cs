using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }
        public string? Category { get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public bool InStock { get; set; }
        public bool IsRecommended { get; set; }
        public int? MaximumOrderPurchase { get; set; }
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public int? OrderCount { get; set; }
        public string Status { get; set; }

        // Navigation property
        public virtual Business? Business { get; set; }
    }
}
