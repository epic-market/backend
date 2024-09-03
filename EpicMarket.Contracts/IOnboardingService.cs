using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IOnboardingService
    {
        Task<List<OnboardingStepResult>> GetAllOnboardingSteps(int UserID);

       Task CompleteStepForUserID(int UserID,int StepID);

    }
}
