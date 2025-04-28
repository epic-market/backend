using EpicMarket.Data.Models;
using System.Reflection.Metadata.Ecma335;

namespace EpicMarket.Admin.MVC.Models
{
	public class CatelogModel
	{
        public EpicMarket.Data.Models.Catalog Product { get; set; }

		public Inventory Inventory { get; set; }

		public List<Inventory> Inventorys { get; set; }
    }
}
