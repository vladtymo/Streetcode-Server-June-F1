using FluentValidation;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Reorder;

public class ReorderFactsRequestDTOValidator : AbstractValidator<ReorderFactsCommand>
{
    public ReorderFactsRequestDTOValidator()
    {
        RuleFor(x => x.newPositionsOfFacts).NotNull();
        RuleFor(x => x.streetcodeId).GreaterThan(0);
    }
}