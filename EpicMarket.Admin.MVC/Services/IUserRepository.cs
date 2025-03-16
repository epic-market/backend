using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Services
{
    public interface IUserRepository
    {
        bool HasPermission(string username, string securable);
        Task<IEnumerable<string>> GetUserPermissions(string username);
    }
} 