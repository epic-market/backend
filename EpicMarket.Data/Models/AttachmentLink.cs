using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Data.Common;

namespace EpicMarket.Data.Models
{
    public class AttachmentLink:BaseModel
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Attachment")]
        public int AttachmentID { get; set; }
        public virtual Attachment Attachments { get; set; }

        [ForeignKey("Entity")]
        public int EntityID { get; set; }
        public virtual Entity Entity { get; set; }


		[ForeignKey("AttachmentType")]
		public int AttachmentTypeID { get; set; }
		public virtual AttachmentType AttachmentType { get; set; }

		public int RecordID { get; set; }
    }
}
