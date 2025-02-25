using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;

namespace EpicMarket.Admin.MVC.Services
{
    public class EventService: IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> LogEvent(EVENT_LOG_SAVE_PARAMS eventLogParams)
        {
            var eventLogRecord = new EventLog();
            var entityModel = await _context.Entity.Where(row => row.Name == eventLogParams.EntityName.Trim()).FirstOrDefaultAsync();
            if (entityModel == null)
            {
                throw new Exception(string.Format("Unable to retrieve Entity by Name {0}", eventLogParams.EntityName));
            }

            var eventModel = await _context.Event.Where(row => row.Name == eventLogParams.EventName.Trim()).FirstOrDefaultAsync();

            if (eventModel == null)
            {
                throw new Exception(string.Format("Unable to retrieve Event by Name {0}", eventLogParams.EventName));
            }

            if (eventLogParams.Description != null)
            {
                eventLogRecord = new EventLog
                {
                    EventID = eventModel.ID,
                    EntityID = entityModel.ID,
                    RecordID = eventLogParams.RecordId,
                    Source = eventLogParams.Source,
                    Description = eventLogParams.Description,
                    Data = eventLogParams.Data,
                    CreateDate = DateTime.Now,
                    CreateBy = eventLogParams.LoggedInUserName
                };
            }
            else
            {
                eventLogRecord = new EventLog
                {
                    EventID = eventModel.ID,
                    EntityID = entityModel.ID,
                    RecordID = eventLogParams.RecordId,
                    Source = eventLogParams.Source,
                    Description = eventModel.Description,
                    Data = eventLogParams.Data,
                    CreateDate = DateTime.Now,
                    CreateBy = eventLogParams.LoggedInUserName
                };
            }

            await _context.EventLog.AddAsync(eventLogRecord);
            await _context.SaveChangesAsync();
            return eventLogRecord.ID;
        }
    }
}