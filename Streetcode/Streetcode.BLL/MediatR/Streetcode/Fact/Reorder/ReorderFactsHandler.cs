using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Reorder;

public class ReorderFactsHandler : IRequestHandler<ReorderFactsCommand, Result<IEnumerable<FactDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    public ReorderFactsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<FactDto>>> Handle(ReorderFactsCommand request, CancellationToken cancellationToken)
    {
        var facts = await _repositoryWrapper.FactRepository.GetAllAsync(f => f.StreetcodeId == request.streetcodeId);
        var listNewPosition = request.facts;

        if (listNewPosition is null || !listNewPosition.Any())
        {
            const string errorMsg = $"Updated list of position cannot be empty or null";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        if (facts is null || !facts.Any())
        {
            string errorMsg = $"Cannot find any fact by streetcodeId {request.streetcodeId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var reorderPositionResult = UpdateFactsPositions(request, facts, listNewPosition);
        var factsWithNewPosition = reorderPositionResult.Value;

        if (!factsWithNewPosition.Any())
        {
            string errorMsg = $"List of fact cannot be empty";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.FactRepository.UpdateRange(factsWithNewPosition);
        var result = await _repositoryWrapper.SaveChangesAsync() > 0;
        if (!result)
        {
            const string errorMsg = "Failed to save changes to the database.";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<IEnumerable<FactDto>>(factsWithNewPosition));
    }

    private Result<IEnumerable<DAL.Entities.Streetcode.TextContent.Fact>> UpdateFactsPositions(ReorderFactsCommand request, IEnumerable<DAL.Entities.Streetcode.TextContent.Fact> facts, IEnumerable<FactUpdatePositionDto> factsWithNewPosition)
    {
        foreach (var factDto in factsWithNewPosition)
        {
            var item = facts.FirstOrDefault(f => f.Id == factDto.Id);
            var oldPosition = item?.Position ?? 0;
            var newPosition = factDto.NewPosition;

            if (item is null)
            {
                string errorMsg = $"Fact with id {factDto.Id}  not on the list";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var itemsToShift = newPosition < oldPosition
                ? facts.Where(i => i.Position >= newPosition && i.Position < oldPosition)
                : facts.Where(i => i.Position > oldPosition && i.Position <= newPosition);

            foreach (var it in itemsToShift)
            {
                it.Position += newPosition < oldPosition ? 1 : -1;
            }

            item.Position = newPosition;

            item.Position = newPosition;
        }

        return Result.Ok(facts);
    }
}