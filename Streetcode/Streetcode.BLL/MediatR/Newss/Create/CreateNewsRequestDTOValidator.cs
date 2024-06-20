using FluentValidation;

namespace Streetcode.BLL.MediatR.Newss.Create;

public class CreateNewsRequestDTOValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsRequestDTOValidator()
    {
        RuleFor(x => x.newNews.ImageId).GreaterThan(0);
        RuleFor(x => x.newNews.Title).NotEmpty();
    }
}