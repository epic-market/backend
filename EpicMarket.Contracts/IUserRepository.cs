using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IUserRepository
    {
        List<AccessControlList_Result> Permissions { get; }

        bool HasPermission(string username, string securable);

        Task<AppUser> GetUserByIdAsync(int id);

        bool IsBusinessVerified(int id);
    }
}
