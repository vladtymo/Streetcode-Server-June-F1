using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Delete;

public record DeleteCoordinateCommand(int Id) : IValidatableRequest<Result<Unit>>;