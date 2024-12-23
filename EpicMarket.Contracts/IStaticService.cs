using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IStaticService
    {
        Task<List<DropDownOptions>> BusinessCategoriesOptions();

        Task<List<DropDownOptions>> GetStatusOptions();
        Task<List<DropDownOptions>> GetOderStatusOptions();
        Task<List<DropDownOptions>> GetOderTypeOptions();
        Task<List<DropDownOptions>> GetAllblogCategories();
        Task<List<DropDownOptions>> GetAllSupportCategorys();
        Task<List<DropDownOptions>> GetAllPersonTypes();
        Task<List<DropDownOptions>> GetAllSupportQuery(int personTypeId); 
        Task<List<HelpItemDTO>> GetHelpItemsforBypage(string pagename);

        Task<List<DropDownOptions>> GetOrderStatusOptions();
        Task<List<DropDownOptions>> GetProofTypes();
        Task<int> SubscribeforOffers( string gmail);

    }
}
