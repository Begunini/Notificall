using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Entities;
using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface ISourceManager
    {
        IQueryable<Source> GetSources();

        Task<bool> SourceExists(string name);

        Task<List<string>> GetHeaders(Guid sourceId);

        Task<SourceContent> GetContent(Guid sourceId, int rows, int offset);

        Task<int?> GetPhoneColumn(Guid sourceId);

        Task SetPhoneColumn(Guid sourceId, int columnIndex);

        Task<Guid> SaveSource(string title, Source source);
    }
}