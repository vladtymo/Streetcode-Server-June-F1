using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline.Create;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Create;

public record CreateTimelineItemCommand(CreateTimelineItemDTO newTimeLine) : IValidatableRequest<Result<CreateTimelineItemDTO>>;