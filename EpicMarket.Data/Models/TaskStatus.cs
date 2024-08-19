using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class TaskStatusType:BaseModel
    {
        public int Id { get; set; }
        [MaxLength(255)]

        [DisplayName("Task Status")]
        public string Status { get; set; }
        public string StatusDescription { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }

    }
}
