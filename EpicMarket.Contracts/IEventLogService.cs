using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IEventLogService
    {
		Task<long> LogEvent(EVENT_LOG_SAVE_PARAMS eVENT_LOG_SAVE_PARAMS);
    }
}
