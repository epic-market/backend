using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class ChangePasswordParams
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }

}
