using FluentResults;
using MediatR;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public record DeleteRelatedFigureCommand(int ObserverId, int TargetId) : IValidatableRequest<Result<Unit>>;
