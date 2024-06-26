using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Streetcode.Facts.Update
{
    public record UpdateFactCommand(FactUpdateCreateDto Fact) : IValidatableRequest<Result<FactDto>>, ICachibleCommandPostProcessor<Result<FactDto>>;
}
