using FluentValidation;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;

public class CreateSourceLinkCategoryRequestDTOValidator : AbstractValidator<CreateSourceLinkCategoryCommand>
{
    public CreateSourceLinkCategoryRequestDTOValidator()
    {
        RuleFor(x => x.newCategory).NotNull();
        RuleFor(x => x.newCategory.Title).NotNull().MaximumLength(100);
        RuleFor(x => x.newCategory.ImageId).GreaterThan(0).NotNull();
    }
}