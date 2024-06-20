using FluentValidation;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.DeleteContentCategory;

public class DeleteContentCategoryRequestDTOValidator : AbstractValidator<DeleteContentCategoryCommand>
{
    public DeleteContentCategoryRequestDTOValidator()
    {
        RuleFor(x => x.sourcelinkcatId).GreaterThan(0).NotNull();
        RuleFor(x => x.streetcodeId).GreaterThan(0).NotNull();
    }
}