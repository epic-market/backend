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
    public class Attachment:BaseModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Comment { get; set; }

        [Required]
        [MaxLength(100)]
        public string DocumentType { get; set; }

        [MaxLength(100)]
        public string DocumentFileType { get; set; }

        [MaxLength(500)]
        public string DocumentFolderPath { get; set; }

        public string DocumentFile { get; set; }

        public int?EntityId { get; set; } //to know which entity this attachment is related to maximum it will be for business

        public int? RecordId { get; set; }

		public virtual ICollection<AttachmentLink> AttachmentLinks { get; set; }

        public Entity Entity { get; set; }
	}
}
