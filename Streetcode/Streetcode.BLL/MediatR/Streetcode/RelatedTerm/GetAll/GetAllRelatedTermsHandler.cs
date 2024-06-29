using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.TextContent.RelatedTerm;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAll;

public record GetAllRelatedTermsHandler : IRequestHandler<GetAllRelatedTermsQuery, Result<IEnumerable<RelatedTermDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;

    public GetAllRelatedTermsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repository = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<RelatedTermDTO>>> Handle(GetAllRelatedTermsQuery request, CancellationToken cancellationToken)
    {
        var relatedTerms = await _repository.RelatedTermRepository
            .GetAllAsync(include: rt => rt.Include(rt => rt.Term));

        var relatedTermsDto = _mapper.Map<IEnumerable<RelatedTermDTO>>(relatedTerms);

        if (relatedTermsDto is null)
        {
            const string errorMsg = "Cannot map DTOs for related words!";
            _logger.LogError(request, errorMsg);
            return new Error(errorMsg);
        }

        return Result.Ok(relatedTermsDto);
    }
}