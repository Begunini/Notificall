using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;

namespace API.Interfaces
{
    public interface ITextService
    {
        Task<List<Text>> GetTexts(Guid sourceId);

        Task<Text> GetText(Guid textId);

        Task<Guid> SaveText(Text text);
    }
}