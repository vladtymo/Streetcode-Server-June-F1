using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public record CreateRelatedFigureCommand(int ObserverId, int TargetId) : IValidatableRequest<Result<Unit>>;
