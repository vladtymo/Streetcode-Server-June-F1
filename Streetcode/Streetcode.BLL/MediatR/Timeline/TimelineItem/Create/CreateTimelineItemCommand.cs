using FluentResults;
using Streetcode.BLL.DTO.Timeline.Create;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Create;

public record CreateTimelineItemCommand(CreateTimelineItemDTO newTimeLine) : IValidatableRequest<Result<CreateTimelineItemDTO>>;