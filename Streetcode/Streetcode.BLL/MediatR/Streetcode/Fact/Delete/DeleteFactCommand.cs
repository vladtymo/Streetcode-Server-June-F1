using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Delete
{
    public record DeleteFactCommand(int Id) : IValidatableRequest<Result<FactDto>>;
}
