using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface ISourceService
    {
        Task<List<Source>> GetSources();

        Task<List<string>> GetHeaders(Guid sourceId);

        Task<SourceContent> GetContent(Guid sourceId, int rows, int offset);

        Task<int?> GetPhoneColumn(Guid sourceId);

        Task SetPhoneColumn(Guid sourceId, int columnIndex);

        Task<bool> ValidateSourceNameNonDuplcate(string name);

        bool ValidateSourceFileExtension(IFormFile file);

        Task<Guid> UploadSource(string title, IFormFile file);
    }
}