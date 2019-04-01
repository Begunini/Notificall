using System;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Database.Enums;
using Infrastructure;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace API.Controllers
{
    [Route("api/preview")]
    public class PreviewController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly ITextService _textService;
        private readonly VoiceService _voiceService;
        private readonly TemplateToTextConverter _textConverter;

        public PreviewController(IConfiguration config, ISourceService sourceService, ITextService textService)
        {
            _sourceService = sourceService;
            _textService = textService;

            var yandexUrl = config["NotificallSettings:YandexUrl"];
            var yandexApiKey = config["NotificallSettings:YandexApiKey"];
            var defaultLanguage = config["NotificallSettings:DefaultLanguage"];

            _voiceService = new VoiceService(yandexUrl, yandexApiKey, defaultLanguage, AudioFormat.mp3, AudioQuality.lo);
            _textConverter = new TemplateToTextConverter();
        }

        [HttpGet]
        [Route("text")]
        public async Task<IActionResult> GetPreview(Guid sourceId, string text, Language language, Speaker speaker, Emotion emotion, decimal speed, int offset = 0)
        {
            var headers = await _sourceService.GetHeaders(sourceId);
            var content = await _sourceService.GetContent(sourceId, 1, offset);

            var finalText = _textConverter.GenerateResultingText(headers, content.Items.FirstOrDefault(), text);

            var voiceFile = await _voiceService.GenerateAudio(finalText, language, speaker, emotion, speed);

            return File(voiceFile.Value, "audio/mpeg", voiceFile.Key);
        }

        [HttpGet]
        [Route("saved-text")]
        public async Task<IActionResult> GetPreview(Guid sourceId, Guid textId, int offset = 0)
        {
            var text = await _textService.GetText(textId);

            var headers = await _sourceService.GetHeaders(sourceId);
            var content = await _sourceService.GetContent(sourceId, 1, offset);

            var finalText = _textConverter.GenerateResultingText(headers, content.Items.FirstOrDefault(), text.Value);

            var voiceFile = await _voiceService.GenerateAudio(finalText, text.Language, text.Speaker, text.Emotion, text.Speed);

            return File(voiceFile.Value, "audio/mpeg", voiceFile.Key);
        }
    }
}