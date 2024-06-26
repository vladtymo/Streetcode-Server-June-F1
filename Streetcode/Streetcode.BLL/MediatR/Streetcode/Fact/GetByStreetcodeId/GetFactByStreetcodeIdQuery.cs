using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;

public record GetFactByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<FactDto>>>, ICachibleQueryPreProcessor<Result<IEnumerable<FactDto>>>
{
    public object? CachedResponse { get; set; }
}