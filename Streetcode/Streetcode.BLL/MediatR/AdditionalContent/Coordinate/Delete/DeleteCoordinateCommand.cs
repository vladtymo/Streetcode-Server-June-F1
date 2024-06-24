using FluentResults;
using MediatR;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Delete;

public record DeleteCoordinateCommand(int Id) : IValidatableRequest<Result<Unit>>;