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
    [Table("ProductCategory")]
    public class CatalogCategory : BaseModel
    {
        public int ID { get; set; }

        public int BusinessID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public int? ParentID { get; set; }

        public ICollection<Catalog> Product { get; set; }

        public Business Business { get; set; }
    }
}

