using MediatR;

namespace Streetcode.BLL.Services.CacheService;

public interface ICachibleQueryPreProcessor<TResult> : IRequest<TResult>
{
    TResult? CachedResponse { get; set; }
}
