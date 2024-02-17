using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class UserAddress
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }

        public virtual AppUser  User { get; set; }

        public virtual Address Address { get; set; }
    }
}
