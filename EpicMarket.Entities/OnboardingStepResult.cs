using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class OnboardingStepResult
    {
        public string StepName { get; set; }
        public string StepDescription { get; set; }
        public string NavigationURL { get; set; }
        public int StepOrder { get; set; }
    }
}
