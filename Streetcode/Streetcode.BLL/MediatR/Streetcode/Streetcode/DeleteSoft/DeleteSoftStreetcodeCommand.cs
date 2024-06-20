using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;

public record DeleteSoftStreetcodeCommand(int Id) : IValidatableRequest<Result<Unit>>;
