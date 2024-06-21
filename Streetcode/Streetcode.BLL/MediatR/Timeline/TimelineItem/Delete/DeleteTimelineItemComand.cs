using FluentResults;
using MediatR;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Delete;

public record DeleteTimelineItemCommand(int Id) : IValidatableRequest<Result<Unit>>;
