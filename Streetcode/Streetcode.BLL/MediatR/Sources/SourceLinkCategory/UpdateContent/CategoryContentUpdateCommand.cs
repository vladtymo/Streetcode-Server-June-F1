using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.UpdateContent
{
    public record CategoryContentUpdateCommand(CategoryContentUpdateDTO updatedContent) : IValidatableRequest<Result<StreetcodeCategoryContentDTO>>;
}
