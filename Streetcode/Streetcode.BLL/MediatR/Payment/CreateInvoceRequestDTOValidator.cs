using FluentValidation;

namespace Streetcode.BLL.MediatR.Payment;

public class CreateInvoiceRequestDTOValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceRequestDTOValidator()
    {
        RuleFor(x => x.Payment.Amount).GreaterThan(0).NotEmpty();
        RuleFor(x => x.Payment.RedirectUrl).NotEmpty();
    }
}