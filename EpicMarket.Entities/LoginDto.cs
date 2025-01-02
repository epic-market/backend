using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

		[Required]
		public string Password { get; set; }
    }

    public class LoginPhoneDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string OTP { get; set; }
        [Required]
        public string ReferenceId { get; set; }
    }
}
