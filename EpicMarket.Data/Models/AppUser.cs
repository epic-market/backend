
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class AppUser :IdentityUser<int>
    {
        // Foreign key to Address table


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UniqueGuid { get; set; }
        public int OTP { get; set; }

        public bool IsActive { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;
 
        public virtual ICollection<Business> Businesses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }

        public virtual ICollection<OutletPerson> OutletPeople { get; set; }

        public virtual ICollection<SupportTicket> SupportTickets { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }

        public virtual ICollection<BusinessEmployeeMap> BusinessEmployeeMaps { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }

    }
}
