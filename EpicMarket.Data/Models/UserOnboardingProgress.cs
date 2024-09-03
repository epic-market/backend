using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class UserOnboardingProgress
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int StepID { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted { get; set; } = false;

        [ForeignKey("UserID")]
        public AppUser User { get; set; }

        [ForeignKey("StepID")]
        public OnboardingStep Step { get; set; }
    }
}
