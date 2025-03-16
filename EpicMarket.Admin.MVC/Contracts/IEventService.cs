using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicMarket.Entities;

namespace EpicMarket.Admin.MVC.Contracts
{
    public interface IEventService
    {
        Task<long> LogEvent(EVENT_LOG_SAVE_PARAMS eventLogParams);
    }
}