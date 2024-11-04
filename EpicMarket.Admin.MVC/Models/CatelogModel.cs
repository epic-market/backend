using EpicMarket.Data.Models;
using System.Reflection.Metadata.Ecma335;

namespace EpicMarket.Admin.MVC.Models
{
	public class CatelogModel
	{
        public Catalog Catalog { get; set; }

		public OutletProduct OutletProduct { get; set; }

		public List<OutletProduct> OutletProducts { get; set; }
    }
}
