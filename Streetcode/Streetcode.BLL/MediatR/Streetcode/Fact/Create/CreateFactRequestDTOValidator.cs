using FluentValidation;
namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public class CreateFactRequestDTOValidator : AbstractValidator<CreateFactCommand>
{
    public CreateFactRequestDTOValidator()
    { 
        RuleFor(x => x.newFact).NotEmpty();
        RuleFor(x => x.newFact.Title).NotEmpty();
        RuleFor(x => x.newFact.FactContent).NotEmpty();
        RuleFor(x => x.newFact.StreetcodeId).GreaterThan(0);
        RuleFor(x => x.newFact.ImageId).GreaterThan(0);
    }
}