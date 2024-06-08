using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class EventLogService : IEventLogService
    {
        private readonly ApplicationDbContext _context;

        public EventLogService(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async Task<long> LogEvent(EVENT_LOG_SAVE_PARAMS eVENT_LOG_SAVE_PARAMS)
        {

            var entityModel = _context.Entity.Where(row => row.Name == eVENT_LOG_SAVE_PARAMS.EntityName.Trim()).FirstOrDefault();
            if (entityModel == null)
            {
                throw new Exception(string.Format("Unable to retrieve Entity by Name {0}", eVENT_LOG_SAVE_PARAMS.EntityName));
            }

            var eventModel = _context.Event.Where(row => row.Name == eVENT_LOG_SAVE_PARAMS.EventName.Trim()).FirstOrDefault();

            if (eventModel == null)
            {
                throw new Exception(string.Format("Unable to retrieve Event by Name {0}", eVENT_LOG_SAVE_PARAMS.EventName));
            }

            EventLog eventLogRecord;

            if (eVENT_LOG_SAVE_PARAMS.Description != null)
            {
                eventLogRecord = new EventLog
                {
                    EventID = eventModel.ID,
                    //SessionID = source.SessionID,
                    Source = eVENT_LOG_SAVE_PARAMS.Source,
                    Description = eVENT_LOG_SAVE_PARAMS.Description,
                    Data = eVENT_LOG_SAVE_PARAMS.Data,
                    NotificationQueueDate = null,
                    CreateDate = DateTime.Now,
                    CreateBy = eVENT_LOG_SAVE_PARAMS.LoggedInUserName
                };
            }
            else
            {

                eventLogRecord = new EventLog
                {
                    EventID = eventModel.ID,
                    //SessionID = source.SessionID,
                    Source = eVENT_LOG_SAVE_PARAMS.Source,
                    Description = eventModel.Description,
                    Data = eVENT_LOG_SAVE_PARAMS.Data,
                    NotificationQueueDate = null,
                    CreateDate = DateTime.Now,
                    CreateBy = eVENT_LOG_SAVE_PARAMS.LoggedInUserName
                };
            }
            _context.EventLog.Add(eventLogRecord);
            await _context.SaveChangesAsync();
            return eventLogRecord.ID;

        }
    }
}
