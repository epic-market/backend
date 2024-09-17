using EpicMarket.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Models
{
    public class Quicklink : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public ICollection<OnboardingStep> OnboardingProgress { get; set; }

        public ICollection<Notification> Notifications { get; set; }

    }
}
