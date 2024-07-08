using FluentValidation;

namespace Streetcode.BLL.MediatR.Email.SendEmail
{
    public class SendVerificationEmailDTOValidator : AbstractValidator<SendVerificationEmailCommand>
    {
        public SendVerificationEmailDTOValidator()
        {
            RuleFor(u => u.email).EmailAddress();
        }
    }
}
