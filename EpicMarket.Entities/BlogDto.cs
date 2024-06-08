using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string InnerHtml { get; set; }
        public string Authour { get; set; }
        public int Count { get; set; }
        public string BlogCategoryName { get; set; }
    }

    public class BlogParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string searchTerm { get; set; } = string.Empty;

    }
    public class BlogsByCategoryParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string categoryName { get; set; } = string.Empty;

    }

}
