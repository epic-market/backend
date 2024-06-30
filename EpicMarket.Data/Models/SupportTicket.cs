using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class SupportTicket
    {
        [Key]
        public int ID { get; set; }

        public string Email { get; set; }

        public string Phonenumber { get; set; }
        public string Fullname { get; set; }
        [ForeignKey("PersonType")]
        public int TypeofPersonid { get; set; }
        [ForeignKey("TaskStatusType")]
        public int? TaskStatusID { get; set; }
   
        public virtual TaskStatusType TaskStatusType { get; set; }
        public virtual PersonType PersonType { get; set; }
    }
}
