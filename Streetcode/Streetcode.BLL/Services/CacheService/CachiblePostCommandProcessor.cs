using FluentResults;
using MediatR;
using MediatR.Pipeline;
namespace Streetcode.BLL.Services.Cache;

public class CachiblePostCommandProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
     where TRequest : ICachibleCommandPostProcessor<TResponse>
{
    private readonly ICacheService _cacheService;
    public CachiblePostCommandProcessor(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        if (response.ToResult().IsFailed)
        {
            return;
        }

        var sharedKeyForEntity = request.ToResult().Value.GetType().ToString().Split('.')[3].Substring(0, request.ToResult().Value.GetType().ToString().Split('.')[3].Length - 1);
        await _cacheService.InvalidateCacheAsync(sharedKeyForEntity);
    }
}
