using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Blog : BaseModel
    {
        [DisplayName("Blog ID")]
        public int Id { get; set; }
        public int BlogCategoryID { get; set; }

        [DisplayName("Blog Title")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string InnerHtml { get; set; }
        public string Authour { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
