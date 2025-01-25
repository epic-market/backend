using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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


        [DisplayName("Outlet Name")]
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public long ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public bool IsOpen { get; set; }
        public double? Weight { get; set; }

        [ForeignKey("StatusOptionSets")]
        public int StatusId { get; set; }

        public virtual Business Bussiness { get; set; }
        public virtual Address Address { get; set; }


        public virtual StatusOptionSet StatusOptionSets { get; set; }

        public virtual ICollection<OutletPerson> OutletPeople { get; set; }

		public virtual ICollection<Order> Orders { get; set; }



		public virtual ICollection<Inventory> Inventory { get; set; }

        public virtual ICollection<Rating>  Ratings { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual MerchantFinance MerchantFinances { get; set; }

    }
}
