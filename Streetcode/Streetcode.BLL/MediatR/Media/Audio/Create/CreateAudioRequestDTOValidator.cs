using FluentValidation;
namespace Streetcode.BLL.MediatR.Media.Audio.Create;

public class CreateAudioRequestDTOValidator : AbstractValidator<CreateAudioCommand>
{
    public CreateAudioRequestDTOValidator()
    {
        RuleFor(x => x.Audio.Description).MaximumLength(200).WithMessage("Description must not exceed 200 characters");
        RuleFor(x => x.Audio.Title).MaximumLength(100).WithMessage("Title must not exceed 100 characters");
        RuleFor(x => x.Audio.Extension).NotEmpty().WithMessage("Extension must not be empty");
        RuleFor(x => x.Audio.BaseFormat).NotEmpty().WithMessage("BaseFormat must not be empty");
        RuleFor(x => x.Audio.MimeType).NotEmpty().WithMessage("MimeType must not be empty");
    }
}