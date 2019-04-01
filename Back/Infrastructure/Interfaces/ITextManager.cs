using System;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities;

namespace Infrastructure.Interfaces
{
    public interface ITextManager
    {
        IQueryable<Text> GetTexts(Guid sourceId);

        Task<Text> GetText(Guid textId);

        Task<Guid> SaveText(Text text);
    }
}