using FluentValidation;

namespace Streetcode.BLL.MediatR.Streetcode.Facts.Update;

public class UpdateFactRequestDTOValidator : AbstractValidator<UpdateFactCommand>
{
    public UpdateFactRequestDTOValidator()
    {
        RuleFor(x => x.Fact).NotNull();
        RuleFor(x => x.Fact.Title).MaximumLength(100).NotNull();
        RuleFor(x => x.Fact.FactContent).NotNull().MaximumLength(1500);
        RuleFor(x => x.Fact.StreetcodeId).NotNull().GreaterThan(0);
        RuleFor(x => x.Fact.ImageId).GreaterThan(0);
        RuleFor(x => x.Fact.ImageDescription).MaximumLength(200).NotNull();
    }
}