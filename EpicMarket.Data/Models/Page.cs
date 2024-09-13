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
    public class Page : BaseModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        public virtual ICollection<HelpItem> HelpItems { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual ApplicationsTable ApplicationsTable { get; set; }
    }
}
