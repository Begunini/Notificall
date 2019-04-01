using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Enums;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface IVoiceService
    {
        AudioFormat Format { get; }

        Task<KeyValuePair<string, byte[]>> GenerateAudio(string text, Language language, Speaker speaker,
            Emotion emotion, decimal speed);
    }
}