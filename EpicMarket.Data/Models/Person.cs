using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Foreign key to Address table
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public virtual ICollection<Business> Businesses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
