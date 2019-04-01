using System;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities;

namespace Infrastructure.Interfaces
{
    public interface ICallManager
    {
        IQueryable<Call> GetCalls();

        Task<Call> GetCall(Guid callId);

        Task<Call> AddCall(Call newCall);

        Task UpdateCall(Call updatedCall);
    }
}
