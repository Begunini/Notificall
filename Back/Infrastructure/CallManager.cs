using System;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class CallManager : ICallManager
    {
        private readonly NotificallDbContext _dbContext;

        public CallManager(NotificallDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Call> GetCalls()
        {
            return _dbContext.Calls;
        }

        public async Task<Call> GetCall(Guid callId)
        {
            return await _dbContext.Calls.SingleOrDefaultAsync(x => x.Id == callId);
        }

        public async Task<Call> AddCall(Call newCall)
        {
            var call = _dbContext.Add(newCall).Entity;
            await _dbContext.SaveChangesAsync();
            return call;
        }

        public async Task UpdateCall(Call updatedCall)
        {
            _dbContext.Update(updatedCall);
            await _dbContext.SaveChangesAsync();
        }
    }
}
