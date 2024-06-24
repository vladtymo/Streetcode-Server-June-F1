using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Cache;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<IEnumerable<PartnerDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly ICacheService _cacheService;

    public GetAllPartnersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, ICacheService cacheService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        string key = $"PartnerAll";
        string? cachedValue = await _cacheService.GetCacheAsync(key);

        if (!string.IsNullOrEmpty(cachedValue))
        {
            IEnumerable<PartnerDTO>? partnersCache = JsonConvert.DeserializeObject<IEnumerable<PartnerDTO>>(
                cachedValue);
            
            if (partnersCache != null)
            {
                return Result.Ok(_mapper.Map<IEnumerable<PartnerDTO>>(partnersCache));
            }
        }

        var partners = await _repositoryWrapper
            .PartnersRepository
            .GetAllAsync(
                include: p => p
                    .Include(pl => pl.PartnerSourceLinks)
                    .Include(p => p.Streetcodes));

        if (partners is null)
        {
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityNotFound, request);
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        await _cacheService.SetCacheAsync(key, _mapper.Map<IEnumerable<PartnerDTO>>(partners));

        return Result.Ok(_mapper.Map<IEnumerable<PartnerDTO>>(partners));
    }
}
