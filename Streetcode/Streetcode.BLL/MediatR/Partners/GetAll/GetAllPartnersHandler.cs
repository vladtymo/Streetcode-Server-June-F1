using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<IEnumerable<PartnerDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetAllPartnersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        if (request.CachedResponse is not null)
        {
            var cachedPartnerDtos = JsonConvert.DeserializeObject<IEnumerable<PartnerDTO>>(request.CachedResponse.ToString() !);
            return Result.Ok(_mapper.Map<IEnumerable<PartnerDTO>>(cachedPartnerDtos));
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

        return Result.Ok(_mapper.Map<IEnumerable<PartnerDTO>>(partners));
    }
}
