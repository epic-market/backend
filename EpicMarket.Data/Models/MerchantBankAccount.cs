using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class MerchantBankAccount : BaseModel
    {
        public int ID { get; set; }
        public int MerchantFinanceId { get; set; }
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountHolderName { get; set; }
        public bool IsPrimaryAccount { get; set; }

        public virtual MerchantFinance MerchantFinance { get; set; }
    }
}
