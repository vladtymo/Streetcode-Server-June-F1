using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Media.Image.Delete;

public record DeleteImageCommand(int Id) : IValidatableRequest<Result<Unit>>;
