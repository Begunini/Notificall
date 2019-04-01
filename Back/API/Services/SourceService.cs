using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Database.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Services
{
    public class SourceService : ISourceService
    {
        private readonly ISourceManager _sourceManager;
        private readonly IFileDataExporter _fileDataExporter;

        public SourceService(ISourceManager sourceManager, IFileDataExporter fileDataExporter)
        {
            _sourceManager = sourceManager;
            _fileDataExporter = fileDataExporter;
        }

        public async Task<List<Source>> GetSources()
        {
            return await _sourceManager.GetSources().ToListAsync();
        }

        public async Task<List<string>> GetHeaders(Guid sourceId)
        {
            return await _sourceManager.GetHeaders(sourceId);
        }

        public async Task<SourceContent> GetContent(Guid sourceId, int rows, int offset)
        {
            return await _sourceManager.GetContent(sourceId, rows, offset);
        }

        public async Task<int?> GetPhoneColumn(Guid sourceId)
        {
            return await _sourceManager.GetPhoneColumn(sourceId);
        }

        public async Task SetPhoneColumn(Guid sourceId, int columnIndex)
        {
            await _sourceManager.SetPhoneColumn(sourceId, columnIndex);
        }

        public async Task<bool> ValidateSourceNameNonDuplcate(string name)
        {
            return await _sourceManager.SourceExists(name);
        }

        public bool ValidateSourceFileExtension(IFormFile file)
        {
            var extensions = new[] { ".txt", ".csv", ".xls", ".xlsx" };
            var extension = Path.GetExtension(file.FileName.ToLower());

            return extensions.Any(x => x.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Guid> UploadSource(string title, IFormFile file)
        {
            var data = await _fileDataExporter.GetData(file);

            var source = new Source
            {
                Title = title,
                Value = JsonConvert.SerializeObject(data),
                Created = DateTime.Now,
                Edited = DateTime.Now
            };

            return await _sourceManager.SaveSource(title, source);
        }
    }
}
