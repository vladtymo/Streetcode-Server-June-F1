using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Team.Create
{
    public record CreatePositionCommand(PositionDTO position) : IValidatableRequest<Result<PositionDTO>>;
}
