using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Data.Common;

namespace EpicMarket.Data.Models
{
    public class TaskType : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } //support_userweb , support_userbusiness, verification

        [MaxLength(255)]
        public string Description { get; set; } 

        public int? TaskCategoryID { get; set; } //userwebapplication , businesswebapplication 

        public int? DefaultDueDateHours { get; set; }

        [MaxLength(20)]
        public string ShortDescription { get; set; }

        [ForeignKey("TaskCategoryID")]
        public virtual ApplicationsTable EventCategorys { get; set; }
        public virtual ICollection<Tasks>? Tasks { get; set; }
        public virtual ICollection<SupportQuerys>? SupportQuerys { get; set; }
    }
}
