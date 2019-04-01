using System;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TextManager : ITextManager
    {
        private readonly NotificallDbContext _dbContext;

        public TextManager(NotificallDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Text> GetTexts(Guid sourceId)
        {
            return _dbContext.Texts;
        }

        public async Task<Text> GetText(Guid textId)
        {
            return await _dbContext.Texts.SingleOrDefaultAsync(x => x.Id == textId);
        }

        public async Task<Guid> SaveText(Text text)
        {
            var textEntity = _dbContext.Texts.Add(text).Entity;
            await _dbContext.SaveChangesAsync();

            return textEntity.Id;
        }
    }
}
