using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Common
{
    public class BaseModel
    {
        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
