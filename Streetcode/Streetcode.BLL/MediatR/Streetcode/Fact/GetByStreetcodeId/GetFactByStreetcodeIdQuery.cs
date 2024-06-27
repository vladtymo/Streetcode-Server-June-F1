using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;

public record GetFactByStreetcodeIdQuery(int StreetcodeId) : ICachibleQueryPreProcessor<Result<IEnumerable<FactDto>>>
{
    public object? CachedResponse { get; set; }
}