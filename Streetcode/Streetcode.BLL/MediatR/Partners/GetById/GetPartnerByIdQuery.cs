using FluentResults;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.GetById;

public record GetPartnerByIdQuery(int Id) : ICachibleQueryPreProcessor<Result<PartnerDTO>>
{
    public object? CachedResponse { get; set; }
}
