using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public record CreateFactCommand(FactDto NewFact) : IValidatableRequest<Result<FactDto>>, ICachibleCommandPostProcessor<Result<FactDto>>;
