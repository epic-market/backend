using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EpicMarket.Data.Common;

namespace EpicMarket.Data.Models
{
    public class CommunicationStatus : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }

    // Static class for communication status constants
    public static class CommunicationStatusConstants
    {
        public const string Queued = "Queued";
        public const string Sent = "Sent";
        public const string Failed = "Failed";
        public const string Retrying = "Retrying";
        
    }
} 