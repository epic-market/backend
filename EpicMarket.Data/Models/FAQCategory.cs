using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class FAQCategory : BaseModel
    {
        public int Id { get; set; }
        public string CategoryTitle { get; set; }
        public string TypeOfFAQ { get; set; } //for customer,Business

        public virtual ICollection<FAQ>? FAQs { get; set; }
    }
}
