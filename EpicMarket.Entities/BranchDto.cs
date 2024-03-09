using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{

    public class BranchDto
    {
        public int BussinessID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public long ContactNumber { get; set; }

        public string ContactEmail { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Pincode { get; set; }
    }

    public class BranchResult
    {
        public int ID { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public long ContactNumber { get; set; }

        public string ContactEmail { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Pincode { get; set; }
    }

    public class BranchParams
    {
        public int BusinessId { get; set; }
        public int PageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public bool ascending { get; set; }
        public string searchTerm { get; set; }

    }


    public class BranchPeopleMapParams
    {
        public int OutletId { get; set; }
        public List<int> AddPersonId { get; set; }
        public List<int> RemovedPersonId { get; set; }
    }
    public class BranchPeopleMapResult
    {
        public List<int> AddedBranchPeopleMapID { get; set; }

        public List<int> RemovedBranchPeopleMapID { get; set; }
    }


    public class BranchProductMapParams
    {
        public int OutletId { get; set; }
        public List<int> AddProductsId { get; set; }
        public List<int> RemovedProductsId { get; set; }
    }

    public class BranchProductMapResult
    {
        public List<int> AddedBranchProductMapID { get; set; }

        public List<int> RemovedBranchProductMapID { get; set; }
    }
}
