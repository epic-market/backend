using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class EmployeeDto
    {
        public int ID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string Address { get; set; }

        public int Pincode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Password { get; set; }

        public string ProfilePicture { get; set; }


    }

    public class AddEmployeeParam 
    {
        public int BusinessID { get; set; }

        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string EmailID { get; set; }
    }

    public class AddEmployeeResult
    {
        public int BusinessID { get; set; }

        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string EmailID { get; set; }
    }

    public class CheckLinkResult
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessEmail { get; set; }
    }

    public class EmployeeMapOptionResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Selected { get; set; }
    }
}
