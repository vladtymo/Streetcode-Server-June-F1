using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Media.Audio.Delete;

public record DeleteAudioCommand(int Id) : IValidatableRequest<Result<Unit>>;
