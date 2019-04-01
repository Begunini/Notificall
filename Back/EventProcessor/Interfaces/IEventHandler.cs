using System.Threading.Tasks;

namespace EventProcessor.Interfaces
{
    public interface IEventHandler
    {
        Task ProcessNew();

        Task ProcessInProgress();
    }
}