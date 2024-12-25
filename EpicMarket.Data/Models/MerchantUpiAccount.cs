using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class MerchantUpiAccount : BaseModel
    {
        public int ID { get; set; }
        public int MerchantFinanceId { get; set; }
        public string UpiIdentifier { get; set; }
        public string MerchantName { get; set; }
        public string QrCodeUrl { get; set; }
        public bool IsPrimaryAccount { get; set; }
        // Navigation Property
        public virtual MerchantFinance MerchantFinance { get; set; }
    }

}
