using FluentValidation;
using Streetcode.BLL.Interfaces.Users;

namespace Streetcode.BLL.MediatR.Account.Register
{
    public class RegisterUserRequestDTOValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserRequestDTOValidator()
        {
            RuleFor(u => u.newUser.Email).EmailAddress();
            RuleFor(u => u.newUser.Password).MinimumLength(7);
            RuleFor(u => u.newUser.Password).Matches("[A-Z]");
            RuleFor(u => u.newUser.Password).Matches("[a-z]");
            RuleFor(u => u.newUser.Password).Matches("[0-9]");
            RuleFor(u => u.newUser.Password).Matches("[^a-zA-Z0-9]");
            RuleFor(u => u.newUser.Username).NotEmpty().Length(4, 256);

            // Add more if needed
        }
    }
}
