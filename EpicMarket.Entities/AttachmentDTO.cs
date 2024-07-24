using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class AttachmentDTO
    {
        public int ID { get; set; }  
        public String AttachmentTypeName { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string DocumentType { get; set; }
        public string DocumentFileType { get; set; }
        public string DocumentFolderPath { get; set; }
        public string DocumentFile { get; set; }
    }
    public class SaveFileDTO
    {
        public string FileName { get; set; }
        public string FullPathLocation { get; set; }
    }
}
