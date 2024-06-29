using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;

public record CreateRelatedTermCommand(RelatedTermDTO RelatedTerm) : IValidatableRequest<Result<RelatedTermDTO>>;