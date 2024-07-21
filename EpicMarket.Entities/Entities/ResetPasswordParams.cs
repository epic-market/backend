using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.Entities
{
    public class ResetPasswordParams
    {
        public string email { get; set; }
        public string path { get; set; }
    }
    public class SetNewPasswordParams
    {
        public string password { get; set; }
        public string token { get; set; }
    }
    public class CheckResetLinkResult
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }

        public string Email { get; set; }
    }

}
