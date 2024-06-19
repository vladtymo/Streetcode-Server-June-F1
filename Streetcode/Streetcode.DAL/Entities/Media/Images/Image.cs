using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.DAL.Entities.Media.Images;

public class Image
{
    public int Id { get; set; }

    public string? Base64 { get; set; } = string.Empty;

    public string? BlobName { get; set; } = string.Empty;

    public string? MimeType { get; set; } = string.Empty;

    public ImageDetails? ImageDetails { get; set; }

    public List<StreetcodeContent> Streetcodes { get; set; }

    public List<Fact> Facts { get; set; }

    public Art? Art { get; set; }

    public Partner? Partner { get; set; }

    public List<SourceLinkCategory> SourceLinkCategories { get; set; }

    public News.News? News { get; set; }
    public TeamMember? TeamMember { get; set; }
}
