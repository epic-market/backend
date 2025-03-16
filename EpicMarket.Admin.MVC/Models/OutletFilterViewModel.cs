using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Models
{
    public class OutletFilterViewModel
    {
        public string OutletId { get; set; }
        public string OutletName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; } = "id";
        public string SortDirection { get; set; } = "asc";
    }

    public class OutletDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public int BusinessID { get; set; }
        public long ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string City { get; set; }
        public string StatusName { get; set; }
        public bool IsOpen { get; set; }
    }
} 