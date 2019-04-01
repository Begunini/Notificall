using API.Dto;
using AutoMapper;
using Database.Entities;

namespace API
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Source, SourceDto>();

            //CreateMap<Combination, CombinationDto>();

            //CreateMap<Voice, VoiceDto>();

            CreateMap<Text, TextDto>();
            CreateMap<TextSaveDto, Text>();

            CreateMap<Event, EventDto>()
                .ForMember(x => x.Source, o => o.MapFrom(x => x.Source.Title))
                .ForMember(x => x.Text, o => o.MapFrom(x => x.Text.Title));

            CreateMap<Call, CallDto>();

            //CreateMap<Background, BackgroundDto>();
        }
    }
}
