using FluentResults;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.Services.Cache;

namespace Streetcode.BLL.MediatR.Partners.Create
{
  public record CreatePartnerCommand(CreatePartnerDTO newPartner) : IValidatableRequest<Result<PartnerDTO>>, ICachibleCommandPostProcessor<Result<PartnerDTO>>;
}
