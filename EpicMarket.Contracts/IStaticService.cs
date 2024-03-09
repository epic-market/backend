using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IStaticService
    {
        Task<List<DropDownOptions>> BusinessCategoriesOptions();
    }
}
