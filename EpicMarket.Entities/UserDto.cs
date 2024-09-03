using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
    public class UserBusinessDto
	{
		public int? businessId { get; set; }

        public string businessStatus { get; set; }
    }

	public class LoginResult
	{
        public UserLoginDto UserDetails { get; set; }

        public UserBusinessDto UserBusiness { get; set; }
        public List<AccessControlList_Result> Securables { get; set; }
    }
    public class TokenDto
    {
        public string Token { get; set; }
    }

    public class CustomerDetails
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
    }
    
}
