using FluentValidation;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;

namespace Streetcode.BLL.MediatR.AdditionalContent.StreetcodeCoordinate.Create;

public class CreateTagRequestDTOValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagRequestDTOValidator()
    {
        RuleFor(dto => dto.tag.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(50)
            .WithMessage("Title must not exceed 50 characters");
    }
}