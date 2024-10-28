using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
	public class BusinessDetailModel
	{
        public Business Business { get; set; }

        public List<Outlet> Outlets { get; set; }

		public Outlet Outlet { get; set; }


		public List<BusinessEmployeeMap> employees { get; set; }

		public BusinessEmployeeMap employee { get; set; }

		public List<Catalog> Catalogs { get; set; }

		public Catalog Catalog { get; set; }


		public List<EventLog> EventLogs{ get; set; }

		public EventLog EventLog { get; set; }
	}
}
