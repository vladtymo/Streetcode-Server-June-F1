using FluentResults;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Streetcode.BLL.Services.Cache
{
    public class CachiblePreQueryProcessor<TRequest, T> : IPipelineBehavior<TRequest, T>
        where TRequest : ICachibleQueryPreProcessor<T>
    {
        private readonly ICacheService _cacheService;

        public CachiblePreQueryProcessor(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<T> Handle(TRequest request, RequestHandlerDelegate<T> next, CancellationToken cancellationToken)
        {
            string key = request.ToResult().Value.ToString() !;
            string? cachedValue = await _cacheService.GetCacheAsync(key);

            if (!string.IsNullOrEmpty(cachedValue))
            { 
                request.CachedResponse = JsonConvert.DeserializeObject(cachedValue) !;
                return await next();
            }
            
            return await next();
        }
    }
}
