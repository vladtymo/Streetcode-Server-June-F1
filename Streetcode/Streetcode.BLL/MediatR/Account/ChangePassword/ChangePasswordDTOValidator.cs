using FluentValidation;


namespace Streetcode.BLL.MediatR.Account.ChangePassword
{
    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(u => u.PasswordChange.NewPassword).MinimumLength(7);
            RuleFor(u => u.PasswordChange).Must(x => x.NewPassword == x.ConfirmPassword);
            RuleFor(u => u.PasswordChange).Must(x => x.NewPassword != x.CurrentPassword);
        }
    }
}
