using EpicMarket.Data.ApplicationModels;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IEmployeeService
    {
        Task<AddEmployeeResult> Register(AddEmployeeParam addEmployeeParam);
        CheckLinkResult CheckEmployeeLink(string queryParam);

        Task<int> CreateEmployeeAccount(EmployeeDto employee);
    }
}
