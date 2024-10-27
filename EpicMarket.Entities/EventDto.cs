using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class EVENT_LOG_SAVE_PARAMS 
    {

        public string EventName { get; set; }
        public string EntityName { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public int? RecordId { get; set; }
        public int BusinessID { get; set; }
        public string LoggedInUserName { get; set; }
    }
}
