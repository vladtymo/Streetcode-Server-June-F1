using FluentResults;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Media.Image.Create;

public record CreateImageCommand(ImageFileBaseCreateDTO Image) : IValidatableRequest<Result<ImageDTO>>;
