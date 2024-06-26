using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.Delete
{
    public record DeletePartnerCommand(int id) : IValidatableRequest<Result<PartnerDTO>>, ICachibleCommandPostProcessor<Result<PartnerDTO>>;
}
