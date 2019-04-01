using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Enums;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class VoiceService : IVoiceService
    {
        public AudioFormat Format { get; }

        private readonly string _defaultLanguage;
        private readonly VoiceGenerator _voiceGenerator;
        
        public VoiceService(string yandexUrl, string yandexApiKey, string defaultLanguage, AudioFormat format, AudioQuality quality)
        {
            Format = format;
            _defaultLanguage = defaultLanguage;
            _voiceGenerator = new VoiceGenerator(yandexUrl, yandexApiKey, format, quality);
        }

        public async Task<KeyValuePair<string, byte[]>> GenerateAudio(string text, Language language, Speaker speaker, Emotion emotion, decimal speed)
        {
            var languageValue = LanguageResponder.DefineLanguage(_defaultLanguage, language);

            var file = await _voiceGenerator.Generate(text, languageValue, speaker, emotion, speed);

            return file;
        }
    }
}
