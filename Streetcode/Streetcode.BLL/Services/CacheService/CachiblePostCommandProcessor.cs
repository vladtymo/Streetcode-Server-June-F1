using System.Text;
using FluentResults;
using MediatR.Pipeline;

namespace Streetcode.BLL.Services.CacheService;

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
        if (response != null && response.ToString() !.Contains("IsSuccess='False'"))
        {
           return;
        }

        var resultString = SplitCamelCase(request.ToResult().ValueOrDefault.ToString() !);
        var sharedKeyForEntity = EraseLastCharIfThatEndLetterS(resultString);
        if (!await _cacheService.CacheKeyPatternExist(sharedKeyForEntity))
        {
            return;  
        }
        
        await _cacheService.InvalidateCacheAsync(sharedKeyForEntity);
    }

    private static string EraseLastCharIfThatEndLetterS(string[] input)
    {
            var entity = input[1];
            if (entity.EndsWith("s"))
            {
                entity = entity.Substring(0, entity.Length - 1);
                var sharedKeyForEntity = entity;
                return sharedKeyForEntity;
            }

            return input[1];
    }
    
    private static string[] SplitCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new[] { input };
        }

        StringBuilder result = new StringBuilder();
        foreach (char c in input)
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result.Append(' ');
            }

            result.Append(c);
        }

        return result.ToString().Split(' ');
    }
}
