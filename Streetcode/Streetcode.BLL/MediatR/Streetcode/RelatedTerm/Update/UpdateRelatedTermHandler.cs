using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.RelatedTerm;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Update;

public class UpdateRelatedTermHandler : IRequestHandler<UpdateRelatedTermCommand, Result<RelatedTermDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;

    public UpdateRelatedTermHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<RelatedTermDTO>> Handle(UpdateRelatedTermCommand request, CancellationToken cancellationToken)
    {
        var relatedTerm = await _repository.RelatedTermRepository
            .GetFirstOrDefaultAsync(rt => rt.Id == request.RelatedTerm.Id);

        if (relatedTerm == null)
        {
            const string errorMsg = "Cannot get RelatedTerm by term TermId";
            _logger.LogError(request, errorMsg);
            return new Error(errorMsg);
        }

        var relatedTermToUpdate = _mapper.Map<Entity>(request.RelatedTerm);

        if (relatedTermToUpdate is null)
        {
            const string errorMsg = "Cannot map new related word for a term!";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var updatedRelatedTerm = _repository.RelatedTermRepository.Update(relatedTermToUpdate);

        var isSuccessResult = await _repository.SaveChangesAsync() > 0;

        if (!isSuccessResult)
        {
            const string errorMsg = "Cannot save changes in the database after related word creation!";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var updatedRelatedTermDto = _mapper.Map<RelatedTermDTO>(updatedRelatedTerm.Entity);

        if (updatedRelatedTermDto != null)
        {
            return Result.Ok(updatedRelatedTermDto);
        }
        else
        {
            const string errorMsg = "Cannot map entity!";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}