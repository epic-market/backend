using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
	public class StatusOptionSet
	{
        public int Id { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }

		public virtual ICollection<Business> Businesses { get; set; }

	}
}
