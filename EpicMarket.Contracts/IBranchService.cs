using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IBranchService
    {
        Task<GetDataResult<List<BranchResult>>> GetAllBranches(BranchParams branchParams, int BusinessID);

        Task<int> AddBranch(BranchDto branchDto, string UserName, int BusinessID, string PageSource);

        Task<int> UpdateBranch(int id, BranchDto branchDto, string UserName, int BusinessID, string PageSource);

        Task<int> MapBranchToPeople(BranchPeopleMapParams branchPeopleMapParams);

        Task<int> MapBranchToProducts(BranchProductMapParams branchProductMap);

        Task<BranchDetailResult> GetBranchByID(int branchId);
        Task<int> VerifyBranchs(VerifyDto verifyBranchDto, string UserName, int AdminPersonID, string PageSource);

        Task DeleteBranch(int branchId, string UserName);
    }
}
