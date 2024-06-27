using FluentResults;
using MediatR.Pipeline;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Streetcode.BLL.Services.Cache;

public class CachiblePostQueryProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : ICachibleQueryPreProcessor<TResponse>
{
    private readonly ICacheService _cacheService;
    public CachiblePostQueryProcessor(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        if (await _cacheService.CacheKeyIsExist(request.ToResult().ValueOrDefault!.ToString() !))
        {
            return;
        }

        var serializeObject = JsonConvert.SerializeObject(response);
        var jsonObject = JObject.Parse(serializeObject);

        var valueToken = jsonObject["Value"];
        _cacheService.SetCacheAsync(request.ToResult().ValueOrDefault.ToString() !, valueToken!);
    }
}