using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Media.Audio.Create;

public record CreateAudioCommand(AudioFileBaseCreateDTO Audio) : IValidatableRequest<Result<AudioDTO>>;
