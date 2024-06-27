using MediatR;

namespace Streetcode.BLL.Services.Cache;

public interface  CachibleQueryPreProcessor<T> : IRequest<T>, CachibleQueryPreProcessor
{
   object? CachedResponse { get; set; }
}

public interface CachibleQueryPreProcessor
{
}
