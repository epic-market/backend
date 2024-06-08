using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
	public class OutletsDetailsModel
	{
        public Outlet Outlet { get; set; }

        public List<OutletPerson> OutletEmployees { get; set; }

		public OutletPerson OutletEmployee { get; set; }

		public List<OutletProduct> OutletProducts { get; set; }

		public OutletProduct OutletProduct { get; set; }

		public List<Order> Orders { get; set; }

		public Order Order { get; set; }

	}
}
