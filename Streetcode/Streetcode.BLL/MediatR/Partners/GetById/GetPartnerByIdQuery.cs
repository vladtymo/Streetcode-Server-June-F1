using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.GetById;

public record GetPartnerByIdQuery(int Id) : IRequest<Result<PartnerDTO>>, ICachibleQueryPreProcessor<Result<PartnerDTO>>
{
    public object? CachedResponse { get; set; }
}
