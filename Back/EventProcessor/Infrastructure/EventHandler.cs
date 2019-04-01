using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Database.Entities;
using Database.Enums;
using EventProcessor.Interfaces;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventProcessor.Infrastructure
{
    public class EventHandler : IEventHandler
    {
        private readonly IEventManager _eventManager;
        private readonly ISourceManager _sourceManager;
        private readonly ITextManager _textManager;
        private readonly ICallManager _callManager;
        private readonly IVoiceService _voiceService;
        private readonly IBlobManager _blobManager;
        private readonly ITelephonyHandler _telephonyHandler;
        private readonly TemplateToTextConverter _textConverter;
        private readonly AudioFileConverter _audioConverter;

        public EventHandler(IEventManager eventManager, ISourceManager sourceManager, ITextManager textManager, 
            ICallManager callManager, IVoiceService voiceService, IBlobManager blobManager, ITelephonyHandler telephonyHandler)
        {
            _eventManager = eventManager;
            _sourceManager = sourceManager;
            _textManager = textManager;
            _callManager = callManager;
            _voiceService = voiceService;
            _blobManager = blobManager;
            _telephonyHandler = telephonyHandler;
            _textConverter = new TemplateToTextConverter();
            _audioConverter = new AudioFileConverter();
        }

        public async Task ProcessNew()
        {
            var events = await _eventManager.GetEvents()
                                    .Where(x => x.Status == EventStatus.Pending).ToListAsync();

            foreach (var newEvent in events)
            {
                newEvent.Status = EventStatus.InProgress;
                await _eventManager.UpdateEvent(newEvent);

                var text = await _textManager.GetText(newEvent.TextId);

                var phoneColumnResult = await _sourceManager.GetPhoneColumn(newEvent.SourceId);
                var phoneColumn = 0;
                if (phoneColumnResult != null)
                {
                    phoneColumn = (int)phoneColumnResult;
                }

                var headers = await _sourceManager.GetHeaders(newEvent.SourceId);
                var content = await _sourceManager.GetContent(newEvent.SourceId, int.MaxValue, 0);

                foreach (var item in content.Items)
                {
                    var phoneNumber = item[phoneColumn];

                    var finalText = _textConverter.GenerateResultingText(headers, item, text.Value);

                    var voiceFile = await _voiceService.GenerateAudio(finalText, text.Language, text.Speaker, text.Emotion, text.Speed);

                    //var callAudioFile = _audioConverter.Convert(voiceFile.Value);

                    var call = new Call
                    {
                        EventId = newEvent.Id,
                        MessageText = finalText,
                        Phone = phoneNumber,
                        Result = CallResult.Pending
                    };

                    var newCall = await _callManager.AddCall(call);

                    var formatName = Enum.GetName(typeof(AudioFormat), _voiceService.Format);
                    var voiceFileNameBase = $"{newEvent.Id.ToString()}_{newCall.Id.ToString()}";
                    var voiceFileName = $"{voiceFileNameBase}.{formatName}";

                    //change to callAudioFile
                    var voiceUrl = await _blobManager.UploadFile(voiceFile.Value, voiceFileName, _voiceService.Format);

                    var callResultSuccess = await _telephonyHandler.MakeCall(phoneNumber, voiceUrl, voiceFileName, voiceFileNameBase);

                    newCall.Result = callResultSuccess ? CallResult.Success : CallResult.Failed;

                    // TODO add migration with removed Required header
                    newCall.Created = DateTime.Now;

                    await _callManager.UpdateCall(newCall);

                    // TODO FIX THIS SHIT
                    Thread.Sleep(30000);
                }
            }
        }

        public async Task ProcessInProgress()
        {
            var events = await _eventManager.GetEvents().Where(x => x.Status == EventStatus.InProgress).ToListAsync();

            foreach (var existingEvent in events)
            {
                var hasNonProcessedCalls = await _callManager.GetCalls()
                                                    .Where(x => x.EventId == existingEvent.Id)
                                                    .AnyAsync(x => x.Result == CallResult.Pending);

                if (!hasNonProcessedCalls)
                {
                    existingEvent.Status = EventStatus.Finished;
                    await _eventManager.UpdateEvent(existingEvent);
                }
            }
        }
    }
}
