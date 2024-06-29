using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetById;

public class GetTermByIdHandler : IRequestHandler<GetTermByIdQuery, Result<TermWithRelatedTermsDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetTermByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TermWithRelatedTermsDTO>> Handle(GetTermByIdQuery request, CancellationToken cancellationToken)
    {
        var term = await _repositoryWrapper.TermRepository
            .GetFirstOrDefaultAsync(
                predicate: f => f.Id == request.Id,
                include: t => t.Include(t => t.RelatedTerms));

        if (term is null)
        {
            var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityWithIdNotFound, request, request.Id);
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<TermWithRelatedTermsDTO>(term));
    }
}