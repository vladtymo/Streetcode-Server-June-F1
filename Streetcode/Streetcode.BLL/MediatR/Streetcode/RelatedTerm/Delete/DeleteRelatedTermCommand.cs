using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public record DeleteRelatedTermCommand(string word) : IValidatableRequest<Result<RelatedTermDTO>>;
}
