using AutoMapper;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Timeline.Create;
using Streetcode.DAL.Entities.Timeline;

namespace Streetcode.BLL.Mapping.Timeline;

public class TimelineItemProfile : Profile
{
    public TimelineItemProfile()
    {
        CreateMap<TimelineItem, TimelineItemDTO>()
            .ForMember(dest => dest.HistoricalContexts, opt => opt.MapFrom(x => x.HistoricalContextTimelines
                .Select(hct => new HistoricalContextDTO
                {
                    Id = hct.HistoricalContextId,
                    Title = hct.HistoricalContext!.Title
                }).ToList()))
            .ReverseMap();

        CreateMap<TimelineItem, CreateTimelineItemDTO>()
            .ForMember(dest => dest.HistoricalContexts, opt => opt.MapFrom(x => x.HistoricalContextTimelines
                .Select(hct => new HistoricalContextDTO
                {
                    Id = hct.HistoricalContextId,
                    Title = hct.HistoricalContext!.Title
                }).ToList()))
            .ReverseMap();
    }
}