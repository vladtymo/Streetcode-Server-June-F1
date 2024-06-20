using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Create;

public record CreateCoordinateCommand(StreetcodeCoordinateDTO StreetcodeCoordinate) : IValidatableRequest<Result<Unit>>;