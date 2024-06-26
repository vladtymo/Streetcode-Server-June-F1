using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Media;

public class Audio : IEntityId
{
    public int Id { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? BlobName { get; set; } = string.Empty;

    public string? MimeType { get; set; } = string.Empty;

    public string? Base64 { get; set; } = string.Empty;

    public StreetcodeContent? Streetcode { get; set; }
}