using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Timeline;

public class HistoricalContext : IEntityId<int>
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public List<HistoricalContextTimeline> HistoricalContextTimelines { get; set; } = new ();
}
