using FluentResults;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort
{
    public record GetAllPartnersShortQuery : ICachibleQueryPreProcessor<Result<IEnumerable<PartnerShortDTO>>>
    {
        public object? CachedResponse { get; set; }
    }
}
