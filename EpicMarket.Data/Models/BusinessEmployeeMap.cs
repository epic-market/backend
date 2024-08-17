using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class BusinessEmployeeMap : BaseModel
    {
        public int ID { get; set; }

        public int BussinessID { get; set; }

        public int EmployeeID { get; set; }

        public virtual AppUser Employee { get; set; }

        public virtual Business Bussiness { get; set; }
    }
}
