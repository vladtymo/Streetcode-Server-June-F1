using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Sources;

public class StreetcodeCategoryContent : IEntity
{
    public string? Text { get; set; } = string.Empty;

    public int SourceLinkCategoryId { get; set; }

    public int StreetcodeId { get; set; }

    public SourceLinkCategory? SourceLinkCategory { get; set; }
    public StreetcodeContent? Streetcode { get; set; }
}
