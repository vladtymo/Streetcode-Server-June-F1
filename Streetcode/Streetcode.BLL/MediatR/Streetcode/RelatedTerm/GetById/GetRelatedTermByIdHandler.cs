using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetById;

public record GetRelatedTermByIdHandler : IRequestHandler<GetRelatedTermByIdQuery, Result<RelatedTermDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;

    public GetRelatedTermByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repository = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<RelatedTermDTO>> Handle(GetRelatedTermByIdQuery request, CancellationToken cancellationToken)
    {
        var relatedTerm = await _repository.RelatedTermRepository
            .GetFirstOrDefaultAsync(
                predicate: rt => rt.Id == request.Id,
                include: rt => rt.Include(rt => rt.Term));

        if (relatedTerm == null)
        {
            const string errorMsg = "Cannot get RelatedTerm by term TermId";
            _logger.LogError(request, errorMsg);
            return new Error(errorMsg);
        }

        var relatedTermDto = _mapper.Map<RelatedTermDTO>(relatedTerm);

        if (relatedTermDto is null)
        {
            const string errorMsg = "Cannot map DTO for related term!";
            _logger.LogError(request, errorMsg);
            return new Error(errorMsg);
        }

        return Result.Ok(relatedTermDto);
    }
}