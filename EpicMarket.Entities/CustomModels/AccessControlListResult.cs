using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{
    internal class AccessControlListResult
    {
    }
    public class AccessControlList_Result 
    {
        public int ID { get; set; }
        public string SecurableName { get; set; }
        public string AccessType { get; set; }
    }
}
