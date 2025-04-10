using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Business : BaseModel
    {

        [DisplayName("Business ID")]
        public int ID { get; set; }
        public int PersonID { get; set; }
		public int? StatusId { get; set; }
		public int BusinessCategoryID { get; set; }

        [DisplayName("Business Name")]
        public string Name { get; set; }

        [DisplayName("Business Description")]
        public string Description { get; set; }

        [DisplayName("Business Banner")]
        public string? Banner { get; set; }

        [DisplayName("Business Logo")]
        public string? Logo { get; set; }

        [DisplayName("Contact Number")]
        public long ContactNumber { get; set; }

        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }


        public int? AddressID { get; set; }
        public int? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public bool IsOpen { get; set; }
        public double? Weight { get; set; }
      

        // Navigation properties
        public virtual AppUser? Person { get; set; }
        public virtual BusinessCategoryInternal? BusinessCategory { get; set; }

         [JsonIgnore]
        public virtual Address? Address   { get; set; }

		[ForeignKey("StatusId")]
		public virtual StatusOptionSet? Status { get; set; }


        [JsonIgnore]
        public virtual ICollection<BusinessEmployeeMap> BusinessEmployees { get; set; }
        [JsonIgnore]
        public virtual ICollection<CatalogCategory> Categories { get; set; }
    }
}
