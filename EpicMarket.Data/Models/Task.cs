using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Data.Common;
using System.ComponentModel;

namespace EpicMarket.Data.Models
{
    public class Tasks : BaseModel
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(200)]
        [DisplayName("Task Name")]
        public string Name { get; set; } //grivarance 

        [DisplayName("Task Description")]
        public string Description { get; set; }//comment


        [DisplayName("Task Type")]
        public int? TaskTypeID { get; set; } // support , verification , help , onboarding 

        public int? ParentID { get; set; }

        [DisplayName("Task Status")]
        public int? TaskStatusID { get; set; } //default  new


        [DisplayName("Task Priority")]
        public int? TaskPriorityID { get; set; } // default 1
       
        [ForeignKey("Entity")]
        public int? TaskEntityID { get; set; }

        [DisplayName("Assigned Person")]
        public int? PrimaryAssignedToPersonID { get; set; } // default admin@epicmarket.in

        [DisplayName("Assigned Date")]
        public DateTime? DateAssigned { get; set; } //which date

        [DisplayName("Due Date")]
        public DateTime? DateDue { get; set; } //get date form the tasktype and add the days to current date

        [DisplayName("Started Date")]
        public DateTime? DateStarted { get; set; } //null

        [DisplayName("Completed Date")]

        public DateTime? DateCompleted { get; set; }// null


        [DisplayName("Submitted By")]
        public int? SubmittedByPersonID { get; set; }// null in case support //but in case the business registration add the person id of the business user


        [DisplayName("Task Details")]
        public string TaskData { get; set; } // null

        //add the column entityid
        public DateTime? ReceivedDate { get; set; } //todaty
        [ForeignKey("TaskTypeID")]
        public virtual TaskType TaskTypes { get; set; }
        [ForeignKey("TaskStatusID")]
        public virtual TaskStatusType TaskStatusType { get; set; }
        public virtual Entity Entity { get; set; }


        [ForeignKey("PrimaryAssignedToPersonID")]
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<SupportTicket> SupportTickets { get; set; }

    }
}
