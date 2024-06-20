using FluentValidation;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.UpdateContent;

public class CategoryContentUpdateRequestDTOValidator : AbstractValidator<CategoryContentUpdateCommand>
{
    public CategoryContentUpdateRequestDTOValidator()
    {
        RuleFor(x => x.updatedContent).NotNull();
        RuleFor(x => x.updatedContent.Text).MaximumLength(4000).NotNull();
        RuleFor(x => x.updatedContent.SourceLinkCategoryId).NotNull().GreaterThan(0);
    }
}