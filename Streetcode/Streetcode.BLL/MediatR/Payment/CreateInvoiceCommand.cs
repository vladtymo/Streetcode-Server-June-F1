using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.Behavior;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.MediatR.Payment;

public record CreateInvoiceCommand(PaymentDTO Payment) : IValidatableRequest<Result<InvoiceInfo>>;
