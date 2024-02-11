
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class AppUser :IdentityUser<int>
    {
        // Foreign key to Address table
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }   

        public virtual ICollection<Business> Businesses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}
