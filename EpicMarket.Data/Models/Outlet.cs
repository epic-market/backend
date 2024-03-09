using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Outlet :BaseModel
    {
        public int ID { get; set; }
        public int BussinessID { get; set; }
        public int AddressID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public int? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public bool? IsOpen { get; set; }
        public double? Weight { get; set; }
        public bool? Status { get; set; }

        public virtual Business Bussiness { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<OutletPerson> OutletPeople { get; set; }

        public virtual ICollection<OutletProduct> OutletProducts { get; set; }
    }
}
