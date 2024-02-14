
using EpicMarket.Data.ApplicationModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class AppRole :IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<AccessControlList> AccessControlLists { get; set; }
    }
}
