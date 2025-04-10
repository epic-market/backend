using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IOutletService
    {
        Task<GetDataResult<List<BranchResult>>> GetAllBranches(BranchParams branchParams, int BusinessID);

        Task<int> AddBranch(BranchDto branchDto, string UserName, int BusinessID, string PageSource);

        Task<int> UpdateBranch(int id, BranchDto branchDto, string UserName, int BusinessID, string PageSource);

        Task<int> MapBranchToPeople(BranchPeopleMapParams branchPeopleMapParams);

        Task<int> MapBranchToProductVariants(BranchProductVariantMapParams branchProductVariantMap);

        Task<BranchDetailResult> GetBranchByID(int branchId);
        Task<int> VerifyBranchs(VerifyDto verifyBranchDto, string UserName, int AdminPersonID, string PageSource);

        Task<List<BranchsDropDownOptions>> GetAllOutletsForDropDown(int personID, int businessID);

        Task DeleteBranch(int branchId, string UserName);

        Task<GetDataResult<List<OutletSeachDto>>> GetNearbyOutletsAsync(OutletSearchRequest request);


        Task<GetDataResult<List<SubscribedOutletDto>>> GetSubscribedOutletsAsync(string customerUserName, int page = 1, int pageSize = 10);

        Task<bool> SubscribeOutletAsync(int outletId, string customerUserName);
    }
}
