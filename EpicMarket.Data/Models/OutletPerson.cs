using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class OutletPerson
    {
        public int ID { get; set; }

        public int PersonId { get; set; }

        public int  OutletId { get; set; }

        public virtual AppUser Person { get; set; }

        public virtual Outlet Outlet { get; set; }
    }
}
