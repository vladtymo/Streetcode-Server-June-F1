using FluentValidation;

namespace Streetcode.BLL.MediatR.Account.Delete
{
    public class DeleteUserDtoValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserDtoValidator()
        {
            RuleFor(d => d.Dto.Id).NotEmpty().WithMessage("User Id can't be empty for delete operation!");
            RuleFor(d => d.Dto.Id).Must((string? id) => Guid.TryParse(id, out Guid r)).WithMessage("Fail to parse UserId to Guid!");
        }
    }    
}
