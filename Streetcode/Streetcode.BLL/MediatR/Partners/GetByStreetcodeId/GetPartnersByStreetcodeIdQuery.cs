using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;

public record GetPartnersByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<PartnerDTO>>>, ICachibleQueryPreProcessor<Result<IEnumerable<PartnerDTO>>>
{
    public object? CachedResponse { get; set; }
}
