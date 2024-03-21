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
        AddEmployeeResult Register(AddEmployeeParam addEmployeeParam);

    }
}
