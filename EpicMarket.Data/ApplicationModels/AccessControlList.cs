using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.ApplicationModels
{
    public class AccessControlList
    {
        public int ID { get; set; }

        public int RoleID { get; set; }

        public int AccessTypeID { get; set; }

        public int SecurableID { get; set; }

        public virtual AppRole Role { get; set; }

        public virtual AccessType AccessType { get; set; }

        public virtual ApplicationSecurables Securable { get; set; }
    }
}
