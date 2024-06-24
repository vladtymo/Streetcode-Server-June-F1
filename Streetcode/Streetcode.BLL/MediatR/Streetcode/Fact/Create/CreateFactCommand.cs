using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public record CreateFactCommand(FactDto NewFact) : IValidatableRequest<Result<FactDto>>;
