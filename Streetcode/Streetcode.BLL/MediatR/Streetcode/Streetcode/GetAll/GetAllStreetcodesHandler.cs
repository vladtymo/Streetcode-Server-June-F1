using Ardalis.Specification;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Specification.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.Specification.Streetcode.Streetcode.GetByFilter;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public class GetAllStreetcodesHandler : IRequestHandler<GetAllStreetcodesQuery, Result<GetAllStreetcodesResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetAllStreetcodesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<GetAllStreetcodesResponseDTO>> Handle(GetAllStreetcodesQuery query, CancellationToken cancellationToken)
    {
        var filterRequest = query.request;
        var amount = filterRequest.Amount;
        var page = filterRequest.Page;

        List<ISpecification<StreetcodeContent>> specifications = new List<ISpecification<StreetcodeContent>>();

        if (amount <= 0 || page <= 0)
        {
            string errorMsg = ErrorMessages.InvalidPaginationParameters;
            _logger.LogError(filterRequest, errorMsg);
            return Result.Fail(errorMsg);
        }

        ApplyFilters(specifications, filterRequest);

        specifications.Add(new StreetcodeApplyPaginationSpec(amount, page));

        var filteredListStreetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllWithSpecAsync(specifications.ToArray());

        if (filteredListStreetcodes == null)
        {
            string errorMsg = ErrorMessages.StreetCodeIsNull;
            _logger.LogError(filterRequest, errorMsg);
            return Result.Fail(errorMsg);
        }

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(filteredListStreetcodes);

        var response = new GetAllStreetcodesResponseDTO
        {
            Pages = CalculateTotalPages(filteredListStreetcodes.Count(), amount),
            Streetcodes = streetcodeDtos
        };

        return Result.Ok(response);
    }

    private void ApplyFilters(
         List<ISpecification<StreetcodeContent>> specifications, GetAllStreetcodesRequestDTO filterRequest)
    {
        if (!string.IsNullOrEmpty(filterRequest.Title))
        {
            specifications.Add(new StreetcodesFindWithMatchTitleSpec(filterRequest.Title));
        }

        if (!string.IsNullOrEmpty(filterRequest.Sort))
        {
            specifications.Add(new StreetcodesSortedByPropertySpec(filterRequest.Sort));
        }

        if (!string.IsNullOrEmpty(filterRequest.Filter))
        {
            specifications.Add(new StreetcodesFilteredByStatusSpec(filterRequest.Filter));
        }
    }

    private int CalculateTotalPages(int totalItems, int itemsPerPage)
    {
        return (int)Math.Ceiling(totalItems / (double)itemsPerPage);
    }
}