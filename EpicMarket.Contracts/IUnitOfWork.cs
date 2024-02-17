using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
