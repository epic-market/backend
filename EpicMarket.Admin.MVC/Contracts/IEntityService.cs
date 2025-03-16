using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Admin.MVC.Contracts
{
    public interface IEntityService
    {
        Task<int> GetEntityId(string entityName);
    }
}