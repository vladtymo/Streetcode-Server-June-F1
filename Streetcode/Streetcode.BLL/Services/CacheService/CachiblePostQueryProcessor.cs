using FluentResults;
using MediatR.Pipeline;
using Newtonsoft.Json.Linq;

namespace Streetcode.BLL.Services.CacheService;

public class CachiblePostQueryProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : ICachibleQueryPreProcessor<TResponse>
{
    private readonly ICacheService _cacheService;
    public CachiblePostQueryProcessor(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        var cacheKey = request.ToResult().ValueOrDefault?.ToString();
        if (cacheKey == null)
        {
            return;
        }

        if (await _cacheService.CacheKeyIsExist(cacheKey))
        {
            return;
        }

        if (response != null && response.ToString()!.Contains("IsSuccess='True'"))
        {
            var valueToken = JObject.FromObject(response)["Value"];
            if (valueToken == null)
            {
                return;
            }

            var value = valueToken.ToString();

            await _cacheService.SetCacheAsync(cacheKey, value);
        }
    }
}