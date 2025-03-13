using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class CatalogFilterViewModel
    {
        public string CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }

    public class CatalogDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public int BusinessID { get; set; }
        public string CategoryName { get; set; }
        public double Rating { get; set; }
        public string StatusName { get; set; }
        public bool IsRecommended { get; set; }
    }
} 