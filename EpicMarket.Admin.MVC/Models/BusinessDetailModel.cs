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

		public List<EpicMarket.Data.Models.Catalog> Products { get; set; }

		public EpicMarket.Data.Models.Catalog Product { get; set; }


		public List<EventLog> EventLogs { get; set; }

		public EventLog EventLog { get; set; }

		public List<string> AttachmentProofs { get; set; }

		public string Attachment { get; set; }
	}


    public class AttachmentProof
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
