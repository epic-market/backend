using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
