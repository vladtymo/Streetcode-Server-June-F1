using FluentResults;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Services.CacheService;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort;

public record GetAllPartnersShortQuery : ICachibleQueryPreProcessor<Result<IEnumerable<PartnerShortDTO>>>
{
    public Result<IEnumerable<PartnerShortDTO>>? CachedResponse { get; set; }
}