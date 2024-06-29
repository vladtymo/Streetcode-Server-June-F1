using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Update;

public class UpdateTermHandler : IRequestHandler<UpdateTermCommand, Result<TermDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;

    public UpdateTermHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<TermDTO>> Handle(UpdateTermCommand request, CancellationToken cancellationToken)
    {
        var term = await _repository.TermRepository
            .GetFirstOrDefaultAsync(rt => rt.Id == request.Term.Id);

        if (term == null)
        {
            const string errorMsg = "Cannot get Term by term TermId";
            _logger.LogError(request, errorMsg);
            return new Error(errorMsg);
        }

        var termToUpdate = _mapper.Map<Entity>(request.Term);

        if (termToUpdate is null)
        {
            const string errorMsg = "Cannot map new term!";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var updatedTerm = _repository.TermRepository.Update(term);

        var isSuccessResult = await _repository.SaveChangesAsync() > 0;

        if (!isSuccessResult)
        {
            const string errorMsg = "Cannot save changes in the database after related word creation!";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var updatedTermDto = _mapper.Map<TermDTO>(updatedTerm.Entity);

        if (updatedTermDto != null)
        {
            return Result.Ok(updatedTermDto);
        }
        else
        {
            const string errorMsg = "Cannot map entity!";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}