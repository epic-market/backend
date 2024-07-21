using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    internal class ProfileDto
    {
    }

    public class Profile_SearchParams 
    {
        public string LoggedInUserName { get; set; }
        public string ApplicationName { get; set; }
        public string UserRole { get; set; }
    }

}
