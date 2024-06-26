using FluentResults;
using MediatR.Pipeline;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Streetcode.BLL.Services.Cache;
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
        var serializeObject = JsonConvert.SerializeObject(response);
        var jsonObject = JObject.Parse(serializeObject);

        var valueToken = jsonObject["Value"];
        await _cacheService.SetCacheAsync(request.ToResult().ValueOrDefault.ToString() !, valueToken!);
    }
}