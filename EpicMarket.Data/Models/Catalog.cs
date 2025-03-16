using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EpicMarket.Data.Models
{
    public class Catalog : BaseModel
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CategoryID { get; set; }
        public bool IsRecommended { get; set; }
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public int? OrderCount { get; set; }
        public bool RequiresRefrigeration { get; set; }
        public string BaseHightlights { get; set; }
        public string VariantOptions { get; set; }

        [ForeignKey("StatusOptionSets")]
        public int StatusId { get; set; }

        // Navigation property
        public virtual Business Business { get; set; }

        public virtual StatusOptionSet StatusOptionSets { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<CatalogVariants> CatalogVariants { get; set; }

    }
}
