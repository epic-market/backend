using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? OutletId { get; set; }
        public int Stars { get; set; } 
        public string Review { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsVerified { get; set; }
        public virtual AppUser Customer { get; set; }
        public virtual Outlet Outlet { get; set; }
        public virtual Catalog Product { get; set; }

    }

}
