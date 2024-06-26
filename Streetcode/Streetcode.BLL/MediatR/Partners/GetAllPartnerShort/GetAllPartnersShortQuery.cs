using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort
{
    public record GetAllPartnersShortQuery : IRequest<Result<IEnumerable<PartnerShortDTO>>>, ICachibleQueryPreProcessor<Result<IEnumerable<PartnerShortDTO>>>
    {
        public object? CachedResponse { get; set; }
    }
}
