using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById;

public class GetFactByIdHandler : IRequestHandler<GetFactByIdQuery, Result<FactDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    // Bad Solution Just for testing
    private readonly IDistributedCache _distributedCache;

    public GetFactByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IDistributedCache cache)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _distributedCache = cache;  
    }

    public async Task<Result<FactDto>> Handle(GetFactByIdQuery request, CancellationToken cancellationToken)
    {
        string key = $"FactDTO-{request.Id}";

        string? cachedValue = await _distributedCache.GetStringAsync(key);

        FactDto? factDto = null;

        // Cache was found
        if (!string.IsNullOrEmpty(cachedValue))
        {
            factDto = JsonConvert.DeserializeObject<FactDto>(cachedValue);

            if (factDto != null)
            {
                return Result.Ok(factDto);
            }            
        }

        var facts = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (facts is null)
        {
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityWithIdNotFound, request, request.Id);
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        factDto = _mapper.Map<FactDto>(facts);

        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(factDto));

        return Result.Ok(factDto);
    }
}