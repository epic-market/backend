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
    public class Tasks : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } //grivarance

        public string Description { get; set; }//comment

        public int? TaskTypeID { get; set; } // support , verification , help , onboarding 
        public int? ParentID { get; set; }
        public int? TaskStatusID { get; set; } //default  new

        public int? TaskPriorityID { get; set; } // default 1
        [ForeignKey("Entity")]
        public int? TaskEntityID { get; set; }

        public int? PrimaryAssignedToPersonID { get; set; } // default admin@epicmarket.in

        public DateTime? DateAssigned { get; set; } //which date

        public DateTime? DateDue { get; set; } //get date form the tasktype and add the days to current date

        public DateTime? DateStarted { get; set; } //null

        public DateTime? DateCompleted { get; set; }// null

        public int? SubmittedByPersonID { get; set; }// null

        public string TaskData { get; set; } // null
        public DateTime? ReceivedDate { get; set; } //todaty
        [ForeignKey("TaskTypeID")]
        public virtual TaskType TaskTypes { get; set; }
        [ForeignKey("TaskStatusID")]
        public virtual TaskStatusType TaskStatusType { get; set; }
        public virtual Entity Entity { get; set; }

    }
}
