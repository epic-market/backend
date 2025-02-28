using EpicMarket.Data.Models;
using System.Collections.Generic;

namespace EpicMarket.Admin.MVC.Models
{
    public class DashboardViewModel
    {
        public int BusinessCount { get; set; }
        public int OutletCount { get; set; }
        public int OrderCount { get; set; }
        public double TotalRevenue { get; set; }
        public string UserFirstName { get; set; }
        public string GreetingMessage { get; set; }
        public List<Tasks> RecentTasks { get; set; }
    }
} 