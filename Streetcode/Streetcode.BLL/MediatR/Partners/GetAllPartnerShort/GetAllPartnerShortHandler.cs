using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Cache;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort
{
    internal class GetAllPartnerShortHandler : IRequestHandler<GetAllPartnersShortQuery, Result<IEnumerable<PartnerShortDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public GetAllPartnerShortHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result<IEnumerable<PartnerShortDTO>>> Handle(GetAllPartnersShortQuery request, CancellationToken cancellationToken)
        {
            string key = $"PartnerShort";
            string? cachedValue = await _cacheService.GetCacheAsync(key);

            if (!string.IsNullOrEmpty(cachedValue))
            {
                IEnumerable<PartnerShortDTO?> partnerShort = JsonConvert.DeserializeObject<IEnumerable<PartnerShortDTO>>(cachedValue) !;

                if (partnerShort != null)
                {
                    return Result.Ok(partnerShort) !;
                }
            }

            var partners = await _repositoryWrapper.PartnersRepository.GetAllAsync();

            if (partners is null)
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityNotFound, request);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            await _cacheService.SetCacheAsync(key, _mapper.Map<IEnumerable<PartnerShortDTO>>(partners));

            return Result.Ok(_mapper.Map<IEnumerable<PartnerShortDTO>>(partners));
        }
    }
}
