using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Partners;

public class Partner
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int LogoId { get; set; }

    public bool IsKeyPartner { get; set; }

    public bool IsVisibleEverywhere { get; set; }

    public string TargetUrl { get; set; } = string.Empty;

    public string UrlTitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Image Logo { get; set; } = new();

    public List<PartnerSourceLink> PartnerSourceLinks { get; set; } = new ();

    public List<StreetcodeContent> Streetcodes { get; set; } = new ();
}