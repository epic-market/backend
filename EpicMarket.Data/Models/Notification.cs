using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public string Type { get; set; }
        public string Link { get; set; } //any link that can be redirect them
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } // Navigation property for the user
    }
}
