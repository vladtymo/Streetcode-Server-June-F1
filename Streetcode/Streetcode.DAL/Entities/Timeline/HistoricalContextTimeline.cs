using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Timeline
{
    public class HistoricalContextTimeline : IEntity
    {
        public int HistoricalContextId { get; set; }

        public int TimelineId { get; set; }

        public HistoricalContext? HistoricalContext { get; set; }

        public TimelineItem? Timeline { get; set; }
    }
}
