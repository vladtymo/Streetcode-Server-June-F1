using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Media;

public class Audio
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string BlobName { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public string Base64 { get; set; } = string.Empty;

    public StreetcodeContent Streetcode { get; set; } = new();
}