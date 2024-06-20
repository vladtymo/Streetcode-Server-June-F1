using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Email;
public record SendEmailCommand(EmailDTO Email) : IValidatableRequest<Result<Unit>>;
