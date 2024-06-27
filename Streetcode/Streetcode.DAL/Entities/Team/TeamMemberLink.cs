using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Entities.Team
{
    public class TeamMemberLink : IEntityId<int>
    {
        public int Id { get; set; }

        public LogoType LogoType { get; set; }

        public string? TargetUrl { get; set; }

        public int TeamMemberId { get; set; }

        public TeamMember? TeamMember { get; set; }
    }
}