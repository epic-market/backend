using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class TasksDTO
    {
        public int? ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? TaskTypeID { get; set; }
        public int? ParentID { get; set; }
        public string TaskStatus { get; set; }

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
        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
    public class CommentDTO
    {
        public string CommentText { get; set; }

        public string Status { get; set; }

        public int? RecordID { get; set; }
        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }
    }
    public class SupportDTO
    {

        public string Email { get; set; }

        public string Phonenumber { get; set; }
        public string Fullname { get; set; }
        public int TypeofPersonid { get; set; }
 
        public int? QueryId { get; set; }
        public string Comment { get; set; }


    }

}
