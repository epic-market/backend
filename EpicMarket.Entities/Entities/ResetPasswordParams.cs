using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.Entities
{
    public class ResetPasswordParams
    {
        [Required]
        public string Email { get; set; }

		[Required]
		public string Path { get; set; }
    }
    public class SetNewPasswordParams
    {
        [Required]
        public string password { get; set; }
        [Required]
        public string token { get; set; }
    }
    public class CheckResetLinkResult
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }

        public string Email { get; set; }
    }

}
