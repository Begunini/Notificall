using System;
using System.Threading.Tasks;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface ITelephonyHandler
    {
        Task<bool> MakeCall(string phone, string voiceUrl, string voiceName, string baseVoiceName);
    }
}