using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class BusinessFilterViewModel
    {
        public string BusinessId { get; set; }
        public string OwnerUsername { get; set; }
        public string ContactNumber { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }

    public class BusinessDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string StatusName { get; set; }
        public string PersonUserName { get; set; }
        public int PersonId { get; set; }
        public string BusinessCategoryName { get; set; }
        public string City { get; set; }
    }

    public class BusinessDataResponse
    {
        public int TotalRecords { get; set; }
        public List<Business> Data { get; set; }
    }
}
