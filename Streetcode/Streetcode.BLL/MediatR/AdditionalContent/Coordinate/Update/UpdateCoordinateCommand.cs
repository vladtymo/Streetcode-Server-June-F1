using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Update;

public record UpdateCoordinateCommand(StreetcodeCoordinateDTO StreetcodeCoordinate) : IValidatableRequest<Result<Unit>>;