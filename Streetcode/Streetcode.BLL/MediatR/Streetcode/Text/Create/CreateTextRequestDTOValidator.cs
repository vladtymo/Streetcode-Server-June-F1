using FluentValidation;

namespace Streetcode.BLL.MediatR.Streetcode.Text.Create;

public class CreateTextRequestDTOValidator : AbstractValidator<CreateTextCommand>
{
    public CreateTextRequestDTOValidator()
    {
        RuleFor(x => x.TextCreate).NotNull();
        RuleFor(x => x.TextCreate.Title).MaximumLength(50).NotNull();
        RuleFor(x => x.TextCreate.TextContent).NotNull().MaximumLength(15000);
        RuleFor(x => x.TextCreate.AdditionalText).MaximumLength(500).NotNull();
        RuleFor(x => x.TextCreate.VideoUrl).MaximumLength(500);
        RuleFor(x => x.TextCreate.Author).MaximumLength(200);
        RuleFor(x => x.TextCreate.StreetcodeId).NotNull().GreaterThan(0);
    }
}
