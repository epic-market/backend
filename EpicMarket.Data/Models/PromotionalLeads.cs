using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class PromotionalLeads
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Gmail { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public string WhichApplication { get; set; }
    }
}
