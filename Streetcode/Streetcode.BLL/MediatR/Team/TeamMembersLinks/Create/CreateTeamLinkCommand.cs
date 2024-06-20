using FluentResults;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create
{
    public record CreateTeamLinkCommand(TeamMemberLinkDTO teamMember) : IValidatableRequest<Result<TeamMemberLinkDTO>>;
}
