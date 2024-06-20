using FluentResults;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.DeleteContentCategory
{
    public record DeleteContentCategoryCommand(int sourcelinkcatId, int streetcodeId) : IValidatableRequest<Result<StreetcodeCategoryContentDTO>>;
}
