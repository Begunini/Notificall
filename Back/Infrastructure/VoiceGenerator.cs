using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Enums;
using Flurl;
using Flurl.Http;
using Infrastructure.Enums;

namespace Infrastructure
{
    public class VoiceGenerator
    {
        private readonly string _url;
        private readonly string _apiKey;
        private readonly string _format;
        private readonly string _quality;

        public VoiceGenerator(string url, string apiKey, AudioFormat format, AudioQuality quality)
        {
            _url = url;
            _apiKey = apiKey;
            _format = Enum.GetName(typeof(AudioFormat), format);
            _quality = Enum.GetName(typeof(AudioQuality), quality);
        }

        public async Task<KeyValuePair<string, byte[]>> Generate(string text, string lang, Speaker speaker, Emotion emotion, decimal speed, string fileName = "voice")
        {
            var speakerName = Enum.GetName(typeof(Speaker), speaker);
            var emotionName = Enum.GetName(typeof(Emotion), emotion);

            var fileBytes = await _url.SetQueryParams(new
            {
                key = _apiKey,
                text,
                format = _format,
                quality = _quality,
                lang,
                speaker = speakerName,
                speed,
                emotion = emotionName
            }).GetBytesAsync();

            return new KeyValuePair<string, byte[]>($"{fileName}.{_format}", fileBytes);
        }
    }
}
