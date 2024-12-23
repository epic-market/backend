using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EpicMarket.Data.Models
{
  public class ProofType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // Name of the proof type (e.g., "License", "Certificate")
    }
}