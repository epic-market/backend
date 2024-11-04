using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public int ContactMethodId { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }

        public AppUser User { get; set; } // Navigation property for the user
        public ContactMethod ContactMethod { get; set; }

    }
}
