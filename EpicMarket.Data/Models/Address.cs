using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Address : BaseModel
    {
        [DisplayName("Address ID")]
        public int Id { get; set; }

        [DisplayName("Address")]
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? MetaData { get; set; }


     
        public virtual ICollection<AppUser> Persons { get; set; }

        public virtual ICollection<Business> Businesses { get; set; }

        public virtual ICollection<Order>   Orders { get; set; }
        public virtual ICollection<Outlet> Outlets { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }
    }
}
