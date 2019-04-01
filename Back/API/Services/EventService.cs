using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using Database.Entities;
using Database.Enums;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class EventService : IEventService
    {
        private readonly IEventManager _eventManager;

        public EventService(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public async Task<List<Event>> GetEvents()
        {
            return await _eventManager.GetEvents().ToListAsync();
        }

        public async Task CreateEvent(Guid sourceID, Guid textId)
        {
            var newEvent = new Event
            {
                SourceId = sourceID,
                TextId = textId,
                Status = EventStatus.Pending,
                Created = DateTime.Now,
                Edited = DateTime.Now
            };

            await _eventManager.CreateEvent(newEvent);
        }
    }
}
