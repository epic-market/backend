using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.ApplicationModels
{
    public class ApplicationConfiguration : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

    }
}
