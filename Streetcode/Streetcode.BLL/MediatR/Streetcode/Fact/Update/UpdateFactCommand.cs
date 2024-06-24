using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Streetcode.Facts.Update
{
    public record UpdateFactCommand(FactUpdateCreateDto Fact) : IValidatableRequest<Result<FactDto>>;
}
