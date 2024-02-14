
using EpicMarket.Data.ApplicationModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }

        public AppRole Roles { get; set; }
    }
}
