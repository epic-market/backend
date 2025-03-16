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
    public class SupportQueries:BaseModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Query { get; set; }

        [ForeignKey("TaskTypes")]
        public int? TaskTypeID { get; set; }
        
        [ForeignKey("PersonType")]
        public int? TypeofPersonid { get; set; }
        
        public virtual PersonType PersonType { get; set; }
        public virtual TaskType TaskTypes { get; set; }
    }
}
