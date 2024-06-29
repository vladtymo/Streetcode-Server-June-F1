using FluentResults;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create;

public record CreateTermCommand(TermDTO Term) : IValidatableRequest<Result<TermDTO>>;