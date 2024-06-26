using MediatR;

namespace Streetcode.BLL.Services.Cache;

public interface ICachibleQueryPreProcessor<T> : IRequest<T>, ICachibleQueryPreProcessor
{
   object? CachedResponse { get; set; }
}

public interface ICachibleQueryPreProcessor
{
}