using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IProfileService
    {

        //List<Menu_Result> GetMenus(Profile_SearchParams searchParams);

        List<AccessControlList_Result> GetAccessControlList(Profile_SearchParams searchParams);
    }
}
