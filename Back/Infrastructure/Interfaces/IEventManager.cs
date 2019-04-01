using System.Linq;
using System.Threading.Tasks;
using Database.Entities;

namespace Infrastructure.Interfaces
{
    public interface IEventManager
    {
        IQueryable<Event> GetEvents();

        Task CreateEvent(Event newEvent);

        Task UpdateEvent(Event updatedEvent);
    }
}
