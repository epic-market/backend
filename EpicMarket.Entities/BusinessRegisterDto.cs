using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpicMarket.Entities.Attributes;

namespace EpicMarket.Entities
{
    public class BusinessRegisterDto
    {
        [Required]
        public int BusinessCategoryID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string BusinessName { get; set; }

        [Required]
        public long ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [Url]
        public string WebsiteURL { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Address { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string State { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }

        [Required]
        [Range(10000, 99999)]
        public int PinCode { get; set; }

        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public DateTime EstablishedOn { get; set; }

        [Required]
        public int ProofTypeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ProofNumber { get; set; }

        [Required]
        [FileSize(5 * 1024 * 1024)] // 5MB limit
        public IFormFile LogoFile { get; set; }

        [Required]
        [FileSize(5 * 1024 * 1024)] // 5MB limit
        public IFormFile[] ProofFile { get; set; }
    }
    public class BusinessDTO_Result 
    {
        public int BusinessId { get; set; }

        public int ProofId { get; set; }
    }

    public class BusinessDetailResult
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
        public int AddressID { get; set; }
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
        public string Status { get; set; }

        public string Thumbnail { get; set; }
        public List<string> Proofs { get; set; }
    }
    public class UpdateBusinessRegisterDto
    {
        public string BusinessName { get; set; }

        public string Description { get; set; }

        public long ContactNumber { get; set; }

        public string ContactEmail { get; set; }

        public string Address { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int PinCode { get; set; }
        public string ProofType { get; set; }

        public string ProofNumber { get; set; }
        public string LogoFile { get; set; }
        public string[] ProofFile { get; set; }
    }
}
