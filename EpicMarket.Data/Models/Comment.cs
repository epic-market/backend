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
    public class Comment : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string CommentText { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

        public int? RecordID{ get; set; }
    }
}
