using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class AttachmentLinkDTO
    {
        public int ID { get; set; }
        public int AttachmentID { get; set; }
        public int EntityID { get; set; }
        public int RecordID { get; set; }
    }

}
