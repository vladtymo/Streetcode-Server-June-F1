using FluentValidation;

namespace Streetcode.BLL.MediatR.Account.ChangePassword
{
    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(u => u.PasswordChange.NewPassword).MinimumLength(7);
            RuleFor(u => u.PasswordChange.NewPassword).Matches("[A-Z]");
            RuleFor(u => u.PasswordChange.NewPassword).Matches("[a-z]");
            RuleFor(u => u.PasswordChange.NewPassword).Matches("[0-9]");
            RuleFor(u => u.PasswordChange.NewPassword).Matches("[^a-zA-Z0-9]");

            RuleFor(u => u.PasswordChange).Must(x => x.NewPassword == x.ConfirmPassword);
            RuleFor(u => u.PasswordChange).Must(x => x.NewPassword != x.CurrentPassword);

        }
    }
}
