using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Team
{
    public class TeamMemberPositions : IEntity
    {
        public int TeamMemberId { get; set; }
        public Positions? Positions { get; set; }
        public TeamMember? TeamMember { get; set; }
        public int PositionsId { get; set; }
    }
}
