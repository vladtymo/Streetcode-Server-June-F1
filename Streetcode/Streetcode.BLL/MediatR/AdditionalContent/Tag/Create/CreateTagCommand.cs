using FluentResults;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
  public record CreateTagCommand(CreateTagDTO tag) : IValidatableRequest<Result<TagDTO>>;
}
