using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
   public class Proof
    {
        [Key]
        public int Id { get; set; }
        
        public string EntityType { get; set; } // "Business" or "Person"
        
        public int EntityId { get; set; } // ID of the business or person

        public string ProofNumber { get; set; }
        [Required]
        public int ProofTypeId { get; set; } // Foreign key to ProofType

        public ProofType ProofType { get; set; } // Navigation property
    }
}