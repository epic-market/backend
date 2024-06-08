using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{
    public class GetDataResult<T>
    {
        public T items { get; set; }

        public int Count { get; set; }
    }
}
