using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TextService : ITextService
    {
        private readonly ITextManager _textManager;

        public TextService(ITextManager textManager)
        {
            _textManager = textManager;
        }

        public async Task<List<Text>> GetTexts(Guid sourceId)
        {
            return await _textManager.GetTexts(sourceId).Where(x => x.SourceId == sourceId).ToListAsync();
        }

        public async Task<Text> GetText(Guid textId)
        {
            return await _textManager.GetText(textId);
        }

        public async Task<Guid> SaveText(Text text)
        {
            text.Created = DateTime.Now;
            text.Edited = DateTime.Now;

            return await _textManager.SaveText(text);
        }
    }
}
