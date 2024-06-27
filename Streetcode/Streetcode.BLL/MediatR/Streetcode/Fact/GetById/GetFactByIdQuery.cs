using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById;

public record GetFactByIdQuery(int Id) : ICachibleQueryPreProcessor<Result<FactDto>>
{
    public object? CachedResponse { get; set; }
}
