using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Media.Images;

public class Art
{
    public int Id { get; set; }

    public string? Description { get; set; } = string.Empty;

    public string? Title { get; set; } = string.Empty;

    public int ImageId { get; set; }

    public Image? Image { get; set; } = new();

    public List<StreetcodeArt> StreetcodeArts { get; set; } = new ();
}
