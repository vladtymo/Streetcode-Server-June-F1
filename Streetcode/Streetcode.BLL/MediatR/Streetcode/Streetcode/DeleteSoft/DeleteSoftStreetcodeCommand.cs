using FluentResults;
using MediatR;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;

public record DeleteSoftStreetcodeCommand(int Id) : IValidatableRequest<Result<Unit>>;
