using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class BlogCategory : BaseModel
    {
        [DisplayName("BlogCategory ID")]
        public int Id { get; set; }

        [DisplayName("BlogCategory Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Blog>? Blogs { get; set; }
        // _context.BlogCategory.where(id=> id==paramID).include(c=>c.blogs)
    }
}
