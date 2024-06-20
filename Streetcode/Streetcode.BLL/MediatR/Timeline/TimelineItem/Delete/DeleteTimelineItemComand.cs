using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Delete;

public record DeleteTimelineItemCommand(int Id) : IValidatableRequest<Result<Unit>>;
