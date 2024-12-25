using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{

    public class BranchDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public long ContactNumber { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        [Required]
        public int Pincode { get; set; }

        public IFormFile[] Photos { get; set; }
        public IFormFile Thumbnail { get; set; }
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

        public int Count { get; set; }
        public bool? IsOpen {  get; set; }

        public int AddressID { get; set; }
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
        public string Status { get; set; }

        public string Thumbnail { get; set; }
    }


    public class BranchDetailResult
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

        public int Count { get; set; }

        public int AddressID { get; set; }
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
        public string Status { get; set; }

        public string Thumbnail { get; set; }
        public List<string> Photos { get; set; }
    }


    public class BranchParams
    {
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public bool ascending { get; set; } = true;
        public string searchTerm { get; set; } = string.Empty;

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
    public class VerifyDto
    {
        public List<int> ListOfBranchIDs { get; set; }

    }

    public class VerifyCatalogDto
    {
        public List<int> ListOfCatalogIDs { get; set; }

    }
    public class QuickActionsParams
    {
        public int? ProductId { get; set; }
        public bool? IsRecommended { get; set; }
        public bool? InStock { get; set; }

    }

    public class ActivityParams
    {
        public int RecordId { get; set; }
        public string Entity { get; set; }
        public int PageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;

    }
    public class ActivityResult
    {
        public long ID { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string EventName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class UpdateBrancheStatusParams
    {
        public int? BranchId { get; set; }
        public bool Is_Open { get; set; } = true;
    }



    public class SubscribedOutletDto
    {
        public int? OutletId { get; set; }

        public string? OutletName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Thumbnail { get; set; }

        public DateTime? SubscribedDate { get; set; }
    }
}
