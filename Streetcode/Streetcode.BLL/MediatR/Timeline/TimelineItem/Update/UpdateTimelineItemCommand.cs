using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Update
{
    public record UpdateTimelineItemCommand(TimelineItemDTO updatedTimeLine) : IRequest<Result<TimelineItemDTO>>;
}
