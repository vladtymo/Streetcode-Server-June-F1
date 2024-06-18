using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Entities.Timeline;

public class TimelineItem
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public DateViewPattern DateViewPattern { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; } = new();

    public List<HistoricalContextTimeline> HistoricalContextTimelines { get; set; } = new ();
}
