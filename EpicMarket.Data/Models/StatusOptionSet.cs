using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{// which is being used by both Businesses,Tasks for now
    public class StatusOptionSet : BaseModel
	{
        public int Id { get; set; }
        [MaxLength(255)]
        public string Status { get; set; }
        public string StatusDescription { get; set; }

		public virtual ICollection<Business> Businesses { get; set; }
        public virtual ICollection<Outlet> Outlets { get; set; }
        public virtual ICollection<Catalog> Catalogs { get; set; }

    }
}
