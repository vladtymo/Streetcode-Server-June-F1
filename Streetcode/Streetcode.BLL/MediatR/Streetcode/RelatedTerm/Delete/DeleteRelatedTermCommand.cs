using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public record DeleteRelatedTermCommand(string word) : IValidatableRequest<Result<RelatedTermDTO>>;
}
