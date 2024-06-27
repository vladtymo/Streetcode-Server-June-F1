namespace Streetcode.BLL.Services.Cache;

public interface ICacheService
{   
     Task InvalidateCacheAsync(string pattern);
     Task SetCacheAsync(string key, string value, TimeSpan? expiry = null);
     Task SetCacheAsync(string key, object value, TimeSpan? expiry = null);
     Task<bool> CacheKeyIsExist(string key);
     Task<bool> CacheKeyPatternExist(string pattern);

     Task<string?> GetCacheAsync(string key);

     Task DeleteCacheAsync(string key);
}
