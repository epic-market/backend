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

        int AddOrUpdateBranch(BranchDto branchDto, string UserName, int BusinessID, string PageSource);

        Task<int> MapBranchToPeople(BranchPeopleMapParams branchPeopleMapParams);

        Task<int> MapBranchToProducts(BranchProductMapParams branchProductMap);

        Task<BranchResult> GetBranchByID(int branchId);
    }
}
