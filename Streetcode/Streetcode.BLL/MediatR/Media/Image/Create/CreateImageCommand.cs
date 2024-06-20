using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Media.Image.Create;

public record CreateImageCommand(ImageFileBaseCreateDTO Image) : IValidatableRequest<Result<ImageDTO>>;
