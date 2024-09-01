using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class BusinessRegisterDto
    {
        public int BusinessCategoryID { get; set; }

        public string BussinessName { get; set; }

        public long ContactNumber { get; set; }

        public string ContactEmail { get; set; }

        public string WebsiteURL { get; set; }

        public string Address { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int PinCode { get; set; }

        public string Description { get; set; }

        public DateTime EstablishedOn { get; set; }

        public string ProofType { get; set; }

        public string ProofNumber { get; set; }

        public IFormFile LogoFile { get; set; }
        public IFormFile ProofFile { get; set; }
    }
    public class BusinessDTO_Result 
        {
        public int BusinessId { get; set; }
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

}
