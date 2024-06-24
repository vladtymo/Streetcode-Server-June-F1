using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Specification.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.Specification.Streetcode.Streetcode.GetByFilter;
using Streetcode.BLL.Specification.Streetcode.Streetcode.NewFolder;
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
        var streetcodes = _repositoryWrapper.StreetcodeRepository.FindAll();
        var amount = filterRequest.Amount;
        var page = filterRequest.Page;

        if (streetcodes == null)
        {
            string errorMsg = string.Format(ErrorMessages.EntityNotFound, streetcodes);
            _logger.LogError(filterRequest, errorMsg);
            return Result.Fail(errorMsg);
        }

        if (amount <= 0 || page <= 0)
        {
            string errorMsg = ErrorMessages.InvalidPaginationParameters;
            _logger.LogError(filterRequest, errorMsg);
            return Result.Fail(errorMsg);
        }

        ApplyFilters(ref streetcodes, filterRequest);

        int pagesAmount = ApplyPagination(ref streetcodes, amount, page);
        var filteredListStreetcodes = await streetcodes.ToListAsync();

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(filteredListStreetcodes);

        var response = new GetAllStreetcodesResponseDTO
        {
            Pages = pagesAmount,
            Streetcodes = streetcodeDtos
        };

        return Result.Ok(response);
    }

    private void ApplyFilters(
        ref IQueryable<StreetcodeContent> streetcodes, GetAllStreetcodesRequestDTO filterRequest)
    {
        if (!string.IsNullOrEmpty(filterRequest.Title))
        {
            streetcodes = streetcodes.ApplySpecification(new StreetcodesFindWithMatchTitleSpec(filterRequest.Title));
        }

        if (!string.IsNullOrEmpty(filterRequest.Sort))
        {
            streetcodes = streetcodes.ApplySpecification(new StreetcodesSortedByPropertySpec(filterRequest.Sort));
        }

        if (!string.IsNullOrEmpty(filterRequest.Filter))
        {
            streetcodes = streetcodes.ApplySpecification(new StreetcodesFilteredByStatusSpec(filterRequest.Filter));
        }
    }

    private int ApplyPagination(
        ref IQueryable<StreetcodeContent> streetcodes,
        int amount,
        int page)
    {
        var totalPages = (int)Math.Ceiling(streetcodes.Count() / (double)amount);

        streetcodes = streetcodes.ApplySpecification(new StreetcodeApplyPaginationSpec(amount, page));
        return totalPages;
    }
}