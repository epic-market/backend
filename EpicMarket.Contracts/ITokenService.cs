using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Entities;
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
        Task<string> ResetPassword(ResetPasswordParams resetPassword);
        Task<string> SetNewPassword(SetNewPasswordParams setNewPasswordParams);
        CheckResetLinkResult CheckResetPasswordLink(string queryParam);

    }
}
