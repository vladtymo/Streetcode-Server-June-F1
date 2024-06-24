using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Cache;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetById;

public class GetPartnerByIdHandler : IRequestHandler<GetPartnerByIdQuery, Result<PartnerDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly ICacheService _cacheService;

    public GetPartnerByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, ICacheService cacheService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Result<PartnerDTO>> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
    {
        string key = $"Partner{request.Id}";
        await _cacheService.GetCacheAsync(key);
        string? cachedValue = await _cacheService.GetCacheAsync(key);

        if (!string.IsNullOrEmpty(cachedValue))
        {
            var partnersCache = JsonConvert.DeserializeObject<PartnerDTO>(
                cachedValue);
            
            if (partnersCache != null)
            {
                return Result.Ok(_mapper.Map<PartnerDTO>(partnersCache));
            }
        }

        var partner = await _repositoryWrapper
            .PartnersRepository
            .GetSingleOrDefaultAsync(
                predicate: p => p.Id == request.Id,
                include: p => p
                    .Include(pl => pl.PartnerSourceLinks));
        if (partner is null)
        {
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityWithIdNotFound, request, request.Id);
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
        
        await _cacheService.SetCacheAsync(key, partner);
        return Result.Ok(_mapper.Map<PartnerDTO>(partner));
    }
}