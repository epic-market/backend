using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class OnboardingService : IOnboardingService
    {


        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public OnboardingService(
                                ApplicationDbContext context,
                                IMapper mapper,
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                ICommunicationQueueService communicationQueueService,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }


        public async Task  CompleteStepForUserID(int UserID, int StepID)
        {
          var step =  _context.UserOnboardingProgresses.Where(c=>c.StepID == StepID && c.UserID == UserID).FirstOrDefault();
            if (step != null)
            { 
                step.CompletedAt = DateTime.Now;
                step.IsCompleted = true;
                _context.UserOnboardingProgresses.Update(step);
                await unitOfWork.Complete();
            }
        }

        public async Task<List<OnboardingStepResult>> GetAllOnboardingSteps(int UserID)
        {
            return 
                await _context.OnboardingSteps
                .Include(os => os.OnboardingProgress)
                .Where(os => !os.OnboardingProgress.Any(up => up.UserID == UserID && up.IsCompleted)).Include(c=>c.Page).Select(c=> new OnboardingStepResult() { 
                StepName = c.StepName,
                StepDescription = c.StepDescription,
                StepOrder = c.StepOrder,
                NavigationURL = c.Page.Url
                }).ToListAsync();
        }
    }
}
