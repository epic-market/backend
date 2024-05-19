using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IBranchService
    {
        Task<List<BranchResult>> GetAllBranches(BranchParams branchParams, int BusinessID);

        Task<int> AddOrUpdateBranch(BranchDto branchDto, string UserName, int BusinessID);

        Task<int> MapBranchToPeople(BranchPeopleMapParams branchPeopleMapParams);

        Task<int> MapBranchToProducts(BranchProductMapParams branchProductMap);

        Task<BranchResult> GetBranchByID(int branchId);
    }
}
