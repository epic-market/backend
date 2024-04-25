using EpicMarket.Data.Models;

namespace EpicMarket.Admin.MVC.Models
{
    public class FAQCategoryModel
    {
        public List<FAQCategory> FAQCategory { get; set; }
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string Search { get; set; }

        public string OrderBy { get; set; }
    }
}
