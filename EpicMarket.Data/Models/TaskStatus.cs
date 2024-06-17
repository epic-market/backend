using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class TaskStatusType:BaseModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }

    }
}
