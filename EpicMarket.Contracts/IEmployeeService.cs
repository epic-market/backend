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
        Task<AddEmployeeResult> Register(AddEmployeeParam addEmployeeParam, int businessid, int userID);
        CheckLinkResult CheckEmployeeLink(string queryParam);

        Task<int> CreateEmployeeAccount(EmployeeDto employee);

        Task<List<EmployeeMapOptionResult>> GetAllEmployeesForMap(int businessid, int Outletid);

        Task<List<EmployeeResult>> GetAllEmployees(EmployeeParams employeeParams, int businessid);

        Task<SingleEmployeeResult> GetEmployeeDetails(int employeeId);
    }
}
