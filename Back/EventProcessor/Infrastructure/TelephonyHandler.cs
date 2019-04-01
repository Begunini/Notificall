using System;
using System.Threading.Tasks;
using AsterNET.Manager;
using AsterNET.Manager.Action;
using Infrastructure.Interfaces;

namespace EventProcessor.Infrastructure
{
    public class TelephonyHandler : ITelephonyHandler
    {
        private readonly ManagerConnection _managerConnection;

        public TelephonyHandler()
        {
            _managerConnection = new ManagerConnection("192.168.26.128", 5038, "notificall", "notificall");
            _managerConnection.Login();
        }

        public async Task<bool> MakeCall(string phone, string voiceUrl, string voiceName, string baseVoiceName)
        {
            

            var action = new OriginateAction
            {
                Async = true,
                Channel = $"Dongle/dongle0/{phone}",
                Exten = "s",
                Context = "outgoing",
                Variable = $"SOUND_URL={voiceUrl},SOUND_NAME={voiceName},BASE_SOUND_NAME={baseVoiceName}"
            };

            var response = _managerConnection.SendAction(action);

            return response.IsSuccess();
        }
    }
}
