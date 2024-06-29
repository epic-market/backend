using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class DatabaseVersion
    {
        public int Id { get; set; }

        public string VersionClass { get; set; }

        public bool Status { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

    }
}
