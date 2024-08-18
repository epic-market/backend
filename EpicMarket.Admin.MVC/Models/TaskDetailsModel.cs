using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
	public class TaskDetailsModel
	{
        public Tasks Task { get; set; }
        public List<Tasks> SubTasks { get; set; }
		public Tasks SubTask { get; set; }
		public List<Comment> Comments { get; set; }
		public Comment Comment { get; set; }
		public List<EventLog> EventLogs { get; set; }
		public EventLog EventLog { get; set; }
	}
}
