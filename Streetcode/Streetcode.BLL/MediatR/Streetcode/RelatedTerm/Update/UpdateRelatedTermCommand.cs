using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Update
{
    public record UpdateRelatedTermCommand(int Id, RelatedTermDTO RelatedTerm) : IValidatableRequest<Result<RelatedTermDTO>>;
}
