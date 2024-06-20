using FluentResults;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Partners.Update
{
  public record UpdatePartnerCommand(CreatePartnerDTO Partner) : IValidatableRequest<Result<PartnerDTO>>;
}
