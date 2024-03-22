using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Business : BaseModel
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int BusinessCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Banner { get; set; }
        public string? Logo { get; set; }
        public long ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public int AddressID { get; set; }
        public int? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public bool IsOpen { get; set; }
        public double? Weight { get; set; }
        public bool Status { get; set; }

        // Navigation properties
        public virtual AppUser? Person { get; set; }
        public virtual BusinessCategoryInternal? BusinessCategory { get; set; }
        public virtual Address? Address   { get; set; }

        public virtual ICollection<Outlet> Outlets { get; set; }

        public virtual ICollection<BusinessEmployeeMap> BusinessEmployees { get; set; }
    }
}
