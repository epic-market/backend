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
        public string Description { get; set; }  // This will serve as excerpt
        public string InnerHtml { get; set; }    // This will serve as content
        public string Authour { get; set; }      // Keeping original spelling for compatibility
        public string AuthorName { get; set; }   // Adding to match frontend model
        public string AuthorImage { get; set; }  // Adding to match frontend model
        public string AuthorBio { get; set; }    // Adding to match frontend model
        public DateTime PublishDate { get; set; }
        public int ReadTime { get; set; }
        public int Count { get; set; }
        public string BlogCategoryName { get; set; }
        public int BlogCategoryId { get; set; }  // Adding to track category ID
        public List<string> Tags { get; set; } = new List<string>();
        public List<BlogDto> RelatedPosts { get; set; } = new List<BlogDto>();
    }

    public class BlogCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class BlogParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string searchTerm { get; set; } = string.Empty;
        public int? CategoryId { get; set; } = null;  // Adding category filter
    }
    
    // Keeping this for backward compatibility if needed
    public class BlogsByCategoryParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string categoryName { get; set; } = string.Empty;
    }
}
