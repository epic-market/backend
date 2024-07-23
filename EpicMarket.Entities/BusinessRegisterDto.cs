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

        public string LogoURL { get; set; }

        public string Description { get; set; }

        public DateTime EstablishedOn { get; set; }

        public string ProofType { get; set; }

        public string ProofNumber { get; set; }

        public string proofURL { get; set; }

        public IFormFile File { get; set; }
        public string FileName { get; set; }
    }
}
