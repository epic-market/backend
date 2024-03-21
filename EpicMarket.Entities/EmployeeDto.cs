using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class EmployeeDto
    {
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
}
