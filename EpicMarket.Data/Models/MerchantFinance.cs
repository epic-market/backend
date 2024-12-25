using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class MerchantFinance : BaseModel
    {
        public int Id { get; set; }
        public int OutletID { get; set; }

        // Navigation Properties
        public ICollection<MerchantUpiAccount> UpiAccounts { get; set; }
        public ICollection<MerchantBankAccount> BankAccounts { get; set; }
        public virtual Outlet Outlet { get; set; }
    }
}
