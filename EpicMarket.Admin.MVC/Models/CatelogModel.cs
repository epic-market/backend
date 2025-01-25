using EpicMarket.Data.Models;
using System.Reflection.Metadata.Ecma335;

namespace EpicMarket.Admin.MVC.Models
{
	public class CatelogModel
	{
        public Catalog Catalog { get; set; }

		public Inventory Inventory { get; set; }

		public List<Inventory> Inventorys { get; set; }
    }
}
