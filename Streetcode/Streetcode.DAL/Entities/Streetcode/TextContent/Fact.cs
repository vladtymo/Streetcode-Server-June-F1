using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Streetcode.TextContent;

public class Fact : IEntityId<int>
{
    public int Id { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? FactContent { get; set; } = string.Empty;

    public int? Position { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}
