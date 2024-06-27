using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Team
{
    public class Positions : IEntityId<int>
    {
        public int Id { get; set; }

        public string? Position { get; set; }

        public List<TeamMember>? TeamMembers { get; set; }
    }
}
