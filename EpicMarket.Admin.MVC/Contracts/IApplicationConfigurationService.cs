using EpicMarket.Data.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IApplicationConfigurationService
    {

        ApplicationConfiguration GetByID(int? id);


        ApplicationConfiguration GetByName(string name);


        string GetApplicationConfigurationValue(string name);



    }
}
