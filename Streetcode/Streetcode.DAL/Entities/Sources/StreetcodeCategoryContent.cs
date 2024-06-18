using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Sources;

public class StreetcodeCategoryContent
{
    public string Text { get; set; } = string.Empty;

    public int SourceLinkCategoryId { get; set; }

    public int StreetcodeId { get; set; }

    public SourceLinkCategory SourceLinkCategory { get; set; } = new();
    public StreetcodeContent Streetcode { get; set; } = new();
}
