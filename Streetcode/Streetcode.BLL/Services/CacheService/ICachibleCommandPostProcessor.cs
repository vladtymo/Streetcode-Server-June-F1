using MediatR;

namespace Streetcode.BLL.Services.Cache;

public interface ICachibleCommandPostProcessor<out TResponse> : IRequest<TResponse>, ICachibleCommandPostProcessor
{
}

public interface ICachibleCommandPostProcessor
{
}