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
    public class Tasks: BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? TaskTypeID { get; set; }

        public int? TaskStatusID { get; set; }

        public int? TaskPriorityID { get; set; }

        public int? PrimaryAssignedToPersonID { get; set; }

        public DateTime? DateAssigned { get; set; }

        public DateTime? DateDue { get; set; }

        public DateTime? DateStarted { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int? SubmittedByPersonID { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string TaskData { get; set; }
        public DateTime? ReceivedDate { get; set; }
        [ForeignKey("TaskTypeID")]
        public virtual TaskType TaskTypes { get; set; }
        [ForeignKey("TaskStatusID")]
        public virtual StatusOptionSet StatusOptionSets { get; set; }

    }
}
