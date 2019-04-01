using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class SourceManager : ISourceManager
    {
        private readonly NotificallDbContext _dbContext;

        public SourceManager(NotificallDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Source> GetSources()
        {
            return _dbContext.Sources;
        }

        public async Task<List<string>> GetHeaders(Guid sourceId)
        {
            var value = (await _dbContext.Sources.SingleOrDefaultAsync(x => x.Id == sourceId)).Value;
            var source = JsonConvert.DeserializeObject<List<KeyValuePair<string, List<string>>>>(value);

            return source.SingleOrDefault(x => x.Key == "headers").Value;
        }

        public async Task<bool> SourceExists(string name)
        {
            return await _dbContext.Sources.AnyAsync(x => x.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<SourceContent> GetContent(Guid sourceId, int rows, int offset)
        {
            var value = (await _dbContext.Sources.SingleOrDefaultAsync(x => x.Id == sourceId)).Value;
            var source = JsonConvert.DeserializeObject<List<KeyValuePair<string, List<string>>>>(value);
            var sourceItems = source.Where(x => x.Key == "item").Select(x => x.Value).ToList();
            var selectedItems = sourceItems.Skip(offset).Take(rows).ToList();

            return new SourceContent()
            {
                Total = sourceItems.Count,
                Items = selectedItems
            };
        }

        public async Task<int?> GetPhoneColumn(Guid sourceId)
        {
            return (await _dbContext.Sources.SingleOrDefaultAsync(x => x.Id == sourceId)).PhoneColumn;
        }

        public async Task SetPhoneColumn(Guid sourceId, int columnIndex)
        {
            var source = await _dbContext.Sources.SingleOrDefaultAsync(x => x.Id == sourceId);
            source.PhoneColumn = columnIndex;
            _dbContext.Update(source);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Guid> SaveSource(string title, Source source)
        {
            var sourceEntity = _dbContext.Sources.Add(source).Entity;

            await _dbContext.SaveChangesAsync();

            return sourceEntity.Id;
        }
    }
}
