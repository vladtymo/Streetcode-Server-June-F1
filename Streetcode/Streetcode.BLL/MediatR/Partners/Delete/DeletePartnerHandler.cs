using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Cache;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Delete
{
    public class DeletePartnerHandler : IRequestHandler<DeletePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public DeletePartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result<PartnerDTO>> Handle(DeletePartnerQuery request, CancellationToken cancellationToken)
        {
            var partner = await _repositoryWrapper.PartnersRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (partner == null)
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityWithIdNotFound, request, request.id);
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            _repositoryWrapper.PartnersRepository.Delete(partner);
            try
            {
                _repositoryWrapper.SaveChanges();
                await _cacheService.InvalidateCacheAsync("Partner");
                return Result.Ok(_mapper.Map<PartnerDTO>(partner));
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
