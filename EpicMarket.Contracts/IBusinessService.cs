using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IBusinessService
    {
        Task<BusinessDTO_Result> RegisterBusiness(BusinessRegisterDto businessRegisterDto , string UserName, int AdminPersonId ,int userID,string PageSource);

        Task<BusinessDetailResult> GetBusinessByID(int businessId);

        Task<int> UpdateBusiness(int id, UpdateBusinessRegisterDto businessRegisterDto, string UserName,int AdminPersonId, string PageSource);
        
        Task<List<BusinessCategoryDto>> GetBusinessCategories();

        Task<BusinessGroupsResponseDto> GetBusinessListings(string categoryFilter = null);
    }
}
