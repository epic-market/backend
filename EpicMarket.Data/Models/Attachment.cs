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

        [ForeignKey("AttachmentType")]
        public int AttachmentTypeID { get; set; }
        public virtual AttachmentType AttachmentType { get; set; }

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
    }
}
