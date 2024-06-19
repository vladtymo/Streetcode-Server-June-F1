using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Streetcode.TextContent;

public class Text
{
    public int Id { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? TextContent { get; set; } = string.Empty;

    public string? AdditionalText { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    public string? Author { get; set; } = string.Empty;

    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}