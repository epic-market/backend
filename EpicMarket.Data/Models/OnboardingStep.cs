using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public  class OnboardingStep
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StepName { get; set; }
        public string StepDescription { get; set; }

        public int PageId { get; set; }
        [Required]
        public int StepOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Page Page { get; set; }
        public ICollection<UserOnboardingProgress> OnboardingProgress { get; set; }
    }
}
