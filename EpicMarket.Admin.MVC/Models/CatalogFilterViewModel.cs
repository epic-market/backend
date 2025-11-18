using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class ProductFilterViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }

    public class ProductDto
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