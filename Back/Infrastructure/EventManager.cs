using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class EventManager : IEventManager
    {
        private readonly NotificallDbContext _dbContext;

        public EventManager(NotificallDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Event> GetEvents()
        {
            return _dbContext.Events
                .Include(x => x.Source)
                .Include(x => x.Text)
                .Include(x => x.Calls);
        }

        public async Task CreateEvent(Event newEvent)
        {
            _dbContext.Events.Add(newEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEvent(Event updatedEvent)
        {
            _dbContext.Events.Update(updatedEvent);
            await _dbContext.SaveChangesAsync();
        }
    }
}
