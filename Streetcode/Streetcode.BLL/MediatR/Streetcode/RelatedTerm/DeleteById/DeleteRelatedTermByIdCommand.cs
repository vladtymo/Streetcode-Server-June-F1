using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.DeleteById;

public record DeleteRelatedTermByIdCommand(int Id) : IValidatableRequest<Result<RelatedTermDTO>>;