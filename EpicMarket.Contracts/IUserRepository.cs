using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IUserRepository
    {
        string LoggedInUsername { get; }
        List<AccessControlList> Permissions { get; }

        bool HasPermission(string username, string securable);

        Task<AppUser> GetUserByIdAsync(int id);

        bool IsBusinessVerified(int id);
    }
}
