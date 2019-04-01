using System.IO;
using System.Threading.Tasks;
using EventProcessor.Interfaces;
using Microsoft.Azure.WebJobs;

namespace EventProcessor
{
    public class Functions
    {
        private readonly IEventHandler _eventHandler;

        public Functions(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task ProcessNewEvents([TimerTrigger("*/15 * * * * *", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            await _eventHandler.ProcessNew();
        }

        public async Task ProcessInProgressEvents([TimerTrigger("*/15 * * * * *", RunOnStartup = true)] TimerInfo timer, TextWriter log)
        {
            await _eventHandler.ProcessInProgress();
        }
    }
}
