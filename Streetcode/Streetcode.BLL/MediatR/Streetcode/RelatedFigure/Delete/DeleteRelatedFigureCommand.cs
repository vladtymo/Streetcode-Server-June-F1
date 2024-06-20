using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public record DeleteRelatedFigureCommand(int ObserverId, int TargetId) : IValidatableRequest<Result<Unit>>;
