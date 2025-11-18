using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{
     public class EmailAttachmentModel
    {
        public string Filename { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Disposition { get; set; }
        public string ContentId { get; set; }
    }
}