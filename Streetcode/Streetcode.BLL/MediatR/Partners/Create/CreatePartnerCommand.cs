using FluentResults;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Partners.Create
{
  public record CreatePartnerCommand(CreatePartnerDTO newPartner) : IValidatableRequest<Result<PartnerDTO>>;
}
