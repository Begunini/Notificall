using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace API.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetEvents();

        Task CreateEvent(Guid sourceID, Guid textId);
    }
}