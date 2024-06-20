using FluentResults;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Streetcode.Facts.Update
{
    public record UpdateFactCommand(FactUpdateCreateDto Fact) : IValidatableRequest<Result<FactDto>>;
}
