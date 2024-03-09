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
        Task<List<BranchResult>> GetAllBranches(BranchParams branchParams);

        Task<int> AddBranch(BranchDto branchDto, string UserName);

        Task<int> MapBranchToPeople(BranchPeopleMapParams branchPeopleMapParams);

        Task<int> MapBranchToProducts(BranchProductMapParams branchProductMap);
    }
}
