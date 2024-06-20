using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Update
{
    public record UpdateRelatedTermCommand(int id, RelatedTermDTO RelatedTerm) : IValidatableRequest<Result<Unit>>
    {
    }
}
