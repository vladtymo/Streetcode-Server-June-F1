using FluentResults;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.UpdateContent
{
    public record CategoryContentUpdateCommand(CategoryContentUpdateDTO updatedContent) : IValidatableRequest<Result<StreetcodeCategoryContentDTO>>;
}
