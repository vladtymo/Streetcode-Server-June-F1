using Streetcode.DAL.Entities.Base;


namespace Streetcode.DAL.Entities.AdditionalContent;

public class Subtitle : IEntityId
{
    public int Id { get; set; }

    public string? SubtitleText { get; set; } = string.Empty;

    public int StreetcodeId { get; set; }

    public Streetcode.StreetcodeContent? Streetcode { get; set; }
}
