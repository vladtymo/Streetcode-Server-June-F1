using FluentValidation;

namespace Streetcode.BLL.MediatR.Timeline.TimelineItem.Create;

public class CreateTimelineItemRequestDTOValidator : AbstractValidator<CreateTimelineItemCommand>
{
    public CreateTimelineItemRequestDTOValidator()
    {
        RuleFor(x => x.newTimeLine).NotNull();
        RuleFor(x => x.newTimeLine.Title).MaximumLength(26);
        RuleFor(x => x.newTimeLine.Description).MaximumLength(400);
        RuleFor(x => x.newTimeLine.Date).NotNull();
        RuleFor(x => x.newTimeLine.DateViewPattern).NotNull();
        RuleFor(x => x.newTimeLine.StreetCodeId).GreaterThan(0).NotNull();
    }
}